﻿using Microsoft.EntityFrameworkCore;
using SharePicker.Models.Database;
using SharePicker.Models.Fmp;

namespace SharePicker.Services;

public class DatabaseWriter(
    ILogger<DatabaseWriter> logger,
    FmpClient fmpClient,
    IDbContextFactory<SharePickerDbContext> dbContextFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromDays(1));
            var stocks = await fmpClient.GetStocksAsync(cancellationToken);
            var tradableSymbols = (await fmpClient.GetTradableSymbolsAsync(cancellationToken))
                .Select(s => s.Symbol)
                .ToHashSet();
            var symbolsWithFinancialStatements = (await fmpClient.GetSymbolsWithFinancialStatementsAsync(cancellationToken)).ToHashSet();

            var upToDateSymbols = await GetUpToDateSymbols(cancellationToken);
            
            foreach (var stock in stocks
                .Where(stock => tradableSymbols.Contains(stock.Symbol))
                .Where(stock => symbolsWithFinancialStatements.Contains(stock.Symbol)))
            {
                if (stock.ExchangeShortName != "LSE")
                    continue;
                
                if (upToDateSymbols.Contains(stock.Symbol))
                    continue;

                try
                {
                    await AddIncomeStatements(stock, cancellationToken);
                    await AddBalanceSheetStatements(stock, cancellationToken);
                    await AddCashFlowStatements(stock, cancellationToken);
                    await AddRatios(stock, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to get write data to database for company {Company}, skipping", stock.Name);
                }
            }

            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }

    private async Task AddIncomeStatements(StockDto stock, CancellationToken cancellationToken)
    {
        if (stock.ExchangeShortName == null || stock.Exchange == null)
            return;

        int yearOfMostRecentData;
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken))
        {
            yearOfMostRecentData = await dbContext.IncomeStatements
                .Where(statement => statement.Company.Symbol == stock.Symbol)
                .Select(statement => statement.Date.Year)
                .OrderBy(year => year)
                .LastOrDefaultAsync(cancellationToken);
        }

        foreach (var dto in await fmpClient.GetIncomeStatementsAsync(stock.Symbol, cancellationToken))
        {
            var date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd");
            
            if (date.Year <= yearOfMostRecentData)
                continue;
            
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var exchange = await dbContext.Exchanges.SingleOrDefaultAsync(exchange => exchange.Symbol == stock.ExchangeShortName, cancellationToken)
                ?? new Exchange { Name = stock.Exchange, Symbol = stock.ExchangeShortName };

            var company = await dbContext.Companies.SingleOrDefaultAsync(company => company.Symbol == stock.Symbol, cancellationToken)
                ?? new Company { Name = stock.Name, Symbol = stock.Symbol, Exchange = exchange };

            var currency = await dbContext.Currencies.SingleOrDefaultAsync(
                currency => currency.Symbol == dto.ReportedCurrency,
                cancellationToken) ?? new Currency { Symbol = dto.ReportedCurrency };

            await dbContext.IncomeStatements.AddAsync(
                new IncomeStatement
                {
                    Company = company,
                    Date = date,
                    Currency = currency,
                    Revenue = dto.Revenue,
                    CostOfSales = dto.CostOfRevenue,
                    GrossProfit = dto.GrossProfit,
                    ResearchAndDevelopmentCosts = dto.ResearchAndDevelopmentExpenses,
                    DistributionCosts = dto.SellingAndMarketingExpenses,
                    AdministrativeCosts = dto.GeneralAndAdministrativeExpenses,
                    OtherCosts = dto.OtherExpenses,
                    OperatingProfit = dto.OperatingIncome,
                    ProfitBeforeIncomeAndTaxation = 
                        dto.IncomeBeforeTax - dto.InterestIncome ?? 0 + dto.InterestExpense ?? 0,
                    FinanceIncome = dto.InterestIncome ?? 0,
                    FinanceExpense = dto.InterestExpense ?? 0,
                    ProfitBeforeTax = dto.IncomeBeforeTax,
                    Taxation = dto.IncomeTaxExpense,
                    ProfitAfterTax = dto.IncomeBeforeTax - dto.IncomeTaxExpense,
                    NetProfit = dto.NetIncome,
                    EarningsPerShare = dto.Eps,
                    EarningsPerShareDiluted = dto.EpsDiluted
                },
                cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Added {Year} income statement for {Company} to database", date.Year, company.Name);
        }
    }
        
    private async Task AddBalanceSheetStatements(StockDto stock, CancellationToken cancellationToken)
    {
        if (stock.ExchangeShortName == null || stock.Exchange == null)
            return;
        
        int yearOfMostRecentData;
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken))
        {
            yearOfMostRecentData = await dbContext.BalanceSheetStatements
                .Where(statement => statement.Company.Symbol == stock.Symbol)
                .Select(statement => statement.Date.Year)
                .OrderBy(year => year)
                .LastOrDefaultAsync(cancellationToken);
        }

        foreach (var dto in await fmpClient.GetBalanceSheetStatementsAsync(stock.Symbol, cancellationToken))
        {
            var date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd");
            
            if (date.Year <= yearOfMostRecentData)
                continue;
            
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var exchange = await dbContext.Exchanges.SingleOrDefaultAsync(exchange => exchange.Symbol == stock.ExchangeShortName, cancellationToken)
                ?? new Exchange { Name = stock.Exchange, Symbol = stock.ExchangeShortName };

            var company = await dbContext.Companies.SingleOrDefaultAsync(company => company.Symbol == stock.Symbol, cancellationToken)
                ?? new Company { Name = stock.Name, Symbol = stock.Symbol, Exchange = exchange };

            var currency = await dbContext.Currencies.SingleOrDefaultAsync(
                currency => currency.Symbol == dto.ReportedCurrency,
                cancellationToken) ?? new Currency { Symbol = dto.ReportedCurrency };

            await dbContext.BalanceSheetStatements.AddAsync(
                new BalanceSheetStatement
                {
                    Company = company,
                    Date = date,
                    Currency = currency,
                    CashAndCashEquivalents = dto.CashAndCashEquivalents,
                    ShortTermInvestments = dto.ShortTermInvestments,
                    NetReceivables = dto.NetReceivables,
                    Inventory = dto.Inventory,
                    OtherCurrentAssets = dto.OtherCurrentAssets,
                    TotalCurrentAssets = dto.TotalCurrentAssets,
                    PropertyPlantEquipmentNet = dto.PropertyPlantEquipmentNet,
                    Goodwill = dto.Goodwill,
                    IntangibleAssets = dto.IntangibleAssets,
                    LongTermInvestments = dto.LongTermInvestments,
                    TaxAssets = dto.TaxAssets,
                    OtherNonCurrentAssets = dto.OtherNonCurrentAssets,
                    TotalNonCurrentAssets = dto.TotalNonCurrentAssets,
                    OtherAssets = dto.OtherAssets,
                    TotalAssets = dto.TotalAssets,
                    AccountPayables = dto.AccountPayables,
                    ShortTermDebt = dto.ShortTermDebt,
                    TaxPayables = dto.TaxPayables,
                    DeferredRevenue = dto.DeferredRevenue - dto.TaxPayables,
                    OtherCurrentLiabilities = dto.OtherCurrentLiabilities,
                    TotalCurrentLiabilities = dto.TotalCurrentLiabilities,
                    LongTermDebt = dto.LongTermDebt,
                    DeferredRevenueNonCurrent = dto.DeferredRevenueNonCurrent,
                    DeferredTaxLiabilitiesNonCurrent = dto.DeferredTaxLiabilitiesNonCurrent,
                    MinorityInterest = dto.MinorityInterest,
                    CapitalLeaseObligations = dto.CapitalLeaseObligations,
                    OtherNonCurrentLiabilities = dto.OtherNonCurrentLiabilities,
                    TotalNonCurrentLiabilities = dto.TotalNonCurrentLiabilities,
                    OtherLiabilities = dto.OtherLiabilities,
                    TotalLiabilities = dto.TotalLiabilities,
                    PreferredStock = dto.PreferredStock,
                    CommonStock = dto.CommonStock,
                    RetainedEarnings = dto.RetainedEarnings,
                    AccumulatedOtherComprehensiveIncomeLoss = dto.AccumulatedOtherComprehensiveIncomeLoss,
                    OtherTotalStockholdersEquity = dto.OtherTotalStockholdersEquity,
                    TotalStockholdersEquity = dto.TotalStockholdersEquity,
                    TotalEquity = dto.TotalEquity,
                    TotalInvestments = dto.TotalInvestments,
                    TotalDebt = dto.TotalDebt,
                    NetDebt = dto.NetDebt
                },
                cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation(
                "Added {Year} balance sheet statement for {Company} to database", 
                date.Year,
                company.Name);
        }
    }        

    private async Task AddCashFlowStatements(StockDto stock, CancellationToken cancellationToken)
    {
        if (stock.ExchangeShortName == null || stock.Exchange == null)
            return;
        
        int yearOfMostRecentData;
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken))
        {
            yearOfMostRecentData = await dbContext.CashFlowStatements
                .Where(statement => statement.Company.Symbol == stock.Symbol)
                .Select(statement => statement.Date.Year)
                .OrderBy(year => year)
                .LastOrDefaultAsync(cancellationToken);
        }

        foreach (var dto in await fmpClient.GetCashFlowStatementsAsync(stock.Symbol, cancellationToken))
        {
            var date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd");
            
            if (date.Year <= yearOfMostRecentData)
                continue;
            
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var exchange = await dbContext.Exchanges.SingleOrDefaultAsync(exchange => exchange.Symbol == stock.ExchangeShortName, cancellationToken)
                ?? new Exchange { Name = stock.Exchange, Symbol = stock.ExchangeShortName };

            var company = await dbContext.Companies.SingleOrDefaultAsync(company => company.Symbol == stock.Symbol, cancellationToken)
                ?? new Company { Name = stock.Name, Symbol = stock.Symbol, Exchange = exchange };

            var currency = await dbContext.Currencies.SingleOrDefaultAsync(
                currency => currency.Symbol == dto.ReportedCurrency, 
                cancellationToken) ?? new Currency { Symbol = dto.ReportedCurrency };

            await dbContext.CashFlowStatements.AddAsync(
                new CashFlowStatement
                {
                    Company = company,
                    Date = date,
                    Currency = currency,
                    NetIncome = dto.NetIncome,
                    DepreciationAndAmortisation = dto.DepreciationAndAmortization,
                    DeferredIncomeTax = dto.DeferredIncomeTax,
                    StockBasedCompensation = dto.StockBasedCompensation,
                    AccountsReceivables = dto.AccountsReceivables,
                    Inventory = dto.Inventory,
                    AccountsPayables = dto.AccountsPayables,
                    OtherWorkingCapital = dto.OtherWorkingCapital,
                    ChangeInWorkingCapital = dto.ChangeInWorkingCapital,
                    OtherNonCashItems = dto.OtherNonCashItems,
                    NetCashFromOperations = dto.NetCashProvidedByOperatingActivities,
                    CapitalExpenditure = dto.CapitalExpenditure,
                    Acquisitions = dto.AcquisitionsNet,
                    PurchasesOfInvestments = dto.PurchasesOfInvestments,
                    SaleOrMaturityOfInvestments = dto.SalesMaturitiesOfInvestments,
                    OtherInvestingActivities = dto.OtherInvestingActivites,
                    NetCashFromInvesting = dto.NetCashUsedForInvestingActivites,
                    SharesIssued = dto.CommonStockIssued,
                    SharesRepurchased = dto.CommonStockRepurchased,
                    DebtRepayment = dto.DebtRepayment,
                    DividendsPaid = dto.DividendsPaid,
                    OtherFinancingActivities = dto.OtherFinancingActivites,
                    NetCashFromFinancing = dto.NetCashUsedProvidedByFinancingActivities,
                    NetChangeInCash = dto.NetChangeInCash
                },
                cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation(
                "Added {Year} cash flow statement for {Company} to database", 
                date.Year, 
                company.Name);
        }
    }

    private async Task AddRatios(StockDto stock, CancellationToken cancellationToken)
    {
        if (stock.ExchangeShortName == null || stock.Exchange == null)
            return;
        
        int yearOfMostRecentData;
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken))
        {
            yearOfMostRecentData = await dbContext.Ratios
                .Where(ratios => ratios.Company.Symbol == stock.Symbol)
                .Select(ratios => ratios.Date.Year)
                .OrderBy(year => year)
                .LastOrDefaultAsync(cancellationToken);
        }

        foreach (var dto in await fmpClient.GetRatiosAsync(stock.Symbol, cancellationToken))
        {
            var date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd");
            
            if (date.Year <= yearOfMostRecentData)
                continue;
            
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var exchange = await dbContext.Exchanges.SingleOrDefaultAsync(exchange => exchange.Symbol == stock.ExchangeShortName, cancellationToken)
                ?? new Exchange { Name = stock.Exchange, Symbol = stock.ExchangeShortName };

            var company = await dbContext.Companies.SingleOrDefaultAsync(company => company.Symbol == stock.Symbol, cancellationToken)
                ?? new Company { Name = stock.Name, Symbol = stock.Symbol, Exchange = exchange };

            await dbContext.Ratios.AddAsync(
                new Ratios
                {
                    Company = company,
                    Date = date,
                    CurrentRatio = dto.CurrentRatio,
                    QuickRatio = dto.QuickRatio,
                    CashRatio = dto.CashRatio,
                    DaysOfSalesOutstanding = dto.DaysOfSalesOutstanding,
                    DaysOfInventoryOutstanding = dto.DaysOfInventoryOutstanding,
                    OperatingCycle = dto.OperatingCycle,
                    DaysOfPayablesOutstanding = dto.DaysOfPayablesOutstanding,
                    CashConversionCycle = dto.CashConversionCycle,
                    GrossProfitMargin = dto.GrossProfitMargin,
                    OperatingProfitMargin = dto.OperatingProfitMargin,
                    PretaxProfitMargin = dto.PretaxProfitMargin,
                    NetProfitMargin = dto.NetProfitMargin,
                    EffectiveTaxRate = dto.EffectiveTaxRate,
                    ReturnOnAssets = dto.ReturnOnAssets,
                    ReturnOnEquity = dto.ReturnOnEquity,
                    ReturnOnCapitalEmployed = dto.ReturnOnCapitalEmployed,
                    NetIncomePerEBT = dto.NetIncomePerEBT,
                    EbtPerEbit = dto.EbtPerEbit,
                    EbitPerRevenue = dto.EbitPerRevenue,
                    DebtRatio = dto.DebtRatio,
                    DebtEquityRatio = dto.DebtEquityRatio,
                    LongTermDebtToCapitalization = dto.LongTermDebtToCapitalization,
                    TotalDebtToCapitalization = dto.TotalDebtToCapitalization,
                    InterestCoverage = dto.InterestCoverage,
                    CashFlowToDebtRatio = dto.CashFlowToDebtRatio,
                    CompanyEquityMultiplier = dto.CompanyEquityMultiplier,
                    ReceivablesTurnover = dto.ReceivablesTurnover,
                    PayablesTurnover = dto.PayablesTurnover,
                    InventoryTurnover = dto.InventoryTurnover,
                    FixedAssetTurnover = dto.FixedAssetTurnover,
                    AssetTurnover = dto.AssetTurnover,
                    OperatingCashFlowPerShare = dto.OperatingCashFlowPerShare,
                    FreeCashFlowPerShare = dto.FreeCashFlowPerShare,
                    CashPerShare = dto.CashPerShare,
                    PayoutRatio = dto.PayoutRatio,
                    OperatingCashFlowSalesRatio = dto.OperatingCashFlowSalesRatio,
                    FreeCashFlowOperatingCashFlowRatio = dto.FreeCashFlowOperatingCashFlowRatio,
                    CashFlowCoverageRatios = dto.CashFlowCoverageRatios,
                    ShortTermCoverageRatios = dto.ShortTermCoverageRatios,
                    CapitalExpenditureCoverageRatio = dto.CapitalExpenditureCoverageRatio,
                    DividendPaidAndCapexCoverageRatio = dto.DividendPaidAndCapexCoverageRatio,
                    DividendPayoutRatio = dto.DividendPayoutRatio,
                    PriceBookValueRatio = dto.PriceBookValueRatio,
                    PriceToBookRatio = dto.PriceToBookRatio,
                    PriceToSalesRatio = dto.PriceToSalesRatio,
                    PriceEarningsRatio = dto.PriceEarningsRatio,
                    PriceToFreeCashFlowsRatio = dto.PriceToFreeCashFlowsRatio,
                    PriceToOperatingCashFlowsRatio = dto.PriceToOperatingCashFlowsRatio,
                    PriceCashFlowRatio = dto.PriceCashFlowRatio,
                    PriceEarningsToGrowthRatio = dto.PriceEarningsToGrowthRatio,
                    PriceSalesRatio = dto.PriceSalesRatio,
                    DividendYield = dto.DividendYield,
                    EnterpriseValueMultiple = dto.EnterpriseValueMultiple,
                    PriceFairValue = dto.PriceFairValue
                },
                cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation("Added {Year} ratios for {Company} to database", date.Year, company.Name);
        }
    }

    private async Task<HashSet<string>> GetUpToDateSymbols(CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var yearOfLatestData = DateTime.Today.Year - 1;

        var upToDateSymbols = await dbContext.Companies
            .Where(company => 
                company.IncomeStatements.Any(statement => statement.Date.Year == yearOfLatestData)
                && company.BalanceSheetStatements.Any(statement => statement.Date.Year == yearOfLatestData) 
                && company.CashFlowStatements.Any(statement => statement.Date.Year == yearOfLatestData)
                && company.Ratios.Any(ratios => ratios.Date.Year == yearOfLatestData))
            .Select(company => company.Symbol)
            .ToListAsync(cancellationToken);

        return upToDateSymbols.ToHashSet();
    }
}
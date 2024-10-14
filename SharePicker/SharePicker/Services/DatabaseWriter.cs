using Microsoft.EntityFrameworkCore;
using SharePicker.Models.Database;
using SharePicker.Models.Fmp;

namespace SharePicker.Services;

public class DatabaseWriter(
    FmpClient fmpClient,
    IDbContextFactory<SharePickerDbContext> dbContextFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var count = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromDays(1));
            var stocks = await fmpClient.GetStocksAsync(cancellationToken);
            var tradableSymbols = (await fmpClient.GetTradableSymbolsAsync(cancellationToken))
                .Select(s => s.Symbol)
                .ToHashSet();
            var symbolsWithFinancialStatements = (await fmpClient.GetSymbolsWithFinancialStatementsAsync(cancellationToken)).ToHashSet();

            foreach (var stock in stocks
                .Where(stock => tradableSymbols.Contains(stock.Symbol))
                .Where(stock => symbolsWithFinancialStatements.Contains(stock.Symbol)))
            {
                if (stock.ExchangeShortName != "LSE")
                    continue;
                
                await AddIncomeStatements(stock, cancellationToken);
                await AddBalanceSheetStatements(stock, cancellationToken);
                await AddCashFlowStatements(stock, cancellationToken);

                if (++count > 5)
                    break;
            }

            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }

    private async Task AddIncomeStatements(StockDto stock, CancellationToken cancellationToken)
    {
        if (stock.ExchangeShortName == null || stock.Exchange == null)
            return;

        foreach (var dto in await fmpClient.GetIncomeStatementsAsync(stock.Symbol, cancellationToken))
        {
            using var dbContext = dbContextFactory.CreateDbContext();

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
                    Date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
                    Currency = currency,
                    Revenue = dto.Revenue,
                    CostOfSales = dto.CostOfRevenue,
                    GrossProfit = dto.GrossProfit,
                    ResearchAndDevelopmentCosts = dto.ResearchAndDevelopmentExpenses,
                    DistributionCosts = dto.SellingAndMarketingExpenses,
                    AdministrativeCosts = dto.GeneralAndAdministrativeExpenses,
                    OtherCosts = dto.OtherExpenses,
                    OperatingProfit = dto.OperatingIncome,
                    ProfitBeforeIncomeAndTaxation = dto.IncomeBeforeTax - dto.InterestIncome + dto.InterestExpense,
                    FinanceIncome = dto.InterestIncome,
                    FinanceExpense = dto.InterestExpense,
                    ProfitBeforeTax = dto.IncomeBeforeTax,
                    Taxation = dto.IncomeTaxExpense,
                    ProfitAfterTax = dto.IncomeBeforeTax - dto.IncomeTaxExpense,
                    NetProfit = dto.NetIncome,
                    EarningsPerShare = dto.Eps,
                    EarningsPerShareDiluted = dto.EpsDiluted
                },
                cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
        
    private async Task AddBalanceSheetStatements(StockDto stock, CancellationToken cancellationToken)
    {
        if (stock.ExchangeShortName == null || stock.Exchange == null)
            return;

        foreach (var dto in await fmpClient.GetBalanceSheetStatementsAsync(stock.Symbol, cancellationToken))
        {
            using var dbContext = dbContextFactory.CreateDbContext();

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
                    Date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
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
        }
    }        

    private async Task AddCashFlowStatements(StockDto stock, CancellationToken cancellationToken)
    {
        if (stock.ExchangeShortName == null || stock.Exchange == null)
            return;

        foreach (var dto in await fmpClient.GetCashFlowStatementsAsync(stock.Symbol, cancellationToken))
        {
            using var dbContext = dbContextFactory.CreateDbContext();

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
                    Date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
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
        }
    } 
}
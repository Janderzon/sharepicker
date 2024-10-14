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
                if (stock.ExchangeShortName == null || stock.ExchangeShortName != "LSE" || stock.Exchange == null)
                    continue;

                using var dbContext = dbContextFactory.CreateDbContext();

                if (await dbContext.Companies.AnyAsync(company => company.Symbol == stock.Symbol, cancellationToken))
                    continue;

                var exchange = await dbContext.Exchanges.SingleOrDefaultAsync(exchange => exchange.Symbol == stock.ExchangeShortName, cancellationToken)
                    ?? new Exchange { Name = stock.Exchange, Symbol = stock.ExchangeShortName };

                var company = new Company { Name = stock.Name, Symbol = stock.Symbol, Exchange = exchange };

                foreach (var incomeStatement in await fmpClient
                    .GetIncomeStatementsAsync(stock.Symbol, cancellationToken))
                {
                    await dbContext.IncomeStatements.AddAsync(
                        await ToDatabaseObject(dbContext, incomeStatement, company, cancellationToken),
                        cancellationToken);
                }

                foreach (var balanceSheetStatement in await fmpClient
                    .GetBalanceSheetStatementsAsync(stock.Symbol, cancellationToken))
                {
                    await dbContext.BalanceSheetStatements.AddAsync(
                        await ToDatabaseObject(dbContext, balanceSheetStatement, company, cancellationToken),
                        cancellationToken);
                }

                foreach (var cashFlowStatement in await fmpClient
                    .GetCashFlowStatementsAsync(stock.Symbol, cancellationToken))
                {
                    await dbContext.CashFlowStatements.AddAsync(
                        await ToDatabaseObject(dbContext, cashFlowStatement, company, cancellationToken),
                        cancellationToken);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            await timer.WaitForNextTickAsync(cancellationToken);

            if (++count > 5)
                break;
        }
    }

    private static async Task<IncomeStatement> ToDatabaseObject(
        SharePickerDbContext dbContext,
        IncomeStatementDto dto,
        Company company,
        CancellationToken cancellationToken) =>
        new IncomeStatement
        {
            Company = company,
            Date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
            Currency = await GetCurrency(dbContext, dto.ReportedCurrency, cancellationToken),
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
        };

    private static async Task<BalanceSheetStatement> ToDatabaseObject(
        SharePickerDbContext dbContext,
        BalanceSheetStatementDto dto,
        Company company,
        CancellationToken cancellationToken) =>
        new BalanceSheetStatement
        {
            Company = company,
            Date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
            Currency = await GetCurrency(dbContext, dto.ReportedCurrency, cancellationToken),
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
            DeferredRevenue = dto.DeferredRevenue,
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
        };

    private static async Task<CashFlowStatement> ToDatabaseObject(
        SharePickerDbContext dbContext,
        CashFlowStatementDto dto,
        Company company,
        CancellationToken cancellationToken) => 
        new CashFlowStatement
        {
            Company = company,
            Date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
            Currency = await GetCurrency(dbContext, dto.ReportedCurrency, cancellationToken),
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
        };

    private static async Task<Currency> GetCurrency(
        SharePickerDbContext dbContext,
        string symbol,
        CancellationToken cancellationToken) => 
        await dbContext.Currencies.SingleOrDefaultAsync(
            currency => currency.Symbol == symbol,
            cancellationToken) ?? new Currency { Symbol = symbol };
}
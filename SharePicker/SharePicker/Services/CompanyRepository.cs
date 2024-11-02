using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SharePicker.Models;

namespace SharePicker.Services;

public class CompanyRepository(
    IDbContextFactory<SharePickerDbContext> dbContextFactory,
    IMemoryCache memoryCache)
{
    [UsedImplicitly]
    private record AvailableCompaniesCacheKey;
    public async Task<List<Company>> GetAvailableCompaniesAsync(CancellationToken cancellationToken) => 
        await memoryCache.GetOrCreateAsync(
            new AvailableCompaniesCacheKey(),
            async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                return await dbContext.Companies
                    .OrderBy(x => x.Symbol)
                    .Select(dbo => ToDomain(dbo))
                    .ToListAsync(cancellationToken);
            }) ?? throw new Exception("Cache entry for available companies was null");

    public async Task<List<Company>> GetFilteredCompaniesAsync(
        ICompanyFilter filter, 
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await filter
            .FilterCompanies(dbContext.Companies)
            .OrderBy(x => x.Symbol)
            .Select(dbo => ToDomain(dbo))
            .ToListAsync(cancellationToken);
    }
    
    [UsedImplicitly]
    private record ExchangesCacheKey;
    public async Task<List<Exchange>> GetExchangeAsync(CancellationToken cancellationToken) =>
        await memoryCache.GetOrCreateAsync(
            new ExchangesCacheKey(),
            async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                return await dbContext.Exchanges
                    .Select(dbo => ToDomain(dbo))
                    .ToListAsync(cancellationToken);
            }) ?? throw new Exception();

    [UsedImplicitly]
    private record IncomeStatementsCacheKey(Company Company);
    public async Task<List<IncomeStatement>> GetIncomeStatementsAsync(
        Company company, 
        CancellationToken cancellationToken) => 
        await memoryCache.GetOrCreateAsync(
            new IncomeStatementsCacheKey(company),
            async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                return await dbContext.IncomeStatements
                   .Where(dbo => dbo.Company.Symbol == company.Symbol)
                   .OrderBy(dbo => dbo.Date)
                   .Include(dbo => dbo.Currency)
                   .Select(dbo => ToDomain(dbo))
                   .ToListAsync(cancellationToken);
            }) ?? throw new Exception($"Cache entry for income statements for company {company.Symbol} was null");

    [UsedImplicitly]
    private record BalanceSheetStatementCacheKey(Company Company);
    public async Task<List<BalanceSheetStatement>> GetBalanceSheetStatementsAsync(
        Company company, 
        CancellationToken cancellationToken) =>
        await memoryCache.GetOrCreateAsync(
            new BalanceSheetStatementCacheKey(company),
            async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                return await dbContext.BalanceSheetStatements
                    .Where(dbo => dbo.Company.Symbol == company.Symbol)
                    .OrderBy(dbo => dbo.Date)
                    .Include(dbo => dbo.Currency)
                    .Select(dbo => ToDomain(dbo))
                    .ToListAsync(cancellationToken);
            }) ?? throw new Exception($"Cache entry for balance sheet statements for company {company.Symbol} was null");

    [UsedImplicitly]
    private record CashFlowStatementsCacheKey(Company Company);
    public async Task<List<CashFlowStatement>> GetCashFlowStatementsAsync(
        Company company,
        CancellationToken cancellationToken) =>
        await memoryCache.GetOrCreateAsync(
            new CashFlowStatementsCacheKey(company),
            async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                return await dbContext.CashFlowStatements
                    .Where(dbo => dbo.Company.Symbol == company.Symbol)
                    .OrderBy(dbo => dbo.Date)
                    .Include(dbo => dbo.Currency)
                    .Select(dbo => ToDomain(dbo))
                    .ToListAsync(cancellationToken);
            }) ?? throw new Exception($"Cache entry for cash flow statements for company {company.Symbol} was null");

    [UsedImplicitly]
    private record RatiosCacheKey(Company Company);
    public async Task<List<Ratios>> GetRatiosAsync(Company company, CancellationToken cancellationToken) =>
        await memoryCache.GetOrCreateAsync(
            new RatiosCacheKey(company),
            async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

                return await dbContext.Ratios
                    .Where(dbo => dbo.Company.Symbol == company.Symbol)
                    .OrderBy(dbo => dbo.Date)
                    .Select(dbo => ToDomain(dbo))
                    .ToListAsync(cancellationToken);
            }) ?? throw new Exception($"Cache entry for ratios for company {company.Symbol} was null");

    private static Company ToDomain(Models.Database.Company dbo) => new(dbo.Symbol, dbo.Name);

    private static Exchange ToDomain(Models.Database.Exchange dbo) => new(dbo.Symbol, dbo.Name);

    private static IncomeStatement ToDomain(Models.Database.IncomeStatement dbo) => new(
        dbo.Date,
        dbo.Currency.Symbol,
        dbo.Revenue,
        dbo.CostOfSales,
        dbo.GrossProfit,
        dbo.ResearchAndDevelopmentCosts,
        dbo.DistributionCosts,
        dbo.AdministrativeCosts,
        dbo.OtherCosts,
        dbo.OperatingProfit,
        dbo.ProfitBeforeIncomeAndTaxation,
        dbo.FinanceIncome,
        dbo.FinanceExpense,
        dbo.ProfitBeforeTax,
        dbo.Taxation,
        dbo.ProfitAfterTax,
        dbo.NetProfit,
        dbo.EarningsPerShare,
        dbo.EarningsPerShareDiluted);

    private static BalanceSheetStatement ToDomain(Models.Database.BalanceSheetStatement dbo) => new(
        dbo.Date,
        dbo.Currency.Symbol,
        new Assets(
            new CurrentAssets(
                dbo.CashAndCashEquivalents,
                dbo.ShortTermInvestments,
                dbo.NetReceivables,
                dbo.Inventory,
                dbo.OtherCurrentAssets,
                dbo.TotalCurrentAssets),
            new NonCurrentAssets(
                dbo.PropertyPlantEquipmentNet,
                dbo.Goodwill,
                dbo.IntangibleAssets,
                dbo.LongTermInvestments,
                dbo.TaxAssets,
                dbo.OtherNonCurrentAssets,
                dbo.TotalNonCurrentAssets),
            dbo.OtherAssets,
            dbo.TotalAssets),
        new Liabilities(
            new CurrentLiabilities(
                dbo.AccountPayables,
                dbo.ShortTermDebt,
                dbo.TaxPayables,
                dbo.DeferredRevenue,
                dbo.OtherCurrentLiabilities,
                dbo.TotalCurrentLiabilities),
            new NonCurrentLiabilities(
                dbo.LongTermDebt,
                dbo.DeferredRevenueNonCurrent,
                dbo.DeferredTaxLiabilitiesNonCurrent,
                dbo.MinorityInterest,
                dbo.CapitalLeaseObligations,
                dbo.OtherNonCurrentLiabilities,
                dbo.TotalNonCurrentLiabilities),
            dbo.OtherLiabilities,
            dbo.TotalLiabilities),
        new Equity(
            dbo.PreferredStock,
            dbo.CommonStock,
            dbo.RetainedEarnings,
            dbo.AccumulatedOtherComprehensiveIncomeLoss,
            dbo.OtherTotalStockholdersEquity,
            dbo.TotalStockholdersEquity,
            dbo.TotalEquity),
        new BalanceSheetSummary(
            dbo.TotalInvestments,
            dbo.TotalDebt,
            dbo.NetDebt));

    private static CashFlowStatement ToDomain(Models.Database.CashFlowStatement dbo) => new(
        dbo.Date,
        dbo.Currency.Symbol,
        new OperationsCashFlow(
            dbo.NetIncome,
            dbo.DepreciationAndAmortisation,
            dbo.DeferredIncomeTax,
            dbo.StockBasedCompensation,
            dbo.AccountsReceivables,
            dbo.Inventory,
            dbo.AccountsPayables,
            dbo.OtherWorkingCapital,
            dbo.ChangeInWorkingCapital,
            dbo.OtherNonCashItems,
            dbo.NetCashFromOperations),
        new InvestingCashFlow(
            dbo.CapitalExpenditure,
            dbo.Acquisitions,
            dbo.PurchasesOfInvestments,
            dbo.SaleOrMaturityOfInvestments,
            dbo.OtherInvestingActivities,
            dbo.NetCashFromInvesting),
        new FinancingCashFlow(
            dbo.SharesIssued,
            dbo.SharesRepurchased,
            dbo.DebtRepayment,
            dbo.DividendsPaid,
            dbo.OtherFinancingActivities,
            dbo.NetCashFromFinancing),
        dbo.NetChangeInCash);

    private static Ratios ToDomain(Models.Database.Ratios dbo) => new(
        dbo.Date,
        dbo.CurrentRatio,
        dbo.QuickRatio,
        dbo.CashRatio,
        dbo.DaysOfSalesOutstanding,
        dbo.DaysOfInventoryOutstanding,
        dbo.OperatingCycle,
        dbo.DaysOfPayablesOutstanding,
        dbo.CashConversionCycle,
        dbo.GrossProfitMargin,
        dbo.OperatingProfitMargin,
        dbo.PretaxProfitMargin,
        dbo.NetProfitMargin,
        dbo.EffectiveTaxRate,
        dbo.ReturnOnAssets,
        dbo.ReturnOnEquity,
        dbo.ReturnOnCapitalEmployed,
        dbo.NetIncomePerEBT,
        dbo.EbtPerEbit,
        dbo.EbitPerRevenue,
        dbo.DebtRatio,
        dbo.DebtEquityRatio,
        dbo.LongTermDebtToCapitalization,
        dbo.TotalDebtToCapitalization,
        dbo.InterestCoverage,
        dbo.CashFlowToDebtRatio,
        dbo.CompanyEquityMultiplier,
        dbo.ReceivablesTurnover,
        dbo.PayablesTurnover,
        dbo.InventoryTurnover,
        dbo.FixedAssetTurnover,
        dbo.AssetTurnover,
        dbo.OperatingCashFlowPerShare,
        dbo.FreeCashFlowPerShare,
        dbo.CashPerShare,
        dbo.PayoutRatio,
        dbo.OperatingCashFlowSalesRatio,
        dbo.FreeCashFlowOperatingCashFlowRatio,
        dbo.CashFlowCoverageRatios,
        dbo.ShortTermCoverageRatios,
        dbo.CapitalExpenditureCoverageRatio,
        dbo.DividendPaidAndCapexCoverageRatio,
        dbo.DividendPayoutRatio,
        dbo.PriceBookValueRatio,
        dbo.PriceToBookRatio,
        dbo.PriceToSalesRatio,
        dbo.PriceEarningsRatio,
        dbo.PriceToFreeCashFlowsRatio,
        dbo.PriceToOperatingCashFlowsRatio,
        dbo.PriceCashFlowRatio,
        dbo.PriceEarningsToGrowthRatio,
        dbo.PriceSalesRatio,
        dbo.DividendYield,
        dbo.EnterpriseValueMultiple,
        dbo.PriceFairValue);
}

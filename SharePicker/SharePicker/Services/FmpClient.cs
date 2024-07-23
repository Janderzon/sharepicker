using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SharePicker.Models;
using SharePicker.Models.Options;
using System.Collections.Concurrent;

namespace SharePicker.Components;

public class FmpClient(
    IOptions<FmpClientOptions> fmpClientOptions,
    HttpClient httpClient,
    IMemoryCache memoryCache) : IDisposable
{
    private record TradableCompaniesCacheKey();
    public async Task<HashSet<Company>> GetTradableCompaniesAsync(CancellationToken cancellationToken) =>
        await GetFromOrPopulateCache(
            new TradableCompaniesCacheKey(),
            async () =>
            {
                var dtos = await GetWithAuth<List<TradableCompanyDto>>("available-traded/list", cancellationToken);

                return dtos
                    .Where(dto => dto.ExchangeShortName != null)
                    .Select(dto => new Company(
                        dto.Symbol,
                        dto.Name,
                        new Exchange(dto.ExchangeShortName ?? throw new Exception("Exchange short name should not be null"))))
                    .ToHashSet();
            });

    private record SymbolsWithFinancialStatementseCacheKey();
    public Task<HashSet<string>> GetSymbolsWithFinancialStatementsAsync(CancellationToken cancellationToken) =>
        GetFromOrPopulateCache(
            new SymbolsWithFinancialStatementseCacheKey(),
            () => GetWithAuth<HashSet<string>>("financial-statement-symbol-lists", cancellationToken));

    private record IncomeStatementsCacheKey(Company Company);
    public async Task<List<IncomeStatement>> GetIncomeStatementsAsync(
        Company company,
        CancellationToken cancellationToken) => await GetFromOrPopulateCache(
            new IncomeStatementsCacheKey(company),
            async () =>
            {
                var dtos = await GetWithAuth<List<IncomeStatementDto>>(
                    $"income-statement/{company.Symbol}",
                    new Dictionary<string, string?>() { { "period", "annual" } },
                    cancellationToken);

                return dtos
                    .Select(dto => new IncomeStatement(
                        DateTimeOffset.ParseExact(dto.Date, "yyyy-MM-dd", null),
                        dto.EbitDa - dto.DepreciationAndAmortization,
                        dto.Revenue,
                        dto.GrossProfit,
                        dto.OperatingIncome,
                        dto.IncomeBeforeTax,
                        dto.IncomeBeforeTax - dto.IncomeTaxExpense,
                        dto.Eps,
                        dto.EpsDiluted))
                    .ToList();
            });

    private record BalanceSheetStatementsCacheKey(Company Company);
    public async Task<List<BalanceSheetStatement>> GetBalanceSheetStatementsAsync(
        Company company,
        CancellationToken cancellationToken) => await GetFromOrPopulateCache(
            new BalanceSheetStatementsCacheKey(company),
            async () =>
            {
                var dtos = await GetWithAuth<List<BalanceSheetStatementDto>>(
                    $"balance-sheet-statement/{company.Symbol}",
                    new Dictionary<string, string?>() { { "period", "annual" } },
                    cancellationToken);

                return dtos
                    .Select(dto => new BalanceSheetStatement(
                        DateTimeOffset.ParseExact(dto.Date, "yyyy-MM-dd", null),
                        new Assets(
                            new CurrentAssets(
                                dto.CashAndCashEquivalents,
                                dto.ShortTermInvestments,
                                dto.CashAndShortTermInvestments,
                                dto.NetReceivables,
                                dto.Inventory,
                                dto.OtherCurrentAssets,
                                dto.TotalCurrentAssets),
                            new NonCurrentAssets(
                                dto.PropertyPlantEquipmentNet,
                                dto.Goodwill,
                                dto.IntangibleAssets,
                                dto.GoodwillAndIntangibleAssets,
                                dto.LongTermInvestments,
                                dto.TaxAssets,
                                dto.OtherNonCurrentAssets,
                                dto.TotalNonCurrentAssets),
                            dto.OtherAssets,
                            dto.TotalAssets),
                        new Liabilities(
                            new CurrentLiabilities(
                                dto.AccountPayables,
                                dto.ShortTermDebt,
                                dto.TaxPayables,
                                dto.DeferredRevenue,
                                dto.OtherCurrentLiabilities,
                                dto.TotalCurrentLiabilities),
                            new NonCurrentLiabilities(
                                dto.LongTermDebt,
                                dto.DeferredRevenueNonCurrent,
                                dto.DeferredTaxLiabilitiesNonCurrent,
                                dto.OtherNonCurrentLiabilities,
                                dto.TotalNonCurrentLiabilities),
                            dto.OtherLiabilities,
                            dto.CapitalLeaseObligations,
                            dto.TotalLiabilities),
                        new Equity(
                            dto.PreferredStock,
                            dto.CommonStock,
                            dto.RetainedEarnings,
                            dto.AccumulatedOtherComprehensiveIncomeLoss,
                            dto.OtherTotalStockholdersEquity,
                            dto.TotalStockholdersEquity,
                            dto.TotalEquity),
                        new BalanceSheetSummary(
                            dto.TotalLiabilitiesAndStockholdersEquity,
                            dto.MinorityInterest,
                            dto.TotalLiabilitiesAndTotalEquity,
                            dto.TotalInvestments,
                            dto.TotalDebt,
                            dto.NetDebt)))
                    .ToList();
            });

    private record CashFlowStatementsCacheKey(Company Company);
    public async Task<List<CashFlowStatement>> GetCashFlowStatementsAsync(
        Company company,
        CancellationToken cancellationToken) => await GetFromOrPopulateCache(
            new CashFlowStatementsCacheKey(company),
            async () =>
            {
                var dtos = await GetWithAuth<List<CashFlowStatementDto>>(
                    $"cash-flow-statement/{company.Symbol}",
                    new Dictionary<string, string?>() { { "period", "annual" } },
                    cancellationToken);

                return dtos
                    .Select(dto => new CashFlowStatement(
                        DateTimeOffset.ParseExact(dto.Date, "yyyy-MM-dd", null),
                        new OperationsCashFlow(
                            dto.NetIncome,
                            dto.DepreciationAndAmortization,
                            dto.StockBasedCompensation,
                            dto.Inventory,
                            dto.AccountsReceivables,
                            dto.AccountsPayables,
                            dto.OtherWorkingCapital,
                            dto.ChangeInWorkingCapital,
                            dto.OtherNonCashItems,
                            dto.OperatingCashFlow,
                            dto.DeferredIncomeTax,
                            dto.NetCashProvidedByOperatingActivities),
                        new InvestingCashFlow(
                            dto.CapitalExpenditure,
                            dto.InvestmentsInPropertyPlantAndEquipment,
                            dto.AcquisitionsNet,
                            dto.PurchasesOfInvestments,
                            0,
                            dto.SalesMaturitiesOfInvestments,
                            0,
                            dto.OtherInvestingActivites,
                            dto.NetCashUsedForInvestingActivites),
                        new FinancingCashFlow(
                            dto.CommonStockIssued,
                            dto.CommonStockRepurchased,
                            0,
                            dto.DebtRepayment,
                            dto.DividendsPaid,
                            0,
                            0,
                            dto.OtherFinancingActivites,
                            dto.NetCashUsedProvidedByFinancingActivities),
                        dto.NetChangeInCash))
                    .ToList();
            });

    private record RatiosCacheKey(Company Company);
    public async Task<List<Ratios>> GetRatiosAsync(Company company, CancellationToken cancellationToken) =>
        await GetFromOrPopulateCache(
            new RatiosCacheKey(company),
            async () =>
            {
                var dtos = await GetWithAuth<List<RatiosDto>>(
                    $"ratios/{company.Symbol}",
                    new Dictionary<string, string?>() { { "period", "annual" } },
                    cancellationToken);

                return dtos
                    .Select(dto => new Ratios(
                        DateTimeOffset.ParseExact(dto.Date, "yyyy-MM-dd", null),
                        dto.ReturnOnCapitalEmployed,
                        dto.EbitPerRevenue))
                    .ToList();
            });

    public void Dispose()
    {
        httpClient.Dispose();
        _lockCreationLock.Dispose();
        foreach (var cacheLock in _cacheLocks.Values)
        {
            cacheLock.Dispose();
        }
    }

    private readonly SemaphoreSlim _lockCreationLock = new(1, 1);
    private readonly ConcurrentDictionary<object, SemaphoreSlim> _cacheLocks = new();
    private async Task<TValue> GetFromOrPopulateCache<TKey, TValue>(TKey key, Func<Task<TValue>> valueFactory)
        where TKey : notnull where TValue : notnull
    {
        if (memoryCache.TryGetValue<TValue>(key, out var cachedValue))
            return cachedValue ?? throw new Exception("Cached value was null");

        if (!_cacheLocks.TryGetValue(key, out var cacheLock))
        {
            await _lockCreationLock.WaitAsync();
            try
            {
                cacheLock = _cacheLocks.GetOrAdd(key, new SemaphoreSlim(1, 1));
            }
            finally
            {
                _lockCreationLock.Release();
            }
        }

        if (memoryCache.TryGetValue(key, out cachedValue))
            return cachedValue ?? throw new Exception("Cached value was null");

        await cacheLock.WaitAsync();
        try
        {
            if (memoryCache.TryGetValue(key, out cachedValue))
                return cachedValue ?? throw new Exception("Cached value was null");

            return await memoryCache.GetOrCreateAsync(
                key,
                cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = fmpClientOptions.Value.CacheExpiry;

                    return valueFactory();
                }) ?? throw new Exception("Result of getting or creating value for cache was null");
        }
        finally
        {
            cacheLock.Release();
        }
    }

    private Task<T> GetWithAuth<T>(string url, CancellationToken cancellationToken) =>
        GetWithAuth<T>(url, new Dictionary<string, string?>(), cancellationToken);

    private async Task<T> GetWithAuth<T>(
        string url,
        IReadOnlyDictionary<string, string?> parameters,
        CancellationToken cancellationToken)
    {
        var parametersWithApiKey = parameters.Append(
            KeyValuePair.Create<string, string?>("apikey", fmpClientOptions.Value.ApiKey));

        return await httpClient.GetFromJsonAsync<T>(
            QueryHelpers.AddQueryString(url, parametersWithApiKey),
            cancellationToken) ?? throw new Exception("Json deserialised to null");
    }

    private class TradableCompanyDto
    {
        public required string Symbol { get; init; }

        public required string Name { get; init; }

        public string? Exchange { get; init; }

        public string? ExchangeShortName { get; init; }
    }

    private class IncomeStatementDto
    {
        public required string Date { get; init; }

        public required string ReportedCurrency { get; init; }

        public required decimal Revenue { get; init; }

        public required decimal GrossProfit { get; init; }

        public required decimal DepreciationAndAmortization { get; init; }

        public required decimal EbitDa { get; init; }

        public required decimal OperatingIncome { get; init; }

        public required decimal IncomeBeforeTax { get; init; }

        public required decimal IncomeTaxExpense { get; init; }

        public required decimal Eps { get; init; }

        public required decimal EpsDiluted { get; init; }
    }

    private class BalanceSheetStatementDto
    {
        public required string Date { get; init; }

        public required decimal CashAndCashEquivalents { get; init; }

        public required decimal ShortTermInvestments { get; init; }

        public required decimal CashAndShortTermInvestments { get; init; }

        public required decimal NetReceivables { get; init; }

        public required decimal Inventory { get; init; }

        public required decimal OtherCurrentAssets { get; init; }

        public required decimal TotalCurrentAssets { get; init; }

        public required decimal PropertyPlantEquipmentNet { get; init; }

        public required decimal Goodwill { get; init; }

        public required decimal IntangibleAssets { get; init; }

        public required decimal GoodwillAndIntangibleAssets { get; init; }

        public required decimal LongTermInvestments { get; init; }

        public required decimal TaxAssets { get; init; }

        public required decimal OtherNonCurrentAssets { get; init; }

        public required decimal TotalNonCurrentAssets { get; init; }

        public required decimal OtherAssets { get; init; }

        public required decimal TotalAssets { get; init; }

        public required decimal AccountPayables { get; init; }

        public required decimal ShortTermDebt { get; init; }

        public required decimal TaxPayables { get; init; }

        public required decimal DeferredRevenue { get; init; }

        public required decimal OtherCurrentLiabilities { get; init; }

        public required decimal TotalCurrentLiabilities { get; init; }

        public required decimal LongTermDebt { get; init; }

        public required decimal DeferredRevenueNonCurrent { get; init; }

        public required decimal DeferredTaxLiabilitiesNonCurrent { get; init; }

        public required decimal OtherNonCurrentLiabilities { get; init; }

        public required decimal TotalNonCurrentLiabilities { get; init; }

        public required decimal OtherLiabilities { get; init; }

        public required decimal CapitalLeaseObligations { get; init; }

        public required decimal TotalLiabilities { get; init; }

        public required decimal PreferredStock { get; init; }

        public required decimal CommonStock { get; init; }

        public required decimal RetainedEarnings { get; init; }

        public required decimal AccumulatedOtherComprehensiveIncomeLoss { get; init; }

        public required decimal OtherTotalStockholdersEquity { get; init; }

        public required decimal TotalStockholdersEquity { get; init; }

        public required decimal TotalEquity { get; init; }

        public required decimal TotalLiabilitiesAndStockholdersEquity { get; init; }

        public required decimal MinorityInterest { get; init; }

        public required decimal TotalLiabilitiesAndTotalEquity { get; init; }

        public required decimal TotalInvestments { get; init; }

        public required decimal TotalDebt { get; init; }

        public required decimal NetDebt { get; init; }
    }

    private class CashFlowStatementDto
    {
        public required string Date { get; init; }

        public required decimal NetIncome { get; init; }

        public required decimal DepreciationAndAmortization { get; init; }

        public required decimal DeferredIncomeTax { get; init; }

        public required decimal StockBasedCompensation { get; init; }

        public required decimal ChangeInWorkingCapital { get; init; }

        public required decimal AccountsReceivables { get; init; }

        public required decimal Inventory { get; init; }

        public required decimal AccountsPayables { get; init; }

        public required decimal OtherWorkingCapital { get; init; }

        public required decimal OtherNonCashItems { get; init; }

        public required decimal NetCashProvidedByOperatingActivities { get; init; }

        public required decimal InvestmentsInPropertyPlantAndEquipment { get; init; }

        public required decimal AcquisitionsNet { get; init; }

        public required decimal PurchasesOfInvestments { get; init; }

        public required decimal SalesMaturitiesOfInvestments { get; init; }

        public required decimal OtherInvestingActivites { get; init; }

        public required decimal NetCashUsedForInvestingActivites { get; init; }

        public required decimal DebtRepayment { get; init; }

        public required decimal CommonStockIssued { get; init; }

        public required decimal CommonStockRepurchased { get; init; }

        public required decimal DividendsPaid { get; init; }

        public required decimal OtherFinancingActivites { get; init; }

        public required decimal NetCashUsedProvidedByFinancingActivities { get; init; }

        public required decimal NetChangeInCash { get; init; }

        public required decimal OperatingCashFlow { get; init; }

        public required decimal CapitalExpenditure { get; init; }
    }

    private class RatiosDto
    {
        public required string Date { get; init; }

        public required decimal CurrentRatio { get; init; }

        public required decimal QuickRatio { get; init; }

        public required decimal CashRatio { get; init; }

        public required decimal DaysOfSalesOutstanding { get; init; }

        public required decimal DaysOfInventoryOutstanding { get; init; }

        public required decimal OperatingCycle { get; init; }

        public required decimal DaysOfPayablesOutstanding { get; init; }

        public required decimal CashConversionCycle { get; init; }

        public required decimal GrossProfitMargin { get; init; }

        public required decimal OperatingProfitMargin { get; init; }

        public required decimal PretaxProfitMargin { get; init; }

        public required decimal NetProfitMargin { get; init; }

        public required decimal EffectiveTaxRate { get; init; }

        public required decimal ReturnOnAssets { get; init; }

        public required decimal ReturnOnEquity { get; init; }

        public required decimal ReturnOnCapitalEmployed { get; init; }

        public required decimal NetIncomePerEBT { get; init; }

        public required decimal EbtPerEbit { get; init; }

        public required decimal EbitPerRevenue { get; init; }

        public required decimal DebtRatio { get; init; }

        public required decimal DebtEquityRatio { get; init; }

        public required decimal LongTermDebtToCapitalization { get; init; }

        public required decimal TotalDebtToCapitalization { get; init; }

        public required decimal InterestCoverage { get; init; }

        public required decimal CashFlowToDebtRatio { get; init; }

        public required decimal CompanyEquityMultiplier { get; init; }

        public required decimal ReceivablesTurnover { get; init; }

        public required decimal PayablesTurnover { get; init; }

        public required decimal InventoryTurnover { get; init; }

        public required decimal FixedAssetTurnover { get; init; }

        public required decimal AssetTurnover { get; init; }

        public required decimal OperatingCashFlowPerShare { get; init; }

        public required decimal FreeCashFlowPerShare { get; init; }

        public required decimal CashPerShare { get; init; }

        public required decimal PayoutRatio { get; init; }

        public required decimal OperatingCashFlowSalesRatio { get; init; }

        public required decimal FreeCashFlowOperatingCashFlowRatio { get; init; }

        public required decimal CashFlowCoverageRatios { get; init; }

        public required decimal ShortTermCoverageRatios { get; init; }

        public required decimal CapitalExpenditureCoverageRatio { get; init; }

        public required decimal DividendPaidAndCapexCoverageRatio { get; init; }

        public required decimal DividendPayoutRatio { get; init; }

        public required decimal PriceBookValueRatio { get; init; }

        public required decimal PriceToBookRatio { get; init; }

        public required decimal PriceToSalesRatio { get; init; }

        public required decimal PriceEarningsRatio { get; init; }

        public required decimal PriceToFreeCashFlowsRatio { get; init; }

        public required decimal PriceToOperatingCashFlowsRatio { get; init; }

        public required decimal PriceCashFlowRatio { get; init; }

        public required decimal PriceEarningsToGrowthRatio { get; init; }

        public required decimal PriceSalesRatio { get; init; }

        public required decimal DividendYield { get; init; }

        public required decimal EnterpriseValueMultiple { get; init; }

        public required decimal PriceFairValue { get; init; }
    }
}

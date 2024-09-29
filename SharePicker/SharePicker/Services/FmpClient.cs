using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SharePicker.Models;
using SharePicker.Models.Options;

namespace SharePicker.Services;

public class FmpClient(IOptions<FmpClientOptions> fmpClientOptions, HttpClient httpClient)
{
    public async Task<HashSet<string>> GetTradableCompaniesAsync(CancellationToken cancellationToken)
    {
        var dtos = await GetWithAuth<List<TradableCompanyDto>>("available-traded/list", cancellationToken);

        return dtos
            .Where(dto => dto.ExchangeShortName != null)
            .Select(dto => new Company(
                dto.Symbol,
                dto.Name,
                new Exchange(dto.ExchangeShortName ?? throw new Exception("Exchange short name should not be null"))))
            .ToHashSet();
    }

    public Task<HashSet<string>> GetSymbolsWithFinancialStatementsAsync(CancellationToken cancellationToken) =>
        GetWithAuth<HashSet<string>>("financial-statement-symbol-lists", cancellationToken);

    public async Task<List<IncomeStatement>> GetIncomeStatementsAsync(
        Company company,
        CancellationToken cancellationToken)
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
    }

    public async Task<List<BalanceSheetStatement>> GetBalanceSheetStatementsAsync(
        Company company,
        CancellationToken cancellationToken)
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
    }

    public async Task<List<CashFlowStatement>> GetCashFlowStatementsAsync(
        Company company,
        CancellationToken cancellationToken)
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
    }

    public async Task<List<Ratios>> GetRatiosAsync(Company company, CancellationToken cancellationToken)
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

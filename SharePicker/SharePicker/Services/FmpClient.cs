using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SharePicker.Models;
using SharePicker.Models.Fmp;
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
            .Select(dto => dto.ToDomain())
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
            .Select(dto => dto.ToDomain())
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
            .Select(dto => dto.ToDomain())
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

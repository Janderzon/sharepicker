using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SharePicker.Models;
using SharePicker.Models.Fmp;
using SharePicker.Models.Options;

namespace SharePicker.Services;

public class FmpClient(IOptions<FmpClientOptions> fmpClientOptions, HttpClient httpClient)
{
    public Task<List<TradableCompanyDto>> GetTradableCompaniesAsync(CancellationToken cancellationToken) => 
        GetWithAuth<List<TradableCompanyDto>>("available-traded/list", cancellationToken);

    public Task<HashSet<string>> GetSymbolsWithFinancialStatementsAsync(CancellationToken cancellationToken) =>
        GetWithAuth<HashSet<string>>("financial-statement-symbol-lists", cancellationToken);

    public Task<List<IncomeStatementDto>> GetIncomeStatementsAsync(Company company, CancellationToken cancellationToken) =>
        GetWithAuth<List<IncomeStatementDto>>(
            $"income-statement/{company.Symbol}",
            new Dictionary<string, string?>() { { "period", "annual" } },
            cancellationToken);

    public Task<List<BalanceSheetStatementDto>> GetBalanceSheetStatementsAsync(Company company, CancellationToken cancellationToken) => 
        GetWithAuth<List<BalanceSheetStatementDto>>(
            $"balance-sheet-statement/{company.Symbol}",
            new Dictionary<string, string?>() { { "period", "annual" } },
            cancellationToken);

    public Task<List<CashFlowStatementDto>> GetCashFlowStatementsAsync(Company company, CancellationToken cancellationToken) =>
        GetWithAuth<List<CashFlowStatementDto>>(
            $"cash-flow-statement/{company.Symbol}",
            new Dictionary<string, string?>() { { "period", "annual" } },
            cancellationToken);

    public Task<List<RatiosDto>> GetRatiosAsync(Company company, CancellationToken cancellationToken) => 
        GetWithAuth<List<RatiosDto>>(
            $"ratios/{company.Symbol}",
            new Dictionary<string, string?>() { { "period", "annual" } },
            cancellationToken);

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
}

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SharePicker.Models.Fmp;
using SharePicker.Models.Options;

namespace SharePicker.Services;

public class FmpClient(IOptions<FmpClientOptions> fmpClientOptions, HttpClient httpClient)
{
    public Task<List<StockDto>> GetStocksAsync(CancellationToken cancellationToken) =>
        GetWithAuth<List<StockDto>>("stock/list", cancellationToken);

    public Task<List<TradableSymbolDto>> GetTradableSymbolsAsync(CancellationToken cancellationToken) => 
        GetWithAuth<List<TradableSymbolDto>>("available-traded/list", cancellationToken);

    public Task<List<string>> GetSymbolsWithFinancialStatementsAsync(CancellationToken cancellationToken) =>
        GetWithAuth<List<string>>("financial-statement-symbol-lists", cancellationToken);

    public Task<List<IncomeStatementDto>> GetIncomeStatementsAsync(string symbol, CancellationToken cancellationToken) =>
        GetWithAuth<List<IncomeStatementDto>>(
            $"income-statement/{symbol}",
            new Dictionary<string, string?>() { { "period", "annual" } },
            cancellationToken);

    public Task<List<BalanceSheetStatementDto>> GetBalanceSheetStatementsAsync(string symbol, CancellationToken cancellationToken) => 
        GetWithAuth<List<BalanceSheetStatementDto>>(
            $"balance-sheet-statement/{symbol}",
            new Dictionary<string, string?>() { { "period", "annual" } },
            cancellationToken);

    public Task<List<CashFlowStatementDto>> GetCashFlowStatementsAsync(string symbol, CancellationToken cancellationToken) =>
        GetWithAuth<List<CashFlowStatementDto>>(
            $"cash-flow-statement/{symbol}",
            new Dictionary<string, string?>() { { "period", "annual" } },
            cancellationToken);

    public Task<List<RatiosDto>> GetRatiosAsync(string symbol, CancellationToken cancellationToken) => 
        GetWithAuth<List<RatiosDto>>(
            $"ratios/{symbol}",
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

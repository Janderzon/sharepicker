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
            .Select(dto => dto.ToDomain())
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
}

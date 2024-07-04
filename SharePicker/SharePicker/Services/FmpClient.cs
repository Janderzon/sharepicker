using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SharePicker.Models;
using SharePicker.Models.Options;

namespace SharePicker.Components;

public class FmpClient(IOptions<FmpClientOptions> fmpClientOptions, HttpClient httpClient) : IDisposable
{
    public async Task<HashSet<Company>> GetTradableCompaniesAsync(CancellationToken cancellationToken)
    {
        var dtos = await GetWithAuth<List<TradableCompanyDto>>("available-traded/list", cancellationToken);

        return dtos
            .Select(dto => new Company(
                dto.Symbol,
                dto.Name,
                new Exchange(dto.Exchange ?? string.Empty, dto.ExchangeShortName ?? string.Empty)))
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
                dto.TotalAssets,
                dto.TotalCurrentLiabilities,
                dto.ShortTermDebt))
            .ToList();
    }

    public async Task<List<Ratios>> GetRatiosAsync(
        Company company,
        CancellationToken cancellationToken)
    {
        var dtos = await GetWithAuth<List<RatiosDto>>(
            $"ratios/{company.Symbol}",
            new Dictionary<string, string?>() { { "period", "annual" } },
            cancellationToken);

        return dtos
            .Select(dto => new Ratios(
                DateTimeOffset.ParseExact(dto.Date, "yyyy-MM-dd", null),
                dto.EbitPerRevenue,
                dto.ReturnOnCapitalEmployed))
            .ToList();
    }

    public void Dispose() => httpClient.Dispose();

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

        public required decimal TotalCurrentLiabilities { get; init; }

        public required decimal ShortTermDebt { get; init; }

        public required decimal TotalAssets { get; init; }
    }

    private class RatiosDto
    {
        public required string Date { get; init; }

        public required decimal EbitPerRevenue { get; init; }

        public required decimal ReturnOnCapitalEmployed { get; init; }
    }
}

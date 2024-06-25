using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SharePicker.Models;
using SharePicker.Models.Options;
using System.ComponentModel.DataAnnotations;

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
                new Exchange(dto.Exchange, dto.ExchangeShortName)))
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
                dto.EbitDa - dto.DepreciationAndAmortization))
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
        [Required]
        public required string Symbol { get; init; }

        [Required]
        public required string Name { get; init; }

        [Required]
        public required string Exchange { get; init; }

        [Required]
        public required string ExchangeShortName { get; init; }
    }

    private class IncomeStatementDto
    {
        [Required]
        public required string Date { get; init; }

        [Required]
        public required decimal DepreciationAndAmortization { get; init; }

        [Required]
        public required decimal EbitDa { get; init; }
    }
}

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SharePicker.Models;
using SharePicker.Models.Options;
using System.ComponentModel.DataAnnotations;

namespace SharePicker.Components;

public class FmpClient(IOptions<FmpClientOptions> fmpClientOptions, HttpClient httpClient) : IDisposable
***REMOVED***
    public async Task<IncomeStatement> GetIncomeStatementAsync(
        Company company,
        CancellationToken cancellationToken)
    ***REMOVED***
        var dto = await GetWithAuth<IncomeStatementDto>(
            $"income-statement/***REMOVED***company.Symbol***REMOVED***",
            new Dictionary<string, string?>() ***REMOVED*** ***REMOVED*** "period", "annual" ***REMOVED*** ***REMOVED***,
            cancellationToken);

        return new IncomeStatement(
            DateTimeOffset.ParseExact(dto.Date, "yyyy-MM-dd", null),
            dto.EbitDa - dto.DepreciationAndAmortization);
***REMOVED***

    public void Dispose() => httpClient.Dispose();

    private Task<T> GetWithAuth<T>(string url, CancellationToken cancellationToken) => 
        GetWithAuth<T>(url, new Dictionary<string, string?>(), cancellationToken);

    private async Task<T> GetWithAuth<T>(
        string url, 
        IReadOnlyDictionary<string, string?> parameters, 
        CancellationToken cancellationToken)
    ***REMOVED***
        var parametersWithApiKey = parameters.Append(
            KeyValuePair.Create<string, string?>("apikey", fmpClientOptions.Value.ApiKey));

        return await httpClient.GetFromJsonAsync<T>(
            QueryHelpers.AddQueryString(url, parametersWithApiKey),
            cancellationToken) ?? throw new Exception("Json deserialised to null");
***REMOVED***

    private class IncomeStatementDto
    ***REMOVED***
        [Required]
        public required string Date ***REMOVED*** get; init; ***REMOVED***

        [Required]
        public required decimal DepreciationAndAmortization ***REMOVED*** get; init; ***REMOVED***

        [Required]
        public required decimal EbitDa ***REMOVED*** get; init; ***REMOVED***
***REMOVED***
***REMOVED***

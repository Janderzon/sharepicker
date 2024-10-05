using SharePicker.Models;

namespace SharePicker.Services;

public class FinancialStatementRepository(FmpClient fmpClient)
{
    public async Task<List<LabeledValue>> GetEbitPerRevenueSeries(Company company, CancellationToken cancellationToken)
    {
        var ratios = await fmpClient.GetRatiosAsync(company, cancellationToken);

        return ratios
            .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.EbitPerRevenue))
            .ToList();
    }

    public async Task<List<LabeledValue>> GetRoceSeries(Company company, CancellationToken cancellationToken)
    {
        var ratios = await fmpClient.GetRatiosAsync(company, cancellationToken);

        return ratios
            .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.ReturnOnCapitalEmployed))
            .ToList();
    }

    public async Task<List<LabeledValue>> GetCapitalTurnoverSeries(Company company, CancellationToken cancellationToken)
    {
        var ratios = await fmpClient.GetRatiosAsync(company, cancellationToken);

        return ratios
            .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.ReturnOnCapitalEmployed / x.EbitPerRevenue))
            .ToList();
    }

    public async Task<List<LabeledValue>> GetRevenueSeries(Company company, CancellationToken cancellationToken)
    {
        var ratios = await fmpClient.GetIncomeStatementsAsync(company, cancellationToken);

        return ratios
            .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.Revenue))
            .ToList();
    }

    public async Task<List<LabeledValue>> GetEbitSeries(Company company, CancellationToken cancellationToken)
    {
        var ratios = await fmpClient.GetIncomeStatementsAsync(company, cancellationToken);

        return ratios
            .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.Ebit))
            .ToList();
    }

    public async Task<HashSet<string>> GetExchanges(CancellationToken cancellationToken)
    {
        var companies = await GetCompaniesAsync(cancellationToken);

        return companies
            .Select(company => company.Exchange)
            .ToHashSet();
    }
}

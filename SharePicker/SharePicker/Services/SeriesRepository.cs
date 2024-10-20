using SharePicker.Models;

namespace SharePicker.Services;

public class SeriesRepository(CompanyRepository companyRepository)
{
    public async Task<List<LabeledValue>> GetRevenueSeriesAsync(Company company, CancellationToken cancellationToken)
    {
        var incomeStatements = await companyRepository.GetIncomeStatementsAsync(company, cancellationToken);

        return incomeStatements
            .Select(incomeStatement => new LabeledValue(incomeStatement.Date.ToString("yyyy"), incomeStatement.Revenue))
            .ToList();
    }

    public async Task<List<LabeledValue>> GetProfitBeforeIncomeAndTaxationSeriesAsync(Company company, CancellationToken cancellationToken)
    {
        var incomeStatements = await companyRepository.GetIncomeStatementsAsync(company, cancellationToken);

        return incomeStatements
            .Select(incomeStatement => new LabeledValue(incomeStatement.Date.ToString("yyyy"), incomeStatement.ProfitBeforeIncomeAndTaxation))
            .ToList();
    }

    public async Task<List<LabeledValue>> GetProfitBeforeIncomeAndTaxationPerRevenueSeriesAsync(Company company, CancellationToken cancellationToken)
    {
        var ratios = await companyRepository.GetRatiosAsync(company, cancellationToken);

        return ratios
            .Where(ratios => ratios.EbitPerRevenue != null)
            .Select(ratios => new LabeledValue(ratios.Date.ToString("yyyy"), ratios.EbitPerRevenue!.Value))
            .ToList();
    }

    public async Task<List<LabeledValue>> GetReturnOnCapitalEmployedSeriesAsync(Company company, CancellationToken cancellationToken)
    {
        var ratios = await companyRepository.GetRatiosAsync(company, cancellationToken);

        return ratios
            .Where(ratios => ratios.ReturnOnCapitalEmployed != null)
            .Select(ratios => new LabeledValue(ratios.Date.ToString("yyyy"), ratios.ReturnOnCapitalEmployed!.Value))
            .ToList();
    }

    public async Task<List<LabeledValue>> GetCapitalTurnoverSeriesAsync(Company company, CancellationToken cancellationToken)
    {
        var ratios = await companyRepository.GetRatiosAsync(company, cancellationToken);

        return ratios
            .Where(ratios => ratios.ReturnOnCapitalEmployed != null && ratios.EbitPerRevenue != null)
            .Where(ratios => ratios.EbitPerRevenue!.Value != 0)
            .Select(ratios => new LabeledValue(ratios.Date.ToString("yyyy"), ratios.ReturnOnCapitalEmployed!.Value / ratios.EbitPerRevenue!.Value))
            .ToList();
    }
}

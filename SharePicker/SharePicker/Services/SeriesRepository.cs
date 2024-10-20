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
}

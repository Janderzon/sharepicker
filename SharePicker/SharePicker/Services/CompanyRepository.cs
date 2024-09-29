using SharePicker.Models;

namespace SharePicker.Services;

public class CompanyRepository(FmpClient fmpClient) : BackgroundService
{
    private HashSet<Company> _companies;

    public List<Company> GetCompanies()
    {
        throw new NotImplementedException();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var tradableCompanies = await fmpClient.GetTradableCompaniesAsync(cancellationToken);
        var symbolsWithFinancialStatements = await fmpClient.GetSymbolsWithFinancialStatementsAsync(cancellationToken);

        return tradableCompanies
            .Where(company => symbolsWithFinancialStatements.Contains(company.Symbol))
            .ToHashSet();
    }
}

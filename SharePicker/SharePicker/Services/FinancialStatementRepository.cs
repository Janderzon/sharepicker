using SharePicker.Components;
using SharePicker.Models;

namespace SharePicker.Services;

public class FinancialStatementRepository(FmpClient fmpClient)
{
    public async Task<HashSet<Company>> GetCompaniesAsync(CancellationToken cancellationToken)
    {
        var tradableCompanies = await fmpClient.GetTradableCompaniesAsync(cancellationToken);

        var symbolsWithFinancialStatements = await fmpClient.GetSymbolsWithFinancialStatementsAsync(cancellationToken);

        return tradableCompanies
            .Where(company => symbolsWithFinancialStatements.Contains(company.Symbol))
            .ToHashSet();
    }

    public Task<List<IncomeStatement>> GetIncomeStatementsAsync(Company company, CancellationToken cancellationToken) =>
        fmpClient.GetIncomeStatementsAsync(company, cancellationToken);
}

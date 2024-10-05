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

        foreach (var company in tradableCompanies
            .Where(company => symbolsWithFinancialStatements.Contains(company.Symbol)))
        {
            var balanceSheetStatements = await fmpClient.GetBalanceSheetStatementsAsync(company.Symbol, cancellationToken);
            var cashFlowStatements = await fmpClient.GetCashFlowStatementsAsync(company.Symbol, cancellationToken);
            var incomeStatements = await fmpClient.GetIncomeStatementsAsync(company.Symbol, cancellationToken);
            var ratios = await fmpClient.GetRatiosAsync(company.Symbol, cancellationToken);
            _companies.Add(new Company(
                company.Symbol,
                company.Name, 
                company.ExchangeShortName,
                balanceSheetStatements.Select(statement => statement.ToDomain()).ToList(),
                cashFlowStatements.Select(statement => statement.ToDomain()).ToList(),
                incomeStatements.Select(statement => statement.ToDomain()).ToList(),
                ratios.Select(statement => statement.ToDomain()).ToList()));
        }
    }
}

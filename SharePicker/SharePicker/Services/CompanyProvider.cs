using SharePicker.Models;

namespace SharePicker.Services;

public class CompanyProvider(FmpClient fmpClient) : BackgroundService
{
    private List<Company> _companies = [];

    public List<Company> GetCompanies() => _companies;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromHours(1));
            var tradableCompanies = await fmpClient.GetTradableCompaniesAsync(cancellationToken);
            var symbolsWithFinancialStatements = await fmpClient.GetSymbolsWithFinancialStatementsAsync(cancellationToken);

            var companies = new List<Company>();
            var companyCount = 0;
            foreach (var company in tradableCompanies
                .Where(company => symbolsWithFinancialStatements.Contains(company.Symbol)))
            {
                if (company.ExchangeShortName == null || company.ExchangeShortName != "NYSE")
                    continue;

                var balanceSheetStatements = await fmpClient.GetBalanceSheetStatementsAsync(company.Symbol, cancellationToken);
                var cashFlowStatements = await fmpClient.GetCashFlowStatementsAsync(company.Symbol, cancellationToken);
                var incomeStatements = await fmpClient.GetIncomeStatementsAsync(company.Symbol, cancellationToken);
                var ratios = await fmpClient.GetRatiosAsync(company.Symbol, cancellationToken);
                companies.Add(new Company(
                    company.Symbol,
                    company.Name,
                    company.ExchangeShortName,
                    balanceSheetStatements.Select(statement => statement.ToDomain()).ToList(),
                    cashFlowStatements.Select(statement => statement.ToDomain()).ToList(),
                    incomeStatements.Select(statement => statement.ToDomain()).ToList(),
                    ratios.Select(statement => statement.ToDomain()).ToList()));

                companyCount++;
                if (companyCount >= 20)
                    break;
            }

            _companies = companies;

            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }
}

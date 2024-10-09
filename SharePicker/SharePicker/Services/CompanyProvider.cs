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
            using var timer = new PeriodicTimer(TimeSpan.FromDays(1));
            var stocks = await fmpClient.GetStocksAsync(cancellationToken);
            var tradableSymbols = (await fmpClient.GetTradableSymbolsAsync(cancellationToken))
                .Select(s => s.Symbol)
                .ToHashSet();
            var symbolsWithFinancialStatements = (await fmpClient.GetSymbolsWithFinancialStatementsAsync(cancellationToken)).ToHashSet();

            var companies = new List<Company>();
            foreach (var stock in stocks
                .Where(stock => tradableSymbols.Contains(stock.Symbol))
                .Where(stock => symbolsWithFinancialStatements.Contains(stock.Symbol)))
            {
                if (stock.ExchangeShortName == null || stock.ExchangeShortName != "LSE")
                    continue;

                await fmpClient.GetFullFinancialStatementsAsync(stock.Symbol, cancellationToken);

                var balanceSheetStatements = await fmpClient.GetBalanceSheetStatementsAsync(stock.Symbol, cancellationToken);
                var cashFlowStatements = await fmpClient.GetCashFlowStatementsAsync(stock.Symbol, cancellationToken);
                var incomeStatements = await fmpClient.GetIncomeStatementsAsync(stock.Symbol, cancellationToken);
                var ratios = await fmpClient.GetRatiosAsync(stock.Symbol, cancellationToken);
                companies.Add(new Company(
                    stock.Symbol,
                    stock.Name,
                    stock.ExchangeShortName,
                    balanceSheetStatements.Select(statement => statement.ToDomain()).ToList(),
                    cashFlowStatements.Select(statement => statement.ToDomain()).ToList(),
                    incomeStatements.Select(statement => statement.ToDomain()).ToList(),
                    ratios.Select(statement => statement.ToDomain()).ToList()));
            }

            _companies = companies;

            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }
}

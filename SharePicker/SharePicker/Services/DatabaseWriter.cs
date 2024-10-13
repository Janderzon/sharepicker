using Microsoft.EntityFrameworkCore;
using SharePicker.Models.Database;

namespace SharePicker.Services;

public class DatabaseWriter(
    FmpClient fmpClient, 
    IDbContextFactory<SharePickerDbContext> dbContextFactory) : BackgroundService
{
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

            foreach (var stock in stocks
                .Where(stock => tradableSymbols.Contains(stock.Symbol))
                .Where(stock => symbolsWithFinancialStatements.Contains(stock.Symbol)))
            {
                if (stock.ExchangeShortName == null || stock.ExchangeShortName != "LSE" || stock.Exchange == null)
                    continue;

                using var dbContext = dbContextFactory.CreateDbContext();

                if (dbContext.Companies.Any(company => company.Symbol == stock.Symbol))
                    continue;

                dbContext.Companies.Add(new Company
                {
                    Name = stock.Name,
                    Symbol = stock.Symbol,
                });

                var incomeStatements = await fmpClient.GetIncomeStatementsAsync(stock.Symbol, cancellationToken);
                var balanceSheetStatements = await fmpClient.GetBalanceSheetStatementsAsync(stock.Symbol, cancellationToken);
                var cashFlowStatements = await fmpClient.GetCashFlowStatementsAsync(stock.Symbol, cancellationToken);

                companies.Add(new Company(
                    stock.Symbol,
                    stock.Name,
                    stock.ExchangeShortName,
                    incomeStatements.Select(ToDomain).ToList(),
                    balanceSheetStatements.Select(ToDomain).ToList(),
                    cashFlowStatements.Select(ToDomain).ToList()));
            }

            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }
}

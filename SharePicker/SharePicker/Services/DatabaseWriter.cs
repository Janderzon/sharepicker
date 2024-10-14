using Microsoft.EntityFrameworkCore;
using SharePicker.Models.Database;
using SharePicker.Models.Fmp;

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

                if (await dbContext.Companies.AnyAsync(company => company.Symbol == stock.Symbol, cancellationToken))
                    continue;

                var exchange = await dbContext.Exchanges.SingleOrDefaultAsync(exchange => exchange.Symbol == stock.ExchangeShortName, cancellationToken)
                    ?? new Exchange { Name = stock.Exchange, Symbol = stock.ExchangeShortName };

                var company = new Company { Name = stock.Name, Symbol = stock.Symbol, Exchange = exchange };

                var incomeStatements = await fmpClient.GetIncomeStatementsAsync(stock.Symbol, cancellationToken);
                foreach (var incomeStatement in incomeStatements)
                {
                    await dbContext.IncomeStatemets.AddAsync(
                        await ToDatabaseObject(dbContext, incomeStatement, company, cancellationToken), 
                        cancellationToken);
                }

                var balanceSheetStatements = await fmpClient.GetBalanceSheetStatementsAsync(stock.Symbol, cancellationToken);
                var cashFlowStatements = await fmpClient.GetCashFlowStatementsAsync(stock.Symbol, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }

    private static async Task<IncomeStatement> ToDatabaseObject(
        SharePickerDbContext dbContext,
        IncomeStatementDto dto, 
        Company company,
        CancellationToken cancellationToken)
    {
        var currency = await dbContext.Currencies.SingleOrDefaultAsync(
            currency => currency.Symbol == dto.ReportedCurrency, 
            cancellationToken) ?? new Currency { Symbol = dto.ReportedCurrency };

        return new IncomeStatement
        {
            Company = company,
            Date = DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
            Currency = currency,
            Revenue = dto.Revenue,
            CostOfSales = dto.CostOfRevenue,
            GrossProfit = dto.GrossProfit,
            ResearchAndDevelopmentCosts = dto.ResearchAndDevelopmentExpenses,
            DistributionCosts = dto.SellingAndMarketingExpenses,
            AdministrativeCosts = dto.GeneralAndAdministrativeExpenses,
            OtherCosts = dto.OtherExpenses,
            OperatingProfit = dto.OperatingIncome,
            ProfitBeforeIncomeAndTaxation = dto.IncomeBeforeTax - dto.InterestIncome + dto.InterestExpense,
            FinanceIncome = dto.InterestIncome,
            FinanceExpense = dto.InterestExpense,
            ProfitBeforeTax = dto.IncomeBeforeTax,
            Taxation = dto.IncomeTaxExpense,
            ProfitAfterTax = dto.IncomeBeforeTax - dto.IncomeTaxExpense,
            NetProfit = dto.NetIncome,
            EarningsPerShare = dto.Eps,
            EarningsPerShareDiluted = dto.EpsDiluted
        };
    }
}

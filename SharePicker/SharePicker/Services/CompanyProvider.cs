using SharePicker.Models;
using SharePicker.Models.Fmp;

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
            stocks = stocks.Where(stock => stock.Symbol == "DOM.L").ToList();
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

                var incomeStatements = await fmpClient.GetIncomeStatementsAsync(stock.Symbol, cancellationToken);
                var balanceSheetStatements = await fmpClient.GetBalanceSheetStatementsAsync(stock.Symbol, cancellationToken);
                var cashFlowStatements = await fmpClient.GetCashFlowStatementsAsync(stock.Symbol, cancellationToken);

                companies.Add(new Company(
                    stock.Symbol,
                    stock.Name,
                    stock.ExchangeShortName,
                    incomeStatements.Select(ToDomain).ToList(),
                    balanceSheetStatements.Select(ToDomain).ToList(),
                    cashFlowStatements.Select(ToDomain).ToList()
                    //ratios.Select(statement => statement.ToDomain()).ToList()
                    ));
            }

            _companies = companies;

            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }

    private IncomeStatement ToDomain(IncomeStatementDto dto) => new(
        DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
        dto.Revenue,
        dto.CostOfRevenue,
        dto.GrossProfit,
        dto.ResearchAndDevelopmentExpenses,
        dto.GeneralAndAdministrativeExpenses,
        dto.SellingAndMarketingExpenses,
        dto.OtherExpenses,
        dto.InterestIncome,
        dto.InterestExpense
        //dto.OperatingIncomeLoss,
        //dto.income,
        //dto.beforetax,
        //dto.AfterTax,
        //dto.EarningsPerShareBasic,
        //dto.EarningsPerShareDiluted
        );

    private BalanceSheetStatement ToDomain(BalanceSheetStatementDto dto) => new(
        DateOnly.ParseExact(dto.Date, "yyyy-MM-dd")
        //new Assets(),
        //new Liabilities(),
        //new Equity(),
        //new BalanceSheetSummary()
        );

    private CashFlowStatement ToDomain(CashFlowStatementDto dto) => new(
        DateOnly.ParseExact(dto.Date, "yyyy-MM-dd")
        //new OperationsCashFlow(
        //    dto.OperatingIncomeLoss,
        //    dto.DepreciationDepletionAndAmortization,
        //    dto,
        //    dto,
        //    dto,
        //    dto,
        //    dto,
        //    dto,
        //    dto,
        //    ),
        //new InvestingCashFlow(),
        //new FinancingCashFlow(),
        //dto
        );
}

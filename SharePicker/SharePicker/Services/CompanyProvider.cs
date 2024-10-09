using SharePicker.Models;
using SharePicker.Models.Fmp;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                var fullFinancialStatements = await fmpClient.GetFullFinancialStatementsAsync(stock.Symbol, cancellationToken);

                var balanceSheetStatements = await fmpClient.GetBalanceSheetStatementsAsync(stock.Symbol, cancellationToken);
                var cashFlowStatements = await fmpClient.GetCashFlowStatementsAsync(stock.Symbol, cancellationToken);
                var incomeStatements = await fmpClient.GetIncomeStatementsAsync(stock.Symbol, cancellationToken);
                var ratios = await fmpClient.GetRatiosAsync(stock.Symbol, cancellationToken);
                companies.Add(new Company(
                    stock.Symbol,
                    stock.Name,
                    stock.ExchangeShortName,
                    balanceSheetStatements.Select(statement => statement.ToDomain()).ToList(),
                    fullFinancialStatements.Select(ExtractCashFlowStatement).ToList(),
                    fullFinancialStatements.Select(ExtractIncomStatement).ToList(),
                    ratios.Select(statement => statement.ToDomain()).ToList()));
            }

            _companies = companies;

            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }

    private IncomeStatement ExtractIncomStatement(FullFinancialStatementDto dto) => new(
        DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
        dto.RevenueFromContractWithCustomerExcludingAssessedTax,
        dto.GrossProfit,
        dto.OperatingIncomeLoss,
        dto.income,
        dto.beforetax,
        dto.AfterTax,
        dto.EarningsPerShareBasic,
        dto.EarningsPerShareDiluted);

    private BalanceSheetStatement ExtractBalanceSheetStatement(FullFinancialStatementDto dto) => new(
        DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
        new Assets(),
        new Liabilities(),
        new Equity(),
        new BalanceSheetSummary());

    private CashFlowStatement ExtractCashFlowStatement(FullFinancialStatementDto dto) => new(
        DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
        new OperationsCashFlow(
            dto.OperatingIncomeLoss,
            dto.DepreciationDepletionAndAmortization,
            dto,
            dto,
            dto,
            dto,
            dto,
            dto,
            dto,
            ),
        new InvestingCashFlow(),
        new FinancingCashFlow(),
        dto);
}

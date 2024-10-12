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
        dto.SellingAndMarketingExpenses,
        dto.GeneralAndAdministrativeExpenses,
        dto.OperatingIncome,
        dto.EbitDa - dto.DepreciationAndAmortization - dto.InterestIncome,
        dto.InterestIncome,
        dto.InterestExpense,
        dto.IncomeBeforeTax,
        dto.IncomeTaxExpense,
        dto.IncomeBeforeTax - dto.IncomeTaxExpense,
        dto.NetIncome,
        dto.Eps,
        dto.EpsDiluted);

    private BalanceSheetStatement ToDomain(BalanceSheetStatementDto dto) => new(
        DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
        new Assets(
            new CurrentAssets(
                dto.CashAndCashEquivalents,
                dto.ShortTermInvestments,
                dto.NetReceivables,
                dto.Inventory,
                dto.OtherCurrentAssets,
                dto.TotalCurrentAssets),
            new NonCurrentAssets(
                dto.PropertyPlantEquipmentNet,
                dto.Goodwill,
                dto.IntangibleAssets,
                dto.LongTermInvestments,
                dto.TaxAssets,
                dto.OtherNonCurrentAssets,
                dto.TotalNonCurrentAssets),
            dto.OtherAssets,
            dto.TotalAssets),
        new Liabilities(
            new CurrentLiabilities(
                dto.AccountPayables,
                dto.ShortTermDebt,
                dto.TaxPayables,
                dto.DeferredRevenue - dto.TaxPayables,
                dto.OtherCurrentLiabilities,
                dto.TotalCurrentLiabilities),
            new NonCurrentLiabilities(
                dto.LongTermDebt,
                dto.DeferredRevenueNonCurrent,
                dto.DeferredTaxLiabilitiesNonCurrent,
                dto.MinorityInterest,
                dto.CapitalLeaseObligations,
                dto.OtherNonCurrentLiabilities,
                dto.TotalNonCurrentLiabilities),
            dto.OtherLiabilities,
            dto.TotalLiabilities),
        new Equity(
            dto.PreferredStock,
            dto.CommonStock,
            dto.RetainedEarnings,
            dto.AccumulatedOtherComprehensiveIncomeLoss,
            dto.OtherTotalStockholdersEquity,
            dto.TotalStockholdersEquity,
            dto.TotalEquity),
        new BalanceSheetSummary(
            dto.TotalInvestments,
            dto.TotalDebt,
            dto.NetDebt));

    private CashFlowStatement ToDomain(CashFlowStatementDto dto) => new(
        DateOnly.ParseExact(dto.Date, "yyyy-MM-dd"),
        new OperationsCashFlow(
            //dto.NetIncome,
            dto.DepreciationAndAmortization,
            //dto.StockBasedCompensation,
            dto.Inventory,
            dto.AccountsReceivables,
            //dto.AccountsPayables,
            //dto.OtherWorkingCapital,
            dto.ChangeInWorkingCapital,
            //dto.OtherNonCashItems,
            //dto.OperatingCashFlow,
            //dto.DeferredIncomeTax,
            dto.NetCashProvidedByOperatingActivities),
        new InvestingCashFlow(
            dto.NetCashUsedForInvestingActivites),
        new FinancingCashFlow(
            dto.NetCashUsedProvidedByFinancingActivities),
        dto.NetChangeInCash);
}

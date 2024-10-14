using Microsoft.EntityFrameworkCore;
using SharePicker.Models;

namespace SharePicker.Services;

public class CompanyProvider(IDbContextFactory<SharePickerDbContext> dbContextFactory) : BackgroundService
{
    private List<Company> _companies = [];

    public List<Company> GetCompanies() => _companies;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromDays(1));

            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var companies = await dbContext.Companies.ToListAsync(cancellationToken);

                _companies = companies
                    .Select(dbo => new Company(
                        dbo.Symbol,
                        dbo.Name,
                        dbo.Exchange.Symbol,
                        dbo.IncomeStatements.Select(ToDomain).ToList(),
                        dbo.BalanceSheetStatements.Select(ToDomain).ToList(),
                        dbo.CashFlowStatements.Select(ToDomain).ToList()))
                    .ToList();
            }

            await timer.WaitForNextTickAsync(cancellationToken);
        }
    }

    private IncomeStatement ToDomain(Models.Database.IncomeStatement dbo) => new(
        dbo.Date,
        dbo.Currency.Symbol,
        dbo.Revenue,
        dbo.CostOfSales,
        dbo.GrossProfit,
        dbo.ResearchAndDevelopmentCosts,
        dbo.DistributionCosts,
        dbo.AdministrativeCosts,
        dbo.OtherCosts,
        dbo.OperatingProfit,
        dbo.ProfitBeforeIncomeAndTaxation,
        dbo.FinanceIncome,
        dbo.FinanceExpense,
        dbo.ProfitBeforeTax,
        dbo.Taxation,
        dbo.ProfitAfterTax,
        dbo.NetProfit,
        dbo.EarningsPerShare,
        dbo.EarningsPerShareDiluted);

    private BalanceSheetStatement ToDomain(Models.Database.BalanceSheetStatement dto) => new(
        dto.Date,
        dto.Currency.Symbol,
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
                dto.DeferredRevenue,
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

    private CashFlowStatement ToDomain(Models.Database.CashFlowStatement dto) => new(
        dto.Date,
        dto.Currency.Symbol,
        new OperationsCashFlow(
            dto.NetIncome,
            dto.DepreciationAndAmortisation,
            dto.DeferredIncomeTax,
            dto.StockBasedCompensation,
            dto.AccountsReceivables,
            dto.Inventory,
            dto.AccountsPayables,
            dto.OtherWorkingCapital,
            dto.ChangeInWorkingCapital,
            dto.OtherNonCashItems,
            dto.NetCashFromOperations),
        new InvestingCashFlow(
            dto.CapitalExpenditure,
            dto.Acquisitions,
            dto.PurchasesOfInvestments,
            dto.SaleOrMaturityOfInvestments,
            dto.OtherInvestingActivities,
            dto.NetCashFromInvesting),
        new FinancingCashFlow(
            dto.SharesIssued,
            dto.SharesRepurchased,
            dto.DebtRepayment,
            dto.DividendsPaid,
            dto.OtherFinancingActivities,
            dto.NetCashFromFinancing),
        dto.NetChangeInCash);
}

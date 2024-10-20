using Microsoft.EntityFrameworkCore;
using SharePicker.Models;

namespace SharePicker.Services;

public class CompanyRepository(IDbContextFactory<SharePickerDbContext> dbContextFactory)
{
    public async Task<List<Company>> GetAvailableCompaniesAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Companies
            .OrderBy(x => x.Symbol)
            .Select(dbo => ToDomain(dbo))
            .ToListAsync(cancellationToken);
    }

    public async Task<Exchange> GetExchangesAsync(Company company, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Companies
            .Where(dbo => dbo.Symbol == company.Symbol)
            .OrderBy(dbo => dbo.Exchange.Symbol)
            .Include(x => x.Exchange)
            .Select(dbo => ToDomain(dbo.Exchange))
            .SingleAsync(cancellationToken);
    }

    public async Task<List<IncomeStatement>> GetIncomeStatementsAsync(
        Company company,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.IncomeStatements
            .Where(dbo => dbo.Company.Symbol == company.Symbol)
            .OrderBy(dbo => dbo.Date)
            .Include(dbo => dbo.Currency)
            .Select(dbo => ToDomain(dbo))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BalanceSheetStatement>> GetBalanceSheetStatementsAsync(
        Company company,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.BalanceSheetStatements
            .Where(dbo => dbo.Company.Symbol == company.Symbol)
            .OrderBy(dbo => dbo.Date)
            .Include(dbo => dbo.Currency)
            .Select(dbo => ToDomain(dbo))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CashFlowStatement>> GetCashFlowStatementsAsync(
        Company company,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.CashFlowStatements
            .Where(dbo => dbo.Company.Symbol == company.Symbol)
            .OrderBy(dbo => dbo.Date)
            .Include(dbo => dbo.Currency)
            .Select(dbo => ToDomain(dbo))
            .ToListAsync(cancellationToken);
    }

    private static Company ToDomain(Models.Database.Company dbo) => new(dbo.Symbol, dbo.Name);

    private static Exchange ToDomain(Models.Database.Exchange dbo) => new(dbo.Symbol, dbo.Name);

    private static IncomeStatement ToDomain(Models.Database.IncomeStatement dbo) => new(
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

    private static BalanceSheetStatement ToDomain(Models.Database.BalanceSheetStatement dto) => new(
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

    private static CashFlowStatement ToDomain(Models.Database.CashFlowStatement dto) => new(
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

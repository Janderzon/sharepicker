using Microsoft.EntityFrameworkCore;
using SharePicker.Models.Database;

namespace SharePicker.Services;

public class SharePickerDbContext(DbContextOptions<SharePickerDbContext> options) : DbContext(options)
{
    public DbSet<BalanceSheetStatement> BalanceSheetStatements { get; set; }

    public DbSet<CashFlowStatement> CashFlowStatements { get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<Currency> Currencies { get; set; }

    public DbSet<Exchange> Exchanges { get; set; }

    public DbSet<IncomeStatement> IncomeStatements { get; set; }

    public DbSet<Ratios> Ratios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BalanceSheetStatement>(entity =>
        {
            entity.HasKey(e => e.BalanceSheetStatementId);

            entity.Property(e => e.AccountPayables).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.AccumulatedOtherComprehensiveIncomeLoss).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CapitalLeaseObligations).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CashAndCashEquivalents).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CommonStock).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DeferredRevenue).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DeferredRevenueNonCurrent).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DeferredTaxLiabilitiesNonCurrent).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Goodwill).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.IntangibleAssets).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Inventory).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.LongTermDebt).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.LongTermInvestments).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.MinorityInterest).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetDebt).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetReceivables).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherAssets).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherCurrentAssets).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherCurrentLiabilities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherLiabilities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherNonCurrentAssets).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherNonCurrentLiabilities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherTotalStockholdersEquity).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.PreferredStock).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.PropertyPlantEquipmentNet).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.RetainedEarnings).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.ShortTermDebt).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.ShortTermInvestments).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TaxAssets).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TaxPayables).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalAssets).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalCurrentAssets).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalCurrentLiabilities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalDebt).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalEquity).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalInvestments).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalLiabilities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalNonCurrentAssets).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalNonCurrentLiabilities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.TotalStockholdersEquity).HasColumnType("decimal(19, 4)");

            entity
                .HasOne(d => d.Company)
                .WithMany(p => p.BalanceSheetStatements)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BalanceSheetStatements_Companies");

            entity
                .HasOne(d => d.Currency)
                .WithMany(p => p.BalanceSheetStatements)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BalanceSheetStatements_Currencies");
        });

        modelBuilder.Entity<CashFlowStatement>(entity =>
        {
            entity.HasKey(e => e.CashFlowStatementId);

            entity.Property(e => e.AccountsPayables).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.AccountsReceivables).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Acquisitions).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CapitalExpenditure).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.ChangeInWorkingCapital).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DebtRepayment).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DeferredIncomeTax).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DepreciationAndAmortisation).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DividendsPaid).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Inventory).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetCashFromOperations).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetCashFromFinancing).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetCashFromInvesting).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetChangeInCash).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetIncome).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherFinancingActivities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherInvestingActivities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherNonCashItems).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherWorkingCapital).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.PurchasesOfInvestments).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.SaleOrMaturityOfInvestments).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.SharesIssued).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.SharesRepurchased).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.StockBasedCompensation).HasColumnType("decimal(19, 4)");

            entity
                .HasOne(d => d.Company)
                .WithMany(p => p.CashFlowStatements)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CashFlowStatements_Companies");

            entity
                .HasOne(d => d.Currency)
                .WithMany(p => p.CashFlowStatements)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CashFlowStatements_Currencies");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId);

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Symbol).HasMaxLength(20);

            entity
                .HasOne(d => d.Exchange)
                .WithMany(p => p.Companies)
                .HasForeignKey(d => d.ExchangeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Companies_Exchanges");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId);

            entity.Property(e => e.Symbol).HasMaxLength(20);
        });

        modelBuilder.Entity<Exchange>(entity =>
        {
            entity.HasKey(e => e.ExchangeId);

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Symbol).HasMaxLength(20);
        });

        modelBuilder.Entity<IncomeStatement>(entity =>
        {
            entity.HasKey(e => e.IncomeStatementId);

            entity.Property(e => e.AdministrativeCosts).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CostOfSales).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DistributionCosts).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.EarningsPerShare).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.EarningsPerShareDiluted).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.FinanceExpense).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.FinanceIncome).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.GrossProfit).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetProfit).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OperatingProfit).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherCosts).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.ProfitAfterTax).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.ProfitBeforeIncomeAndTaxation).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.ProfitBeforeTax).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.ResearchAndDevelopmentCosts).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Revenue).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Taxation).HasColumnType("decimal(19, 4)");

            entity
                .HasOne(d => d.Company)
                .WithMany(p => p.IncomeStatements)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IncomeStatements_Companies");

            entity
                .HasOne(d => d.Currency)
                .WithMany(p => p.IncomeStatements)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IncomeStatements_Currencies");
        });

        modelBuilder.Entity<Ratios>(entity =>
        {
            entity.HasKey(e => e.RatiosId);

            entity.Property(e => e.NetIncome).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DepreciationAndAmortization).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DeferredIncomeTax).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.StockBasedCompensation).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.ChangeInWorkingCapital).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.AccountsReceivables).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Inventory).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.AccountsPayables).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherWorkingCapital).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherNonCashItems).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetCashProvidedByOperatingActivities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.InvestmentsInPropertyPlantAndEquipment).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.AcquisitionsNet).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.PurchasesOfInvestments).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.SalesMaturitiesOfInvestments).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherInvestingActivites).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetCashUsedForInvestingActivites).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DebtRepayment).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CommonStockIssued).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CommonStockRepurchased).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DividendsPaid).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherFinancingActivites).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetCashUsedProvidedByFinancingActivities).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.EffectOfForexChangesOnCash).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetChangeInCash).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CashAtEndOfPeriod).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CashAtBeginningOfPeriod).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OperatingCashFlow).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CapitalExpenditure).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.FreeCashFlow).HasColumnType("decimal(19, 4)");

            entity
                .HasOne(d => d.Company)
                .WithMany(p => p.Ratios)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ratios_Companies");
        });
    }
}

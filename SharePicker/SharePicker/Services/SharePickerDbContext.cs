using Microsoft.EntityFrameworkCore;
using SharePicker.Models.Database;
using SharePicker.Models.Options;

namespace SharePicker.Services;

public class SharePickerDbContext(
    DatabaseOptions databaseOptions,
    DbContextOptions<SharePickerDbContext> options) : DbContext(options)
{
    public DbSet<BalanceSheetStatement> BalanceSheetStatements { get; set; }

    public DbSet<CashFlowStatement> CashFlowStatements { get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<Currency> Currencies { get; set; }

    public DbSet<Exchange> Exchanges { get; set; }

    public DbSet<IncomeStatemet> IncomeStatemets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .UseSqlServer(databaseOptions.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BalanceSheetStatement>(entity =>
        {
            entity.HasKey(e => e.BalanceSheetStatementId).HasName("PK__BalanceS__6C9B057D66D1BBAC");

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
            entity.HasKey(e => e.CashFlowStatementId).HasName("PK__CashFlow__DE52E984BB10B14D");

            entity.Property(e => e.AccountsPayables).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.AccountsReceivables).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Acquisitions).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CapitalExpenditure).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.ChangeInWorkingCapital).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DebtRepayment).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DeferredIncomeTax).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DepreciationAndAmortization).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DividendsPaid).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Inventory).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetCashFlowFromOperations).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetCashFromFinancing).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetCashFromInvesting).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetChangeInCash).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetIncome).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherFinancingActivites).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.OtherInvestingActivites).HasColumnType("decimal(19, 4)");
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
            entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971CACA675BBC1");

            entity.Property(e => e.Name).HasMaxLength(50);
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
            entity.HasKey(e => e.CurrencyId).HasName("PK__Currenci__14470AF086E05275");

            entity.Property(e => e.Symbol).HasMaxLength(20);
        });

        modelBuilder.Entity<Exchange>(entity =>
        {
            entity.HasKey(e => e.ExchangeId).HasName("PK__Exchange__72E6008BABE01433");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Symbol).HasMaxLength(20);
        });

        modelBuilder.Entity<IncomeStatemet>(entity =>
        {
            entity.HasKey(e => e.IncomeStatementId).HasName("PK__IncomeSt__811D9AF8F6B3484D");

            entity.Property(e => e.AdministrativeCosts).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.CostOfSales).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.DistributionCosts).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.EarningsPerShare).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.EarningsPerShareDiluted).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.FinanceExpense).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.FinanceIncome).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.GrossProfit).HasColumnType("decimal(19, 4)");
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
                .WithMany(p => p.IncomeStatemets)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IncomeStatements_Companies");

            entity
                .HasOne(d => d.Currency)
                .WithMany(p => p.IncomeStatemets)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IncomeStatements_Currencies");
        });
    }
}

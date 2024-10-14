namespace SharePicker.Models.Database;

public class BalanceSheetStatement
{
    public int BalanceSheetStatementId { get; set; }

    public int CompanyId { get; set; }

    public int CurrencyId { get; set; }

    public required DateOnly Date { get; set; }

    public required decimal CashAndCashEquivalents { get; set; }

    public required decimal ShortTermInvestments { get; set; }

    public required decimal NetReceivables { get; set; }

    public required decimal Inventory { get; set; }

    public required decimal OtherCurrentAssets { get; set; }

    public required decimal TotalCurrentAssets { get; set; }

    public required decimal PropertyPlantEquipmentNet { get; set; }

    public required decimal Goodwill { get; set; }

    public required decimal IntangibleAssets { get; set; }

    public required decimal LongTermInvestments { get; set; }

    public required decimal TaxAssets { get; set; }

    public required decimal OtherNonCurrentAssets { get; set; }

    public required decimal TotalNonCurrentAssets { get; set; }

    public required decimal OtherAssets { get; set; }

    public required decimal TotalAssets { get; set; }

    public required decimal AccountPayables { get; set; }

    public required decimal ShortTermDebt { get; set; }

    public required decimal TaxPayables { get; set; }

    public required decimal DeferredRevenue { get; set; }

    public required decimal OtherCurrentLiabilities { get; set; }

    public required decimal TotalCurrentLiabilities { get; set; }

    public required decimal LongTermDebt { get; set; }

    public required decimal DeferredRevenueNonCurrent { get; set; }

    public required decimal DeferredTaxLiabilitiesNonCurrent { get; set; }

    public required decimal MinorityInterest { get; set; }

    public required decimal CapitalLeaseObligations { get; set; }

    public required decimal OtherNonCurrentLiabilities { get; set; }

    public required decimal TotalNonCurrentLiabilities { get; set; }

    public required decimal OtherLiabilities { get; set; }

    public required decimal TotalLiabilities { get; set; }

    public required decimal PreferredStock { get; set; }

    public required decimal CommonStock { get; set; }

    public required decimal RetainedEarnings { get; set; }

    public required decimal AccumulatedOtherComprehensiveIncomeLoss { get; set; }

    public required decimal OtherTotalStockholdersEquity { get; set; }

    public required decimal TotalStockholdersEquity { get; set; }

    public required decimal TotalEquity { get; set; }

    public required decimal TotalInvestments { get; set; }

    public required decimal TotalDebt { get; set; }

    public required decimal NetDebt { get; set; }

    public required Company Company { get; set; }

    public required Currency Currency { get; set; }
}

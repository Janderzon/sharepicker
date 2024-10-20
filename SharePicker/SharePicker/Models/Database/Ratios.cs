namespace SharePicker.Models.Database;

public class Ratios
{
    public int RatiosId { get; set; }
    public int CompanyId { get; set; }
    public required string Date { get; set; }
    public required string ReportedCurrency { get; set; }
    public required decimal NetIncome { get; set; }
    public required decimal DepreciationAndAmortization { get; set; }
    public required decimal DeferredIncomeTax { get; set; }
    public required decimal StockBasedCompensation { get; set; }
    public required decimal ChangeInWorkingCapital { get; set; }
    public required decimal AccountsReceivables { get; set; }
    public required decimal Inventory { get; set; }
    public required decimal AccountsPayables { get; set; }
    public required decimal OtherWorkingCapital { get; set; }
    public required decimal OtherNonCashItems { get; set; }
    public required decimal NetCashProvidedByOperatingActivities { get; set; }
    public required decimal InvestmentsInPropertyPlantAndEquipment { get; set; }
    public required decimal AcquisitionsNet { get; set; }
    public required decimal PurchasesOfInvestments { get; set; }
    public required decimal SalesMaturitiesOfInvestments { get; set; }
    public required decimal OtherInvestingActivites { get; set; }
    public required decimal NetCashUsedForInvestingActivites { get; set; }
    public required decimal DebtRepayment { get; set; }
    public required decimal CommonStockIssued { get; set; }
    public required decimal CommonStockRepurchased { get; set; }
    public required decimal DividendsPaid { get; set; }
    public required decimal OtherFinancingActivites { get; set; }
    public required decimal NetCashUsedProvidedByFinancingActivities { get; set; }
    public required decimal EffectOfForexChangesOnCash { get; set; }
    public required decimal NetChangeInCash { get; set; }
    public required decimal CashAtEndOfPeriod { get; set; }
    public required decimal CashAtBeginningOfPeriod { get; set; }
    public required decimal OperatingCashFlow { get; set; }
    public required decimal CapitalExpenditure { get; set; }
    public required decimal FreeCashFlow { get; set; }
    public required Company Company { get; set; }
}

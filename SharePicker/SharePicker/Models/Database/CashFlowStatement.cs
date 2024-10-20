namespace SharePicker.Models.Database;

public class CashFlowStatement
{
    public int CashFlowStatementId { get; set; }
    public int CompanyId { get; set; }
    public required DateOnly Date { get; set; }
    public int CurrencyId { get; set; }
    public required decimal NetIncome { get; set; }
    public required decimal DepreciationAndAmortisation { get; set; }
    public required decimal DeferredIncomeTax { get; set; }
    public required decimal StockBasedCompensation { get; set; }
    public required decimal AccountsReceivables { get; set; }
    public required decimal Inventory { get; set; }
    public required decimal AccountsPayables { get; set; }
    public required decimal OtherWorkingCapital { get; set; }
    public required decimal ChangeInWorkingCapital { get; set; }
    public required decimal OtherNonCashItems { get; set; }
    public required decimal NetCashFromOperations { get; set; }
    public required decimal CapitalExpenditure { get; set; }
    public required decimal Acquisitions { get; set; }
    public required decimal PurchasesOfInvestments { get; set; }
    public required decimal SaleOrMaturityOfInvestments { get; set; }
    public required decimal OtherInvestingActivities { get; set; }
    public required decimal NetCashFromInvesting { get; set; }
    public required decimal SharesIssued { get; set; }
    public required decimal SharesRepurchased { get; set; }
    public required decimal DebtRepayment { get; set; }
    public required decimal DividendsPaid { get; set; }
    public required decimal OtherFinancingActivities { get; set; }
    public required decimal NetCashFromFinancing { get; set; }
    public required decimal NetChangeInCash { get; set; }
    public required Company Company { get; set; }
    public required Currency Currency { get; set; }
}

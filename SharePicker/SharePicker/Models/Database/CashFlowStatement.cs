namespace SharePicker.Models.Database;

public class CashFlowStatement
{
    public required int CashFlowStatementId { get; set; }

    public required int CompanyId { get; set; }

    public required DateOnly Date { get; set; }

    public required int CurrencyId { get; set; }

    public required decimal NetIncome { get; set; }

    public required decimal DepreciationAndAmortization { get; set; }

    public required decimal DeferredIncomeTax { get; set; }

    public required decimal StockBasedCompensation { get; set; }

    public required decimal AccountsReceivables { get; set; }

    public required decimal Inventory { get; set; }

    public required decimal AccountsPayables { get; set; }

    public required decimal OtherWorkingCapital { get; set; }

    public required decimal ChangeInWorkingCapital { get; set; }

    public required decimal OtherNonCashItems { get; set; }

    public required decimal NetCashFlowFromOperations { get; set; }

    public required decimal CapitalExpenditure { get; set; }

    public required decimal Acquisitions { get; set; }

    public required decimal PurchasesOfInvestments { get; set; }

    public required decimal SaleOrMaturityOfInvestments { get; set; }

    public required decimal OtherInvestingActivites { get; set; }

    public required decimal NetCashFromInvesting { get; set; }

    public required decimal SharesIssued { get; set; }

    public required decimal SharesRepurchased { get; set; }

    public required decimal DebtRepayment { get; set; }

    public required decimal DividendsPaid { get; set; }

    public required decimal OtherFinancingActivites { get; set; }

    public required decimal NetCashFromFinancing { get; set; }

    public required decimal NetChangeInCash { get; set; }

    public required Company Company { get; set; }

    public required Currency Currency { get; set; }
}

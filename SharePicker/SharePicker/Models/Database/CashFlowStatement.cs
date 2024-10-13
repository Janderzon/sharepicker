namespace SharePicker.Models.Database;

public class CashFlowStatement
{
    public int CashFlowStatementId { get; set; }

    public int CompanyId { get; set; }

    public DateOnly Date { get; set; }

    public int CurrencyId { get; set; }

    public decimal NetIncome { get; set; }

    public decimal DepreciationAndAmortization { get; set; }

    public decimal DeferredIncomeTax { get; set; }

    public decimal StockBasedCompensation { get; set; }

    public decimal AccountsReceivables { get; set; }

    public decimal Inventory { get; set; }

    public decimal AccountsPayables { get; set; }

    public decimal OtherWorkingCapital { get; set; }

    public decimal ChangeInWorkingCapital { get; set; }

    public decimal OtherNonCashItems { get; set; }

    public decimal NetCashFlowFromOperations { get; set; }

    public decimal CapitalExpenditure { get; set; }

    public decimal Acquisitions { get; set; }

    public decimal PurchasesOfInvestments { get; set; }

    public decimal SaleOrMaturityOfInvestments { get; set; }

    public decimal OtherInvestingActivites { get; set; }

    public decimal NetCashFromInvesting { get; set; }

    public decimal SharesIssued { get; set; }

    public decimal SharesRepurchased { get; set; }

    public decimal DebtRepayment { get; set; }

    public decimal DividendsPaid { get; set; }

    public decimal OtherFinancingActivites { get; set; }

    public decimal NetCashFromFinancing { get; set; }

    public decimal NetChangeInCash { get; set; }

    public Company Company { get; set; } = null!;

    public Currency Currency { get; set; } = null!;
}

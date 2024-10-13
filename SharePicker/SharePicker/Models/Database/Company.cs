namespace SharePicker.Models.Database;

public class Company
{
    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public int ExchangeId { get; set; }

    public ICollection<BalanceSheetStatement> BalanceSheetStatements { get; set; } = [];

    public ICollection<CashFlowStatement> CashFlowStatements { get; set; } = [];

    public Exchange Exchange { get; set; } = null!;

    public ICollection<IncomeStatemet> IncomeStatemets { get; set; } = [];
}

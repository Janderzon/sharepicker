namespace SharePicker.Models.Database;

public class Currency
{
    public int CurrencyId { get; set; }

    public string Symbol { get; set; } = null!;

    public ICollection<BalanceSheetStatement> BalanceSheetStatements { get; set; } = [];

    public ICollection<CashFlowStatement> CashFlowStatements { get; set; } = [];

    public ICollection<IncomeStatemet> IncomeStatemets { get; set; } = [];
}

namespace SharePicker.Models.Database;

public class Currency
{
    public int CurrencyId { get; set; }

    public required string Symbol { get; set; }

    public ICollection<BalanceSheetStatement> BalanceSheetStatements { get; set; } = [];

    public ICollection<CashFlowStatement> CashFlowStatements { get; set; } = [];

    public ICollection<IncomeStatement> IncomeStatemets { get; set; } = [];
}

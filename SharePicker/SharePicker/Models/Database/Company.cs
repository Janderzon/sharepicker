namespace SharePicker.Models.Database;

public class Company
{
    public int CompanyId { get; set; }

    public required string Name { get; set; }

    public required string Symbol { get; set; }

    public int ExchangeId { get; set; }

    public ICollection<BalanceSheetStatement> BalanceSheetStatements { get; set; } = [];

    public ICollection<CashFlowStatement> CashFlowStatements { get; set; } = [];

    public required Exchange Exchange { get; set; }

    public ICollection<IncomeStatement> IncomeStatemets { get; set; } = [];
}

namespace SharePicker.Models.Fmp;

public record IncomeStatementDto
{
    public required string Date { get; init; }
    public required string ReportedCurrency { get; init; }
    public required decimal Revenue { get; init; }
    public required decimal GrossProfit { get; init; }
    public required decimal DepreciationAndAmortization { get; init; }
    public required decimal EbitDa { get; init; }
    public required decimal OperatingIncome { get; init; }
    public required decimal IncomeBeforeTax { get; init; }
    public required decimal IncomeTaxExpense { get; init; }
    public required decimal Eps { get; init; }
    public required decimal EpsDiluted { get; init; }
}

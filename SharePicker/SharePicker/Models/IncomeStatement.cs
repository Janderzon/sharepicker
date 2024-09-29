namespace SharePicker.Models;

public record IncomeStatement(
    DateTimeOffset DateTimeOffset, 
    decimal Ebit,
    decimal Revenue,
    decimal GrossProfit,
    decimal OperatingProfits,
    decimal ProfitBeforeTax,
    decimal ProfitAfterTax,
    decimal EarningsPerShare,
    decimal DilutedEarningsPerShare) : Statement;

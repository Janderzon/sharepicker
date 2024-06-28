namespace SharePicker.Models;

public record IncomeStatement(
    DateTimeOffset DateTimeOffset, 
    decimal Ebit,
    decimal Revenue);

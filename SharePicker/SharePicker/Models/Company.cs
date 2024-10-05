namespace SharePicker.Models;

public record Company(
    string Symbol,
    string Name,
    string? Exchange,
    List<BalanceSheetStatement> BalanceSheetStatements,
    List<CashFlowStatement> CashFlowStatements,
    List<IncomeStatement> IncomeStatements,
    List<Ratios> ratios);

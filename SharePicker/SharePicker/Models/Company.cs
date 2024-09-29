namespace SharePicker.Models;

public record Company(
    string Symbol,
    string Name,
    Exchange Exchange,
    YearlyStatements<BalanceSheetStatement> BalanceSheetStatements,
    YearlyStatements<CashFlowStatement> CashFlowStatements,
    YearlyStatements<IncomeStatement> IncomeStatements);

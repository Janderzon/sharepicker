namespace SharePicker.Models;

public record BalanceSheetStatement(
    DateTimeOffset DateTimeOffset,
    decimal TotalAssets,
    decimal CurrentLiabilities,
    decimal ShortTermDebt);

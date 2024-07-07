namespace SharePicker.Models;

public record BalanceSheetStatement(
    DateTimeOffset DateTimeOffset,
    Assets Assets,
    Liabilities Liabilities,
    Equity Equity);

public record Assets(
    CurrentAssets CurrentAssets,
    NonCurrentAssets NonCurrentAssets,
    decimal OtherAssets,
    decimal TotalAssets);

public record CurrentAssets(
    decimal CashAndCashEquivalents,
    decimal ShortTermInvestments,
    decimal CashAndShortTermInvestments,
    decimal NetReceivables,
    decimal Inventory,
    decimal OtherCurrentAssets,
    decimal TotalCurrentAssets);

public record NonCurrentAssets(
    decimal PropertyPlantEquipmentNet,
    decimal Goodwill,
    decimal IntangibleAssets,
    decimal GoodwillAndIntangibleAssets,
    decimal LongTermInvestments,
    decimal TaxAssets,
    decimal OtherNonCurrentAssets,
    decimal TotalNonCurrentAssets);

public record Liabilities(
    CurrentLiabilities CurrentLiabilities,
    NonCurrentLiabilities NonCurrentLiabilities,
    decimal OtherLiabilities,
    decimal CapitalLeaseObligations,
    decimal TotalLiabilities);

public record CurrentLiabilities(
    decimal AccountPayables,
    decimal ShortTermDebt,
    decimal TaxPayables,
    decimal DeferredRevenue,
    decimal OtherCurrentLiabilities,
    decimal TotalCurrentLiabilities);

public record NonCurrentLiabilities(
    decimal LongTermDebt,
    decimal DeferredRevenueNonCurrent,
    decimal DeferredTaxLiabilitiesNonCurrent,
    decimal OtherNonCurrentLiabilities,
    decimal TotalNonCurrentLiabilities);

public record Equity(
    decimal PreferredStock,
    decimal CommonStock,
    decimal ReatainedEarnings,
    decimal AccumulatedOtherComprehensiveIncomeLoss,
    decimal OtherTotalStockholdersEquity,
    decimal TotalStockholdersEquitey,
    decimal TotalEquity);

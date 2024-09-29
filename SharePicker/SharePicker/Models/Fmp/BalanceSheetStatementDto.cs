namespace SharePicker.Models.Fmp;

public record BalanceSheetStatementDto
{
    public required string Date { get; init; }
    public required decimal CashAndCashEquivalents { get; init; }
    public required decimal ShortTermInvestments { get; init; }
    public required decimal CashAndShortTermInvestments { get; init; }
    public required decimal NetReceivables { get; init; }
    public required decimal Inventory { get; init; }
    public required decimal OtherCurrentAssets { get; init; }
    public required decimal TotalCurrentAssets { get; init; }
    public required decimal PropertyPlantEquipmentNet { get; init; }
    public required decimal Goodwill { get; init; }
    public required decimal IntangibleAssets { get; init; }
    public required decimal GoodwillAndIntangibleAssets { get; init; }
    public required decimal LongTermInvestments { get; init; }
    public required decimal TaxAssets { get; init; }
    public required decimal OtherNonCurrentAssets { get; init; }
    public required decimal TotalNonCurrentAssets { get; init; }
    public required decimal OtherAssets { get; init; }
    public required decimal TotalAssets { get; init; }
    public required decimal AccountPayables { get; init; }
    public required decimal ShortTermDebt { get; init; }
    public required decimal TaxPayables { get; init; }
    public required decimal DeferredRevenue { get; init; }
    public required decimal OtherCurrentLiabilities { get; init; }
    public required decimal TotalCurrentLiabilities { get; init; }
    public required decimal LongTermDebt { get; init; }
    public required decimal DeferredRevenueNonCurrent { get; init; }
    public required decimal DeferredTaxLiabilitiesNonCurrent { get; init; }
    public required decimal OtherNonCurrentLiabilities { get; init; }
    public required decimal TotalNonCurrentLiabilities { get; init; }
    public required decimal OtherLiabilities { get; init; }
    public required decimal CapitalLeaseObligations { get; init; }
    public required decimal TotalLiabilities { get; init; }
    public required decimal PreferredStock { get; init; }
    public required decimal CommonStock { get; init; }
    public required decimal RetainedEarnings { get; init; }
    public required decimal AccumulatedOtherComprehensiveIncomeLoss { get; init; }
    public required decimal OtherTotalStockholdersEquity { get; init; }
    public required decimal TotalStockholdersEquity { get; init; }
    public required decimal TotalEquity { get; init; }
    public required decimal TotalLiabilitiesAndStockholdersEquity { get; init; }
    public required decimal MinorityInterest { get; init; }
    public required decimal TotalLiabilitiesAndTotalEquity { get; init; }
    public required decimal TotalInvestments { get; init; }
    public required decimal TotalDebt { get; init; }
    public required decimal NetDebt { get; init; }

    public BalanceSheetStatement ToDomain() => new(
        DateTimeOffset.ParseExact(Date, "yyyy-MM-dd", null),
        new Assets(
            new CurrentAssets(
                CashAndCashEquivalents,
                ShortTermInvestments,
                CashAndShortTermInvestments,
                NetReceivables,
                Inventory,
                OtherCurrentAssets,
                TotalCurrentAssets),
            new NonCurrentAssets(
                PropertyPlantEquipmentNet,
                Goodwill,
                IntangibleAssets,
                GoodwillAndIntangibleAssets,
                LongTermInvestments,
                TaxAssets,
                OtherNonCurrentAssets,
                TotalNonCurrentAssets),
            OtherAssets,
            TotalAssets),
        new Liabilities(
            new CurrentLiabilities(
                AccountPayables,
                ShortTermDebt,
                TaxPayables,
                DeferredRevenue,
                OtherCurrentLiabilities,
                TotalCurrentLiabilities),
            new NonCurrentLiabilities(
                LongTermDebt,
                DeferredRevenueNonCurrent,
                DeferredTaxLiabilitiesNonCurrent,
                OtherNonCurrentLiabilities,
                TotalNonCurrentLiabilities),
            OtherLiabilities,
            CapitalLeaseObligations,
            TotalLiabilities),
        new Equity(
            PreferredStock,
            CommonStock,
            RetainedEarnings,
            AccumulatedOtherComprehensiveIncomeLoss,
            OtherTotalStockholdersEquity,
            TotalStockholdersEquity,
            TotalEquity),
        new BalanceSheetSummary(
            TotalLiabilitiesAndStockholdersEquity,
            MinorityInterest,
            TotalLiabilitiesAndTotalEquity,
            TotalInvestments,
            TotalDebt,
            NetDebt));
}

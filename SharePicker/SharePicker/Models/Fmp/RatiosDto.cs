﻿namespace SharePicker.Models.Fmp;

public record RatiosDto
{
    public required string Date { get; init; }
    public required decimal CurrentRatio { get; init; }
    public required decimal QuickRatio { get; init; }
    public required decimal CashRatio { get; init; }
    public required decimal DaysOfSalesOutstanding { get; init; }
    public required decimal DaysOfInventoryOutstanding { get; init; }
    public required decimal OperatingCycle { get; init; }
    public required decimal DaysOfPayablesOutstanding { get; init; }
    public required decimal CashConversionCycle { get; init; }
    public required decimal GrossProfitMargin { get; init; }
    public required decimal OperatingProfitMargin { get; init; }
    public required decimal PretaxProfitMargin { get; init; }
    public required decimal NetProfitMargin { get; init; }
    public required decimal EffectiveTaxRate { get; init; }
    public required decimal ReturnOnAssets { get; init; }
    public required decimal ReturnOnEquity { get; init; }
    public required decimal ReturnOnCapitalEmployed { get; init; }
    public required decimal NetIncomePerEBT { get; init; }
    public required decimal EbtPerEbit { get; init; }
    public required decimal EbitPerRevenue { get; init; }
    public required decimal DebtRatio { get; init; }
    public required decimal DebtEquityRatio { get; init; }
    public required decimal LongTermDebtToCapitalization { get; init; }
    public required decimal TotalDebtToCapitalization { get; init; }
    public required decimal InterestCoverage { get; init; }
    public required decimal CashFlowToDebtRatio { get; init; }
    public required decimal CompanyEquityMultiplier { get; init; }
    public required decimal ReceivablesTurnover { get; init; }
    public required decimal PayablesTurnover { get; init; }
    public required decimal InventoryTurnover { get; init; }
    public required decimal FixedAssetTurnover { get; init; }
    public required decimal AssetTurnover { get; init; }
    public required decimal OperatingCashFlowPerShare { get; init; }
    public required decimal FreeCashFlowPerShare { get; init; }
    public required decimal CashPerShare { get; init; }
    public required decimal PayoutRatio { get; init; }
    public required decimal OperatingCashFlowSalesRatio { get; init; }
    public required decimal FreeCashFlowOperatingCashFlowRatio { get; init; }
    public required decimal CashFlowCoverageRatios { get; init; }
    public required decimal ShortTermCoverageRatios { get; init; }
    public required decimal CapitalExpenditureCoverageRatio { get; init; }
    public required decimal DividendPaidAndCapexCoverageRatio { get; init; }
    public required decimal DividendPayoutRatio { get; init; }
    public required decimal PriceBookValueRatio { get; init; }
    public required decimal PriceToBookRatio { get; init; }
    public required decimal PriceToSalesRatio { get; init; }
    public required decimal PriceEarningsRatio { get; init; }
    public required decimal PriceToFreeCashFlowsRatio { get; init; }
    public required decimal PriceToOperatingCashFlowsRatio { get; init; }
    public required decimal PriceCashFlowRatio { get; init; }
    public required decimal PriceEarningsToGrowthRatio { get; init; }
    public required decimal PriceSalesRatio { get; init; }
    public required decimal DividendYield { get; init; }
    public required decimal EnterpriseValueMultiple { get; init; }
    public required decimal PriceFairValue { get; init; }

    public Ratios ToDomain() => new(
        DateTimeOffset.ParseExact(Date, "yyyy-MM-dd", null),
        ReturnOnCapitalEmployed,
        EbitPerRevenue);
}
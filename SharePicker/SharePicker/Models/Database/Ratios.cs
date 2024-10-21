namespace SharePicker.Models.Database;

public class Ratios
{
    public int RatiosId { get; set; }
    public int CompanyId { get; set; }
    public required DateOnly Date { get; set; }
    public required decimal? CurrentRatio { get; set; }
    public required decimal? QuickRatio { get; set; }
    public required decimal? CashRatio { get; set; }
    public required decimal? DaysOfSalesOutstanding { get; set; }
    public required decimal? DaysOfInventoryOutstanding { get; set; }
    public required decimal? OperatingCycle { get; set; }
    public required decimal? DaysOfPayablesOutstanding { get; set; }
    public required decimal? CashConversionCycle { get; set; }
    public required decimal? GrossProfitMargin { get; set; }
    public required decimal? OperatingProfitMargin { get; set; }
    public required decimal? PretaxProfitMargin { get; set; }
    public required decimal? NetProfitMargin { get; set; }
    public required decimal? EffectiveTaxRate { get; set; }
    public required decimal? ReturnOnAssets { get; set; }
    public required decimal? ReturnOnEquity { get; set; }
    public required decimal? ReturnOnCapitalEmployed { get; set; }
    public required decimal? NetIncomePerEBT { get; set; }
    public required decimal? EbtPerEbit { get; set; }
    public required decimal? EbitPerRevenue { get; set; }
    public required decimal? DebtRatio { get; set; }
    public required decimal? DebtEquityRatio { get; set; }
    public required decimal? LongTermDebtToCapitalization { get; set; }
    public required decimal? TotalDebtToCapitalization { get; set; }
    public required decimal? InterestCoverage { get; set; }
    public required decimal? CashFlowToDebtRatio { get; set; }
    public required decimal? CompanyEquityMultiplier { get; set; }
    public required decimal? ReceivablesTurnover { get; set; }
    public required decimal? PayablesTurnover { get; set; }
    public required decimal? InventoryTurnover { get; set; }
    public required decimal? FixedAssetTurnover { get; set; }
    public required decimal? AssetTurnover { get; set; }
    public required decimal? OperatingCashFlowPerShare { get; set; }
    public required decimal? FreeCashFlowPerShare { get; set; }
    public required decimal? CashPerShare { get; set; }
    public required decimal? PayoutRatio { get; set; }
    public required decimal? OperatingCashFlowSalesRatio { get; set; }
    public required decimal? FreeCashFlowOperatingCashFlowRatio { get; set; }
    public required decimal? CashFlowCoverageRatios { get; set; }
    public required decimal? ShortTermCoverageRatios { get; set; }
    public required decimal? CapitalExpenditureCoverageRatio { get; set; }
    public required decimal? DividendPaidAndCapexCoverageRatio { get; set; }
    public required decimal? DividendPayoutRatio { get; set; }
    public required decimal? PriceBookValueRatio { get; set; }
    public required decimal? PriceToBookRatio { get; set; }
    public required decimal? PriceToSalesRatio { get; set; }
    public required decimal? PriceEarningsRatio { get; set; }
    public required decimal? PriceToFreeCashFlowsRatio { get; set; }
    public required decimal? PriceToOperatingCashFlowsRatio { get; set; }
    public required decimal? PriceCashFlowRatio { get; set; }
    public required decimal? PriceEarningsToGrowthRatio { get; set; }
    public required decimal? PriceSalesRatio { get; set; }
    public required decimal? DividendYield { get; set; }
    public required decimal? EnterpriseValueMultiple { get; set; }
    public required decimal? PriceFairValue { get; set; }
    public required Company Company { get; set; }
}

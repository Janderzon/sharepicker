namespace SharePicker.Models;

public record CashFlow(
    OperationsCashFlow OperationsCashFlow,
    InvestingCashFlow InvestingCashFlow,
    FinancingCashFlow FinancingCashFlow)
{
    public decimal NetCashFlow { get; } =
        OperationsCashFlow.NetCashFlow + 
        InvestingCashFlow.NetCashFlow +
        FinancingCashFlow.NetCashFlow;
}

public record OperationsCashFlow(
    decimal DepreciationAndAmortisation,
    decimal ProfitOnDisposals,
    decimal ChangeInStock,
    decimal ChangeInDebtors,
    decimal ChangeInCreditors,
    decimal ChangeInProvisions,
    decimal ChangeInWorkingCapital,
    decimal Other,
    decimal OperatingCashFlow,
    decimal TaxPaid)
{
    public decimal NetCashFlow { get; } =
        DepreciationAndAmortisation +
        ProfitOnDisposals +
        ChangeInStock +
        ChangeInDebtors +
        ChangeInCreditors +
        ChangeInProvisions +
        ChangeInWorkingCapital +
        Other +
        OperatingCashFlow +
        Other;
}

public record InvestingCashFlow(
    decimal CapitalExpenditure,
    decimal SaleOfFixedAssets,
    decimal Aquisitions,
    decimal SaleOfBusinesses,
    decimal InterestReceived,
    decimal OtherInvestmentsReceived,
    decimal DividendsFromJointVentures,
    decimal Other)
{
    public decimal NetCashFlow { get; } = 
        CapitalExpenditure +
        SaleOfFixedAssets +
        Aquisitions +
        SaleOfBusinesses +
        InterestReceived +
        OtherInvestmentsReceived +
        DividendsFromJointVentures +
        Other;
}

public record FinancingCashFlow(
    decimal NewShareIssues,
    decimal ShareBuyBack,
    decimal NewBorrowing,
    decimal RepaymentOfBorrowing,
    decimal EquityDividendsPaid,
    decimal DividendsPaidToMinorities,
    decimal InterestPaid,
    decimal Other)
{
    public decimal NetCashFlow { get; } = 
        NewShareIssues +
        ShareBuyBack +
        NewBorrowing +
        RepaymentOfBorrowing +
        EquityDividendsPaid +
        DividendsPaidToMinorities +
        InterestPaid +
        Other;
}

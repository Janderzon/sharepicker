namespace SharePicker.Models;

public record CashFlowStatement(
    DateTimeOffset DateTimeOffset,
    OperationsCashFlow OperationsCashFlow,
    InvestingCashFlow InvestingCashFlow,
    FinancingCashFlow FinancingCashFlow,
    decimal NetCashFlow) : Statement;

public record OperationsCashFlow(
    decimal OperatingProfit,
    decimal DepreciationAndAmortisation,
    decimal ProfitOnDisposals,
    decimal ChangeInStock,
    decimal ChangeInDebtors,
    decimal ChangeInCreditors,
    decimal ChangeInProvisions,
    decimal ChangeInWorkingCapital,
    decimal Other,
    decimal OperatingCashFlow,
    decimal TaxPaid,
    decimal NetCashFlow);

public record InvestingCashFlow(
    decimal CapitalExpenditure,
    decimal SaleOfFixedAssets,
    decimal Aquisitions,
    decimal SaleOfBusinesses,
    decimal InterestReceived,
    decimal OtherInvestmentsReceived,
    decimal DividendsFromJointVentures,
    decimal Other,
    decimal NetCashFlow);

public record FinancingCashFlow(
    decimal NewShareIssues,
    decimal ShareBuyBack,
    decimal NewBorrowing,
    decimal RepaymentOfBorrowing,
    decimal EquityDividendsPaid,
    decimal DividendsPaidToMinorities,
    decimal InterestPaid,
    decimal Other,
    decimal NetCashFlow);

namespace SharePicker.Models;

public record CashFlowStatement(
    DateOnly Date,
    OperationsCashFlow OperationsCashFlow,
    InvestingCashFlow InvestingCashFlow,
    FinancingCashFlow FinancingCashFlow,
    decimal NetChangeInCash);

public record OperationsCashFlow(
    //decimal OperatingProfit,
    decimal DepreciationAndAmortisation,
    //decimal ProfitOnDisposals,
    decimal ChangeInStock,
    decimal ChangeInDebtors,
    //decimal ChangeInCreditors,
    //decimal ChangeInProvisions,
    decimal ChangeInWorkingCapital,
    //decimal Other,
    //decimal OperatingCashFlow,
    //decimal TaxPaid,
    decimal NetCashFromOperations);

public record InvestingCashFlow(
    decimal CapitalExpenditure,
    decimal Aquisitions,
    decimal PurchasesOfInvestments,
    decimal SaleOrMaturityOfInvestments,
    decimal OtherInvestingActivities,
    decimal NetCashFromInvesting);

public record FinancingCashFlow(
    decimal NewShareIssues,
    decimal ShareBuyBack,
    decimal RepaymentOfBorrowing,
    decimal DividendsPaid,
    decimal OtherFinancingActivities,
    decimal NetCashFromFinancing);

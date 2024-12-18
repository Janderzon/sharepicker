﻿namespace SharePicker.Models;

public record CashFlowStatement(
    DateOnly Date,
    string Currency,
    OperationsCashFlow OperationsCashFlow,
    InvestingCashFlow InvestingCashFlow,
    FinancingCashFlow FinancingCashFlow,
    decimal NetChangeInCash);

public record OperationsCashFlow(
    decimal NetIncome,
    decimal DepreciationAndAmortisation,
    decimal DeferredIncomeTax,
    decimal StockBasedCompensation,
    decimal AccountsReceivables,
    decimal Inventory,
    decimal AccountsPayables,
    decimal OtherWorkingCapital,
    decimal ChangeInWorkingCapital,
    decimal OtherNonCashItems,
    decimal NetCashFromOperations);

public record InvestingCashFlow(
    decimal CapitalExpenditure,
    decimal Acquisitions,
    decimal PurchasesOfInvestments,
    decimal SaleOrMaturityOfInvestments,
    decimal OtherInvestingActivities,
    decimal NetCashFromInvesting);

public record FinancingCashFlow(
    decimal SharesIssued,
    decimal SharesRepurchased,
    decimal DebtRepayment,
    decimal DividendsPaid,
    decimal OtherFinancingActivities,
    decimal NetCashFromFinancing);

namespace SharePicker.Models.Fmp;

public  record CashFlowStatementDto
{
    public required string Date { get; init; }
    public required decimal NetIncome { get; init; }
    public required decimal DepreciationAndAmortization { get; init; }
    public required decimal DeferredIncomeTax { get; init; }
    public required decimal StockBasedCompensation { get; init; }
    public required decimal ChangeInWorkingCapital { get; init; }
    public required decimal AccountsReceivables { get; init; }
    public required decimal Inventory { get; init; }
    public required decimal AccountsPayables { get; init; }
    public required decimal OtherWorkingCapital { get; init; }
    public required decimal OtherNonCashItems { get; init; }
    public required decimal NetCashProvidedByOperatingActivities { get; init; }
    public required decimal InvestmentsInPropertyPlantAndEquipment { get; init; }
    public required decimal AcquisitionsNet { get; init; }
    public required decimal PurchasesOfInvestments { get; init; }
    public required decimal SalesMaturitiesOfInvestments { get; init; }
    public required decimal OtherInvestingActivites { get; init; }
    public required decimal NetCashUsedForInvestingActivites { get; init; }
    public required decimal DebtRepayment { get; init; }
    public required decimal CommonStockIssued { get; init; }
    public required decimal CommonStockRepurchased { get; init; }
    public required decimal DividendsPaid { get; init; }
    public required decimal OtherFinancingActivites { get; init; }
    public required decimal NetCashUsedProvidedByFinancingActivities { get; init; }
    public required decimal NetChangeInCash { get; init; }
    public required decimal OperatingCashFlow { get; init; }
    public required decimal CapitalExpenditure { get; init; }

    public CashFlowStatement ToDomain() => new(
        DateTimeOffset.ParseExact(Date, "yyyy-MM-dd", null),
        new OperationsCashFlow(
            NetIncome,
            DepreciationAndAmortization,
            StockBasedCompensation,
            Inventory,
            AccountsReceivables,
            AccountsPayables,
            OtherWorkingCapital,
            ChangeInWorkingCapital,
            OtherNonCashItems,
            OperatingCashFlow,
            DeferredIncomeTax,
            NetCashProvidedByOperatingActivities),
        new InvestingCashFlow(
            CapitalExpenditure,
            InvestmentsInPropertyPlantAndEquipment,
            AcquisitionsNet,
            PurchasesOfInvestments,
            0,
            SalesMaturitiesOfInvestments,
            0,
            OtherInvestingActivites,
            NetCashUsedForInvestingActivites),
        new FinancingCashFlow(
            CommonStockIssued,
            CommonStockRepurchased,
            0,
            DebtRepayment,
            DividendsPaid,
            0,
            0,
            OtherFinancingActivites,
            NetCashUsedProvidedByFinancingActivities),
        NetChangeInCash);
}

USE master
ALTER DATABASE Sharepicker SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE Sharepicker
GO

/* Create database */
CREATE DATABASE Sharepicker
GO

/* Use the Sharepicker database */
USE Sharepicker
GO

/* Create tables */
CREATE TABLE Exchanges (
    ExchangeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Symbol NVARCHAR(20) NOT NULL 
)

CREATE TABLE Companies (
    CompanyId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Symbol NVARCHAR(20) NOT NULL,
    ExchangeId INT NOT NUll

    CONSTRAINT FK_Companies_Exchanges FOREIGN KEY (ExchangeId)
        REFERENCES dbo.Exchanges (ExchangeId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
)

CREATE TABLE Currencies (
    CurrencyId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Symbol NVARCHAR(20) NOT NULL
)

CREATE TABLE BalanceSheetStatements (
    BalanceSheetStatementId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyId INT NOT NULL,
    CurrencyId INT NOT NULL,
    Date DATE NOT NULL,
    CashAndCashEquivalents DECIMAL(19,4) NOT NULL,
    ShortTermInvestments DECIMAL(19,4) NOT NULL,
    NetReceivables DECIMAL(19,4) NOT NULL,
    Inventory DECIMAL(19,4) NOT NULL,
    OtherCurrentAssets DECIMAL(19,4) NOT NULL,
    TotalCurrentAssets DECIMAL(19,4) NOT NULL,
    PropertyPlantEquipmentNet DECIMAL(19,4) NOT NULL,
    Goodwill DECIMAL(19,4) NOT NULL,
    IntangibleAssets DECIMAL(19,4) NOT NULL,
    LongTermInvestments DECIMAL(19,4) NOT NULL,
    TaxAssets DECIMAL(19,4) NOT NULL,
    OtherNonCurrentAssets DECIMAL(19,4) NOT NULL,
    TotalNonCurrentAssets DECIMAL(19,4) NOT NULL,
    OtherAssets DECIMAL(19,4) NOT NULL,
    TotalAssets DECIMAL(19,4) NOT NULL,
    AccountPayables DECIMAL(19,4) NOT NULL,
    ShortTermDebt DECIMAL(19,4) NOT NULL,
    TaxPayables DECIMAL(19,4) NOT NULL,
    DeferredRevenue DECIMAL(19,4) NOT NULL,
    OtherCurrentLiabilities DECIMAL(19,4) NOT NULL,
    TotalCurrentLiabilities DECIMAL(19,4) NOT NULL,
    LongTermDebt DECIMAL(19,4) NOT NULL,
    DeferredRevenueNonCurrent DECIMAL(19,4) NOT NULL,
    DeferredTaxLiabilitiesNonCurrent DECIMAL(19,4) NOT NULL,
    MinorityInterest DECIMAL(19,4) NOT NULL,
    CapitalLeaseObligations DECIMAL(19,4) NOT NULL,
    OtherNonCurrentLiabilities DECIMAL(19,4) NOT NULL,
    TotalNonCurrentLiabilities DECIMAL(19,4) NOT NULL,
    OtherLiabilities DECIMAL(19,4) NOT NULL,
    TotalLiabilities DECIMAL(19,4) NOT NULL,
    PreferredStock DECIMAL(19,4) NOT NULL,
    CommonStock DECIMAL(19,4) NOT NULL,
    RetainedEarnings DECIMAL(19,4) NOT NULL,
    AccumulatedOtherComprehensiveIncomeLoss DECIMAL(19,4) NOT NULL,
    OtherTotalStockholdersEquity DECIMAL(19,4) NOT NULL,
    TotalStockholdersEquity DECIMAL(19,4) NOT NULL,
    TotalEquity DECIMAL(19,4) NOT NULL,
    TotalInvestments DECIMAL(19,4) NOT NULL,
    TotalDebt DECIMAL(19,4) NOT NULL,
    NetDebt DECIMAL(19,4) NOT NULL

    CONSTRAINT FK_BalanceSheetStatements_Companies FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (CompanyId),
    CONSTRAINT FK_BalanceSheetStatements_Currencies FOREIGN KEY (CurrencyId)
        REFERENCES dbo.Currencies (CurrencyId)
)
GO

CREATE TABLE CashFlowStatements (
    CashFlowStatementId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyId INT NOT NULL,
    Date DATE NOT NULL,
    CurrencyId INT NOT NULL,
    NetIncome DECIMAL(19,4) NOT NULL,
    DepreciationAndAmortisation DECIMAL(19,4) NOT NULL,
    DeferredIncomeTax DECIMAL(19,4) NOT NULL,
    StockBasedCompensation DECIMAL(19,4) NOT NULL,
    AccountsReceivables DECIMAL(19,4) NOT NULL,
    Inventory DECIMAL(19,4) NOT NULL,
    AccountsPayables DECIMAL(19,4) NOT NULL,
    OtherWorkingCapital DECIMAL(19,4) NOT NULL,
    ChangeInWorkingCapital DECIMAL(19,4) NOT NULL,
    OtherNonCashItems DECIMAL(19,4) NOT NULL,
    NetCashFromOperations DECIMAL(19,4) NOT NULL,
    CapitalExpenditure DECIMAL(19,4) NOT NULL,
    Acquisitions DECIMAL(19,4) NOT NULL,
    PurchasesOfInvestments DECIMAL(19,4) NOT NULL,
    SaleOrMaturityOfInvestments DECIMAL(19,4) NOT NULL,
    OtherInvestingActivities DECIMAL(19,4) NOT NULL,
    NetCashFromInvesting DECIMAL(19,4) NOT NULL,
    SharesIssued DECIMAL(19,4) NOT NULL,
    SharesRepurchased DECIMAL(19,4) NOT NULL,
    DebtRepayment DECIMAL(19,4) NOT NULL,
    DividendsPaid DECIMAL(19,4) NOT NULL,
    OtherFinancingActivities DECIMAL(19,4) NOT NULL,
    NetCashFromFinancing DECIMAL(19,4) NOT NULL,
    NetChangeInCash DECIMAL(19,4) NOT NULL

    CONSTRAINT FK_CashFlowStatements_Companies FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (CompanyId),
    CONSTRAINT FK_CashFlowStatements_Currencies FOREIGN KEY (CurrencyId)
        REFERENCES dbo.Currencies (CurrencyId)
)

CREATE TABLE IncomeStatements (
    IncomeStatementId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyId INT NOT NULL,
    Date DATE NOT NULL, 
    CurrencyId INT NOT NULL,
    Revenue DECIMAL(19,4) NOT NULL,
    CostOfSales DECIMAL(19,4) NOT NULL,
    GrossProfit DECIMAL(19,4) NOT NULL,
    ResearchAndDevelopmentCosts DECIMAL(19,4) NOT NULL,
    DistributionCosts DECIMAL(19,4) NOT NULL,
    AdministrativeCosts DECIMAL(19,4) NOT NULL,
    OtherCosts DECIMAL(19,4) NOT NULL,
    OperatingProfit DECIMAL(19,4) NOT NULL,
    ProfitBeforeIncomeAndTaxation DECIMAL(19,4) NOT NULL,
    FinanceIncome DECIMAL(19,4) NOT NULL,
    FinanceExpense DECIMAL(19,4) NOT NULL,
    ProfitBeforeTax DECIMAL(19,4) NOT NULL,
    Taxation DECIMAL(19,4) NOT NULL,
    ProfitAfterTax DECIMAL(19,4) NOT NULL,
    NetProfit DECIMAL(19,4) NOT NULL,
    EarningsPerShare DECIMAL(19,4),
    EarningsPerShareDiluted DECIMAL(19,4)

    CONSTRAINT FK_IncomeStatements_Companies FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (CompanyId),
    CONSTRAINT FK_IncomeStatements_Currencies FOREIGN KEY (CurrencyId)
        REFERENCES dbo.Currencies (CurrencyId)
)

CREATE TABLE Ratios (
    RatiosId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyId INT NOT NULL,
    Date DATE NOT NULL
    CurrentRatio DECIMAL(19,4),
    QuickRatio DECIMAL(19,4),
    CashRatio DECIMAL(19,4),
    DaysOfSalesOutstanding DECIMAL(19,4),
    DaysOfInventoryOutstanding DECIMAL(19,4),
    OperatingCycle DECIMAL(19,4),
    DaysOfPayablesOutstanding DECIMAL(19,4),
    CashConversionCycle DECIMAL(19,4)
    GrossProfitMargin DECIMAL(19,4),
    OperatingProfitMargin DECIMAL(19,4),
    PretaxProfitMargin DECIMAL(19,4),
    NetProfitMargin DECIMAL(19,4),
    EffectiveTaxRate DECIMAL(19,4),
    ReturnOnAssets DECIMAL(19,4),
    ReturnOnEquity DECIMAL(19,4),
    ReturnOnCapitalEmployed DECIMAL(19,4),
    NetIncomePerEBT DECIMAL(19,4),
    EbtPerEbit DECIMAL(19,4),
    EbitPerRevenue DECIMAL(19,4),
    DebtRatio DECIMAL(19,4),
    DebtEquityRatio DECIMAL(19,4),
    LongTermDebtToCapitalization DECIMAL(19,4),
    TotalDebtToCapitalization DECIMAL(19,4),
    InterestCoverage DECIMAL(19,4),
    CashFlowToDebtRatio DECIMAL(19,4),
    CompanyEquityMultiplier DECIMAL(19,4),
    ReceivablesTurnover DECIMAL(19,4),
    PayablesTurnover DECIMAL(19,4),
    InventoryTurnover DECIMAL(19,4),
    FixedAssetTurnover DECIMAL(19,4),
    AssetTurnover DECIMAL(19,4),
    OperatingCashFlowPerShare DECIMAL(19,4),
    FreeCashFlowPerShare DECIMAL(19,4),
    CashPerShare DECIMAL(19,4),
    PayoutRatio DECIMAL(19,4),
    OperatingCashFlowSalesRatio DECIMAL(19,4),
    FreeCashFlowOperatingCashFlowRatio DECIMAL(19,4),
    CashFlowCoverageRatios DECIMAL(19,4),
    ShortTermCoverageRatios DECIMAL(19,4),
    CapitalExpenditureCoverageRatio DECIMAL(19,4),
    DividendPaidAndCapexCoverageRatio DECIMAL(19,4),
    DividendPayoutRatio DECIMAL(19,4),
    PriceBookValueRatio DECIMAL(19,4),
    PriceToBookRatio DECIMAL(19,4),
    PriceToSalesRatio DECIMAL(19,4),
    PriceEarningsRatio DECIMAL(19,4),
    PriceToFreeCashFlowsRatio DECIMAL(19,4),
    PriceToOperatingCashFlowsRatio DECIMAL(19,4),
    PriceCashFlowRatio DECIMAL(19,4),
    PriceEarningsToGrowthRatio DECIMAL(19,4),
    PriceSalesRatio DECIMAL(19,4),
    DividendYield DECIMAL(19,4),
    EnterpriseValueMultiple DECIMAL(19,4),
    PriceFairValue DECIMAL(19,4)

    CONSTRAINT FK_IncomeStatements_Companies FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (CompanyId),
)

GO
USE master
GO

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
    Name NVARCHAR(50) NOT NULL,
    Symbol NVARCHAR(20) NOT NULL 
)

CREATE TABLE Companies (
    CompanyId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Symbol NVARCHAR(20) NOT NULL,
    ExchangeId INT NOT NUll

    CONSTRAINT FK_Companies_Exchanges FOREIGN KEY (ExchangeId)
        REFERENCES dbo.Exchanges (ExchangeId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
)

CREATE TABLE BalanceSheetStatements (
    BalanceSheetStatementId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyId INT NOT NULL,
    Date DATE NOT NULL,
    CashAndCashEquivalents DECIMAL(19,4) NOT NULL,
    ShortTermInvestments DECIMAL(19,4) NOT NULL,
    CashAndShortTermInvestments DECIMAL(19,4) NOT NULL,
    NetReceivables DECIMAL(19,4) NOT NULL,
    Inventory DECIMAL(19,4) NOT NULL,
    OtherCurrentAssets DECIMAL(19,4) NOT NULL,
    TotalCurrentAssets DECIMAL(19,4) NOT NULL,
    PropertyPlantEquipmentNet DECIMAL(19,4) NOT NULL,
    Goodwill DECIMAL(19,4) NOT NULL,
    IntangibleAssets DECIMAL(19,4) NOT NULL,
    GoodwillAndIntangibleAssets DECIMAL(19,4) NOT NULL,
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
    OtherNonCurrentLiabilities DECIMAL(19,4) NOT NULL,
    TotalNonCurrentLiabilities DECIMAL(19,4) NOT NULL,
    OtherLiabilities DECIMAL(19,4) NOT NULL,
    CapitalLeaseObligations DECIMAL(19,4) NOT NULL,
    TotalLiabilities DECIMAL(19,4) NOT NULL,
    PreferredStock DECIMAL(19,4) NOT NULL,
    CommonStock DECIMAL(19,4) NOT NULL,
    RetainedEarnings DECIMAL(19,4) NOT NULL,
    AccumulatedOtherComprehensiveIncomeLoss DECIMAL(19,4) NOT NULL,
    OtherTotalStockholdersEquity DECIMAL(19,4) NOT NULL,
    TotalStockholdersEquity DECIMAL(19,4) NOT NULL,
    TotalEquity DECIMAL(19,4) NOT NULL,
    TotalLiabilitiesAndStockholdersEquity DECIMAL(19,4) NOT NULL,
    MinorityInterest DECIMAL(19,4) NOT NULL,
    TotalLiabilitiesAndTotalEquity DECIMAL(19,4) NOT NULL,
    TotalInvestments DECIMAL(19,4) NOT NULL,
    TotalDebt DECIMAL(19,4) NOT NULL,
    NetDebt DECIMAL(19,4) NOT NULL

    CONSTRAINT FK_BalanceSheetStatements_Companies FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (CompanyId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
)

CREATE TABLE CashFlowStatements (
    CashFlowStatementId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyId INT NOT NULL,
    Date DATE NOT NULL,
    NetIncome DECIMAL(19,4) NOT NULL,
    DepreciationAndAmortization DECIMAL(19,4) NOT NULL,
    DeferredIncomeTax DECIMAL(19,4) NOT NULL,
    StockBasedCompensation DECIMAL(19,4) NOT NULL,
    ChangeInWorkingCapital DECIMAL(19,4) NOT NULL,
    AccountsReceivables DECIMAL(19,4) NOT NULL,
    Inventory DECIMAL(19,4) NOT NULL,
    AccountsPayables DECIMAL(19,4) NOT NULL,
    OtherWorkingCapital DECIMAL(19,4) NOT NULL,
    OtherNonCashItems DECIMAL(19,4) NOT NULL,
    NetCashProvidedByOperatingActivities DECIMAL(19,4) NOT NULL,
    InvestmentsInPropertyPlantAndEquipment DECIMAL(19,4) NOT NULL,
    AcquisitionsNet DECIMAL(19,4) NOT NULL,
    PurchasesOfInvestments DECIMAL(19,4) NOT NULL,
    SalesMaturitiesOfInvestments DECIMAL(19,4) NOT NULL,
    OtherInvestingActivites DECIMAL(19,4) NOT NULL,
    NetCashUsedForInvestingActivites DECIMAL(19,4) NOT NULL,
    DebtRepayment DECIMAL(19,4) NOT NULL,
    CommonStockIssued DECIMAL(19,4) NOT NULL,
    CommonStockRepurchased DECIMAL(19,4) NOT NULL,
    DividendsPaid DECIMAL(19,4) NOT NULL,
    OtherFinancingActivites DECIMAL(19,4) NOT NULL,
    NetCashUsedProvidedByFinancingActivities DECIMAL(19,4) NOT NULL,
    NetChangeInCash DECIMAL(19,4) NOT NULL,
    OperatingCashFlow DECIMAL(19,4) NOT NULL,
    CapitalExpenditure DECIMAL(19,4) NOT NULL

    CONSTRAINT FK_CashFlowStatements_Companies FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (CompanyId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
)

CREATE TABLE IncomeStatemets (
    IncomeStatementId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyId INT NOT NULL,
    Date DATE NOT NULL, 
    ReportedCurrency DECIMAL(19,4) NOT NULL,
    Revenue DECIMAL(19,4) NOT NULL,
    GrossProfit DECIMAL(19,4) NOT NULL,
    DepreciationAndAmortization DECIMAL(19,4) NOT NULL,
    EbitDa DECIMAL(19,4) NOT NULL,
    OperatingIncome DECIMAL(19,4) NOT NULL,
    IncomeBeforeTax DECIMAL(19,4) NOT NULL,
    IncomeTaxExpense DECIMAL(19,4) NOT NULL,
    Eps DECIMAL(19,4),
    EpsDiluted DECIMAL(19,4)

    CONSTRAINT FK_IncomeStatements_Companies FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (CompanyId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION 
)

GO

INSERT INTO dbo.Exchanges (Name, Symbol)
VALUES ('London Stock Exchange', 'LSE')
GO
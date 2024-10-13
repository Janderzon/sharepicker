USE master
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
    DepreciationAndAmortization DECIMAL(19,4) NOT NULL,
    DeferredIncomeTax DECIMAL(19,4) NOT NULL,
    StockBasedCompensation DECIMAL(19,4) NOT NULL,
    AccountsReceivables DECIMAL(19,4) NOT NULL,
    Inventory DECIMAL(19,4) NOT NULL,
    AccountsPayables DECIMAL(19,4) NOT NULL,
    OtherWorkingCapital DECIMAL(19,4) NOT NULL,
    ChangeInWorkingCapital DECIMAL(19,4) NOT NULL,
    OtherNonCashItems DECIMAL(19,4) NOT NULL,
    NetCashFlowFromOperations DECIMAL(19,4) NOT NULL,
    CapitalExpenditure DECIMAL(19,4) NOT NULL,
    Acquisitions DECIMAL(19,4) NOT NULL,
    PurchasesOfInvestments DECIMAL(19,4) NOT NULL,
    SaleOrMaturityOfInvestments DECIMAL(19,4) NOT NULL,
    OtherInvestingActivites DECIMAL(19,4) NOT NULL,
    NetCashFromInvesting DECIMAL(19,4) NOT NULL,
    SharesIssued DECIMAL(19,4) NOT NULL,
    SharesRepurchased DECIMAL(19,4) NOT NULL,
    DebtRepayment DECIMAL(19,4) NOT NULL,
    DividendsPaid DECIMAL(19,4) NOT NULL,
    OtherFinancingActivites DECIMAL(19,4) NOT NULL,
    NetCashFromFinancing DECIMAL(19,4) NOT NULL,
    NetChangeInCash DECIMAL(19,4) NOT NULL

    CONSTRAINT FK_CashFlowStatements_Companies FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (CompanyId),
    CONSTRAINT FK_CashFlowStatements_Currencies FOREIGN KEY (CurrencyId)
        REFERENCES dbo.Currencies (CurrencyId)
)

CREATE TABLE IncomeStatemets (
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
    EarningsPerShare DECIMAL(19,4),
    EarningsPerShareDiluted DECIMAL(19,4)

    CONSTRAINT FK_IncomeStatements_Companies FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (CompanyId),
    CONSTRAINT FK_IncomeStatements_Currencies FOREIGN KEY (CurrencyId)
        REFERENCES dbo.Currencies (CurrencyId)
)

GO

INSERT INTO dbo.Exchanges (Name, Symbol)
VALUES ('London Stock Exchange', 'LSE')
GO
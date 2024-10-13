namespace SharePicker.Models;

public record IncomeStatement(
    DateOnly Date,
    decimal Revenue,
    decimal CostOfSales,
    decimal GrossProfit,
    decimal ResearchAndDevelopmentCosts,
    decimal DistributionCosts,
    decimal AdministrativeCosts,
    decimal OtherCosts,
    decimal OperatingProfit,
    decimal ProfitBeforeIncomeAndTaxation,
    decimal FinanceIncome,
    decimal FinanceExpense,
    decimal ProfitBeforeTax,
    decimal Taxation,
    decimal ProfitAfterTax,
    decimal NetProfit,
    decimal? EarningsPerShare,
    decimal? DilutedEarningsPerShare);

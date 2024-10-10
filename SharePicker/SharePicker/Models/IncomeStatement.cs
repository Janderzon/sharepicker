namespace SharePicker.Models;

public record IncomeStatement(
    DateOnly Date,
    decimal Revenue,
    decimal CostOfSales,
    decimal GrossProfit,
    decimal ResearchAndDevelopmentCosts,
    decimal AdministrativeCosts,
    decimal DistributionCosts
    //decimal OperatingProfit,
    //decimal ProfitBeforeInterestAndTax,
    //decimal ProfitBeforeTax,
    //decimal ProfitAfterTax,
    //decimal? EarningsPerShare,
    //decimal? DilutedEarningsPerShare
    );

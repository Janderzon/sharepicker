using JetBrains.Annotations;

namespace SharePicker.Models.Fmp;

[PublicAPI]
public record IncomeStatementDto
{
    public required string Date { get; init; }
    public required string ReportedCurrency { get; init; }
    public required decimal Revenue { get; init; }
    public required decimal CostOfRevenue { get; init; }
    public required decimal GrossProfit { get; init; }
    public required decimal ResearchAndDevelopmentExpenses { get; init; }
    public required decimal GeneralAndAdministrativeExpenses { get; init; }
    public required decimal SellingAndMarketingExpenses { get; init; }
    public required decimal SellingGeneralAndAdministrativeExpenses { get; init; }
    public required decimal OtherExpenses { get; init; }
    public required decimal OperatingExpenses { get; init; }
    public required decimal CostAndExpenses { get; init; }
    public required decimal InterestIncome { get; init; }
    public required decimal InterestExpense { get; init; }
    public required decimal DepreciationAndAmortization { get; init; }
    public required decimal EbitDa { get; init; }
    public required decimal OperatingIncome { get; init; }
    public required decimal TotalOtherIncomeExpensesNet { get; init; }
    public required decimal IncomeBeforeTax { get; init; }
    public required decimal IncomeTaxExpense { get; init; }
    public required decimal NetIncome { get; init; }
    public required decimal? Eps { get; init; }
    public required decimal? EpsDiluted { get; init; }
}

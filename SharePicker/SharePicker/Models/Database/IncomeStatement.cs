namespace SharePicker.Models.Database;

public class IncomeStatement
{
    public int IncomeStatementId { get; set; }
    public int CompanyId { get; set; }
    public required DateOnly Date { get; set; }
    public int CurrencyId { get; set; }
    public required decimal Revenue { get; set; }
    public required decimal CostOfSales { get; set; }
    public required decimal GrossProfit { get; set; }
    public required decimal ResearchAndDevelopmentCosts { get; set; }
    public required decimal DistributionCosts { get; set; }
    public required decimal AdministrativeCosts { get; set; }
    public required decimal OtherCosts { get; set; }
    public required decimal OperatingProfit { get; set; }
    public required decimal ProfitBeforeIncomeAndTaxation { get; set; }
    public required decimal FinanceIncome { get; set; }
    public required decimal FinanceExpense { get; set; }
    public required decimal ProfitBeforeTax { get; set; }
    public required decimal Taxation { get; set; }
    public required decimal ProfitAfterTax { get; set; }
    public required decimal NetProfit { get; set; }
    public required decimal? EarningsPerShare { get; set; }
    public required decimal? EarningsPerShareDiluted { get; set; }
    public required Company Company { get; set; }
    public required Currency Currency { get; set; }
}

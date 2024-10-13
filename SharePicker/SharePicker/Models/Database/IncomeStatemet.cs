namespace SharePicker.Models.Database;

public class IncomeStatemet
{
    public int IncomeStatementId { get; set; }

    public int CompanyId { get; set; }

    public DateOnly Date { get; set; }

    public int CurrencyId { get; set; }

    public decimal Revenue { get; set; }

    public decimal CostOfSales { get; set; }

    public decimal GrossProfit { get; set; }

    public decimal ResearchAndDevelopmentCosts { get; set; }

    public decimal DistributionCosts { get; set; }

    public decimal AdministrativeCosts { get; set; }

    public decimal OtherCosts { get; set; }

    public decimal OperatingProfit { get; set; }

    public decimal ProfitBeforeIncomeAndTaxation { get; set; }

    public decimal FinanceIncome { get; set; }

    public decimal FinanceExpense { get; set; }

    public decimal ProfitBeforeTax { get; set; }

    public decimal Taxation { get; set; }

    public decimal ProfitAfterTax { get; set; }

    public decimal? EarningsPerShare { get; set; }

    public decimal? EarningsPerShareDiluted { get; set; }

    public Company Company { get; set; } = null!;

    public Currency Currency { get; set; } = null!;
}

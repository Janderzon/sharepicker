using SharePicker.Models;

namespace SharePicker.Services;

public class FinancialStatementRepository
{
    public List<LabeledValue> GetEbitPerRevenueSeries(Company company) => company.ratios
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.EbitPerRevenue))
        .ToList();

    public List<LabeledValue> GetRoceSeries(Company company) => company.ratios
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.ReturnOnCapitalEmployed))
        .ToList();

    public List<LabeledValue> GetCapitalTurnoverSeries(Company company) => company.ratios
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.ReturnOnCapitalEmployed / x.EbitPerRevenue))
        .ToList();

    public List<LabeledValue> GetRevenueSeries(Company company) => company.IncomeStatements
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.Revenue))
        .ToList();

    public List<LabeledValue> GetEbitSeries(Company company) => company.IncomeStatements
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.Ebit))
        .ToList();
}

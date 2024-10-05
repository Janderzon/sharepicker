namespace SharePicker.Models;

public record Company(
    string Symbol,
    string Name,
    string Exchange,
    List<BalanceSheetStatement> BalanceSheetStatements,
    List<CashFlowStatement> CashFlowStatements,
    List<IncomeStatement> IncomeStatements,
    List<Ratios> Ratios)
{
    public List<LabeledValue> GetEbitPerRevenueSeries() => Ratios
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.EbitPerRevenue))
        .ToList();

    public List<LabeledValue> GetRoceSeries() => Ratios
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.ReturnOnCapitalEmployed))
        .ToList();

    public List<LabeledValue> GetCapitalTurnoverSeries() => Ratios
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.ReturnOnCapitalEmployed / x.EbitPerRevenue))
        .ToList();

    public List<LabeledValue> GetRevenueSeries() => IncomeStatements
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.Revenue))
        .ToList();

    public List<LabeledValue> GetEbitSeries() => IncomeStatements
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.Ebit))
        .ToList();
};

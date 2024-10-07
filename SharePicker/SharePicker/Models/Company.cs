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
        .Where(x => x.ReturnOnCapitalEmployed != null)
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.ReturnOnCapitalEmployed!.Value))
        .ToList();

    public List<LabeledValue> GetCapitalTurnoverSeries() => Ratios
        .Where(x => x.ReturnOnCapitalEmployed != null)
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.ReturnOnCapitalEmployed!.Value / x.EbitPerRevenue))
        .ToList();

    public List<LabeledValue> GetRevenueSeries() => IncomeStatements
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.Revenue))
        .ToList();

    public List<LabeledValue> GetEbitSeries() => IncomeStatements
        .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.Ebit))
        .ToList();
};

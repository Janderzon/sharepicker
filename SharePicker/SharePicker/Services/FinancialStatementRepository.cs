using SharePicker.Components;
using SharePicker.Models;

namespace SharePicker.Services;

public class FinancialStatementRepository(FmpClient fmpClient)
{
    public async Task<HashSet<Company>> GetCompaniesAsync(CancellationToken cancellationToken)
    {
        var tradableCompanies = await fmpClient.GetTradableCompaniesAsync(cancellationToken);

        var symbolsWithFinancialStatements = await fmpClient.GetSymbolsWithFinancialStatementsAsync(cancellationToken);

        return tradableCompanies
            .Where(company => symbolsWithFinancialStatements.Contains(company.Symbol))
            .ToHashSet();
    }

    public Task<List<IncomeStatement>> GetIncomeStatementsAsync(Company company, CancellationToken cancellationToken) =>
        fmpClient.GetIncomeStatementsAsync(company, cancellationToken);

    public Task<List<BalanceSheetStatement>> GetBalanceSheetStatementsAsync(Company company, CancellationToken cancellationToken) =>
        fmpClient.GetBalanceSheetStatementsAsync(company, cancellationToken);

    public Task<List<Ratios>> GetRatiosAsync(Company company, CancellationToken cancellationToken) =>
        fmpClient.GetRatiosAsync(company, cancellationToken);

    public Task<List<CashFlowStatement>> GetCashFlowStatementsAsync(Company company, CancellationToken cancellationToken) =>
        fmpClient.GetCashFlowStatementsAsync(company, cancellationToken);

    public async Task<List<LabeledValue>> GetProfitMarginSeries(Company company, CancellationToken cancellationToken)
    {
        var ratios = await fmpClient.GetRatiosAsync(company, cancellationToken);

        return ratios
            .Select(x => new LabeledValue(x.DateTimeOffset.ToString("yyyy"), x.ProfitMargin))
            .ToList();
    }
}

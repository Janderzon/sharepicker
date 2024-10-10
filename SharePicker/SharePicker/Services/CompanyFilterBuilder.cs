using SharePicker.Models;

namespace SharePicker.Services;

public interface ICompanyFilter
{
    public List<Company> FilterCompanies(IEnumerable<Company> companies);
}

public class CompanyFilterBuilder
{
    private readonly List<Func<Company, bool>> _filters = [];

    public CompanyFilterBuilder WithMinProfitMargin(decimal percentage)
    {
        //_filters.Add(company => company.Ratios.All(ratios => ratios.EbitPerRevenue * 100 >= percentage));
        return this;
    }

    public CompanyFilterBuilder WithMinReturnOnCapitalEmployed(decimal percentage)
    {
        //_filters.Add(company => company.Ratios.All(ratios => ratios.ReturnOnCapitalEmployed * 100 >= percentage));
        return this;
    }

    public CompanyFilterBuilder WithIncreasingProfits()
    {
        _filters.Add(company =>
        {
            //var lastProfit = 0m;
            var decresingProfitCount = 0;
            //foreach (var incomeStatement in company.IncomeStatements)
            //{
            //    if (incomeStatement.Ebit < lastProfit)
            //        decresingProfitCount++;
            //    lastProfit = incomeStatement.Ebit;
            //}
            return decresingProfitCount == 0;
        });
        return this;
    }

    public ICompanyFilter Build() => new CompanyFilter([.._filters]);

    private class CompanyFilter(List<Func<Company, bool>> filters) : ICompanyFilter
    {
        public List<Company> FilterCompanies(IEnumerable<Company> companies) => companies
            .Where(company => filters.All(filter => filter.Invoke(company)))
            .ToList();
    }
}

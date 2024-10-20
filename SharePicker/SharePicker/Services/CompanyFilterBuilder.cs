using SharePicker.Models.Database;

namespace SharePicker.Services;

public interface ICompanyFilter
{
    public IQueryable<Company> FilterCompanies(IQueryable<Company> companies);
}

public class CompanyFilterBuilder
{
    private readonly List<Func<IQueryable<Company>, IQueryable<Company>>> _filters = [];

    public CompanyFilterBuilder WithMinProfitMargin(decimal percentage)
    {
        _filters.Add(filter => filter.Where(company =>
            company.Ratios.All(ratios => ratios.EbitPerRevenue * 100 >= percentage)));
        return this;
    }

    public CompanyFilterBuilder WithMinReturnOnCapitalEmployed(decimal percentage)
    {
        _filters.Add(filter => filter.Where(company =>
            company.Ratios.All(ratios => ratios.ReturnOnCapitalEmployed * 100 >= percentage)));
        return this;
    }

    //public CompanyFilterBuilder WithIncreasingProfits()
    //{
    //    _filters.Add(company =>
    //    {
    //        //var lastProfit = 0m;
    //        var decresingProfitCount = 0;
    //        //foreach (var incomeStatement in company.IncomeStatements)
    //        //{
    //        //    if (incomeStatement.Ebit < lastProfit)
    //        //        decresingProfitCount++;
    //        //    lastProfit = incomeStatement.Ebit;
    //        //}
    //        return decresingProfitCount == 0;
    //    });
    //    return this;
    //}

    public ICompanyFilter Build() => new CompanyFilter([.._filters]);

    private class CompanyFilter(List<Func<IQueryable<Company>, IQueryable<Company>>> filters) : ICompanyFilter
    {
        public IQueryable<Company> FilterCompanies(IQueryable<Company> companies)
        {
            foreach (var filter in filters)
            {
                companies = filter.Invoke(companies);
            }

            return companies;
        }
    }
}

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

    public CompanyFilterBuilder WithIncreasingProfits()
    {
        _filters.Add(filter => filter.Where(company => company.IncomeStatements
            .Join(
                company.IncomeStatements,
                outerStatement => outerStatement.Date.Year,
                innerStatement => innerStatement.Date.Year + 1,
                (outerStatement, innerStatement) => innerStatement.ProfitBeforeIncomeAndTaxation < outerStatement.ProfitBeforeIncomeAndTaxation)
            .Count(result => result == false) <= 2));
        return this;
    }

    public CompanyFilterBuilder WithExchanges(IEnumerable<Models.Exchange> exchanges)
    {
        var exchangesToInclude = exchanges.Select(exchange => exchange.Symbol);
        
        _filters.Add(filter => filter.Where(company => exchangesToInclude.Contains(company.Exchange.Symbol)));
        
        return this;
    }

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

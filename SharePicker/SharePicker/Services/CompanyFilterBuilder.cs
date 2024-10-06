﻿using SharePicker.Models;

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
        _filters.Add(company => company.Ratios.All(ratios => ratios.EbitPerRevenue * 100 >= percentage));
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
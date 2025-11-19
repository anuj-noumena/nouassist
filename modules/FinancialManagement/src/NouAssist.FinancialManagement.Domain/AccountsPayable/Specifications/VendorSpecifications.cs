using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.AccountsPayable.Specifications
{
    public class ActiveVendorsSpecification : Specification<Vendor>
    {
        public override Expression<Func<Vendor, bool>> ToExpression() =>
            v => v.IsActive;
    }

    public class PreferredVendorsSpecification : Specification<Vendor>
    {
        public override Expression<Func<Vendor, bool>> ToExpression() =>
            v => v.IsPreferred && v.IsActive;
    }

    public class VendorsByNameSpecification : Specification<Vendor>
    {
        private readonly string _searchTerm;

        public VendorsByNameSpecification(string searchTerm)
        {
            _searchTerm = searchTerm?.ToLower() ?? string.Empty;
        }

        public override Expression<Func<Vendor, bool>> ToExpression() =>
            v => v.VendorName.ToLower().Contains(_searchTerm);
    }
}

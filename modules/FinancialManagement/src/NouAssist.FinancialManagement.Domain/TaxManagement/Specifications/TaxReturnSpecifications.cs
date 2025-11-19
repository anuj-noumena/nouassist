using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using NouAssist.FinancialManagement.TaxManagement.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.TaxManagement.Specifications
{
    public class UnfiledTaxReturnsSpecification : Specification<TaxReturn>
    {
        public override Expression<Func<TaxReturn, bool>> ToExpression() =>
            t => t.Status != TaxReturnStatus.Filed && t.Status != TaxReturnStatus.Accepted;
    }

    public class TaxReturnsByYearSpecification : Specification<TaxReturn>
    {
        private readonly int _taxYear;

        public TaxReturnsByYearSpecification(int taxYear)
        {
            _taxYear = taxYear;
        }

        public override Expression<Func<TaxReturn, bool>> ToExpression() =>
            t => t.TaxYear == _taxYear;
    }

    public class IncompleteDocumentsSpecification : Specification<TaxReturn>
    {
        public override Expression<Func<TaxReturn, bool>> ToExpression() =>
            t => t.Documents.Any(d => 
                d.Status == TaxDocumentStatus.Pending || 
                d.Status == TaxDocumentStatus.Incomplete);
    }
}

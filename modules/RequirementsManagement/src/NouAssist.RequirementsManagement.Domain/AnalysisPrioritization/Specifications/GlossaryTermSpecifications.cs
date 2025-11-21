using System;
using System.Linq.Expressions;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Specifications
{
    /// <summary>
    /// Base specification class for GlossaryTerm
    /// </summary>
    public abstract class GlossaryTermSpecification : Specification<GlossaryTerm>
    {
    }

    /// <summary>
    /// Specification to find approved glossary terms
    /// </summary>
    public class ApprovedTermsSpecification : GlossaryTermSpecification
    {
        public override Expression<Func<GlossaryTerm, bool>> ToExpression()
        {
            return term => term.IsApproved;
        }
    }

    /// <summary>
    /// Specification to find terms pending approval
    /// </summary>
    public class PendingApprovalSpecification : GlossaryTermSpecification
    {
        public override Expression<Func<GlossaryTerm, bool>> ToExpression()
        {
            return term => !term.IsApproved;
        }
    }

    /// <summary>
    /// Specification to find terms by partial name match
    /// </summary>
    public class TermNameContainsSpecification : GlossaryTermSpecification
    {
        private readonly string _searchTerm;

        public TermNameContainsSpecification(string searchTerm)
        {
            _searchTerm = searchTerm.ToLower();
        }

        public override Expression<Func<GlossaryTerm, bool>> ToExpression()
        {
            return term => term.Name.ToLower().Contains(_searchTerm);
        }
    }

    /// <summary>
    /// Specification to find terms approved by specific user
    /// </summary>
    public class ApprovedByUserSpecification : GlossaryTermSpecification
    {
        private readonly Guid _userId;

        public ApprovedByUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<GlossaryTerm, bool>> ToExpression()
        {
            return term => term.ApprovedBy == _userId;
        }
    }

    /// <summary>
    /// Specification to find terms with synonyms or acronyms
    /// </summary>
    public class HasAlternativeFormsSpecification : GlossaryTermSpecification
    {
        public override Expression<Func<GlossaryTerm, bool>> ToExpression()
        {
            return term => !string.IsNullOrEmpty(term.Synonyms) || 
                          !string.IsNullOrEmpty(term.Acronyms);
        }
    }

    /// <summary>
    /// Specification to find terms created within a date range
    /// </summary>
    public class CreatedWithinRangeSpecification : GlossaryTermSpecification
    {
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;

        public CreatedWithinRangeSpecification(DateTime fromDate, DateTime toDate)
        {
            _fromDate = fromDate;
            _toDate = toDate;
        }

        public override Expression<Func<GlossaryTerm, bool>> ToExpression()
        {
            return term => term.CreationTime >= _fromDate && 
                          term.CreationTime <= _toDate;
        }
    }
}

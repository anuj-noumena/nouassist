using System;
using System.Linq.Expressions;
using NouAssist.Infrastructure.SearchEngine.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.SearchEngine.Specifications
{
    public class RecentSearchesSpecification : Specification<SearchHistory>
    {
        private readonly Guid _userId;
        private readonly DateTime _since;

        public RecentSearchesSpecification(Guid userId, DateTime since)
        {
            _userId = userId;
            _since = since;
        }

        public override Expression<Func<SearchHistory, bool>> ToExpression() =>
            h => h.UserId == _userId && h.SearchedAt >= _since;
    }

    public class SuccessfulSearchesSpecification : Specification<SearchHistory>
    {
        public override Expression<Func<SearchHistory, bool>> ToExpression() =>
            h => h.ResultCount.HasValue && h.ResultCount.Value > 0;
    }

    public class ZeroResultSearchesSpecification : Specification<SearchHistory>
    {
        private readonly Guid _userId;

        public ZeroResultSearchesSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<SearchHistory, bool>> ToExpression() =>
            h => h.UserId == _userId && 
                 h.ResultCount.HasValue && 
                 h.ResultCount.Value == 0;
    }
}

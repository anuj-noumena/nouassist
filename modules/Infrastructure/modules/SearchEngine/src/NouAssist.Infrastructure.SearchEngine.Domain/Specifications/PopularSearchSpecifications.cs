using System;
using System.Linq.Expressions;
using NouAssist.Infrastructure.SearchEngine.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.SearchEngine.Specifications
{
    public class TopPopularSearchesSpecification : Specification<PopularSearch>
    {
        private readonly int _minSearchCount;

        public TopPopularSearchesSpecification(int minSearchCount = 10)
        {
            _minSearchCount = minSearchCount;
        }

        public override Expression<Func<PopularSearch, bool>> ToExpression() =>
            p => p.SearchCount >= _minSearchCount && !p.IsDeleted;
    }

    public class RecentlyPopularSearchesSpecification : Specification<PopularSearch>
    {
        private readonly DateTime _since;

        public RecentlyPopularSearchesSpecification(DateTime since)
        {
            _since = since;
        }

        public override Expression<Func<PopularSearch, bool>> ToExpression() =>
            p => p.LastSearchedAt.HasValue && 
                 p.LastSearchedAt.Value >= _since && 
                 !p.IsDeleted;
    }
}

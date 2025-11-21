using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore
{
    public static class EfCoreRepositoryExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            return query.Skip(skipCount).Take(maxResultCount);
        }

        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPagedAndSortedResultRequest input)
        {
            return query.Skip(input.SkipCount).Take(input.MaxResultCount);
        }
    }
}

using System.Linq;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Entities;

namespace NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore
{
    public static class DocumentManagementEfCoreQueryableExtensions
    {
        public static IQueryable<Document> IncludeDetails(
            this IQueryable<Document> queryable,
            bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Versions);
        }

        public static IQueryable<DocumentVersion> IncludeDetails(
            this IQueryable<DocumentVersion> queryable,
            bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable;
        }
    }
}

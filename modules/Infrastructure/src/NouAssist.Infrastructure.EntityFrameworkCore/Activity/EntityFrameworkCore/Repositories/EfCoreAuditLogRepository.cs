using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Activity.Aggregates;
using NouAssist.Activity.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Activity.EntityFrameworkCore.Repositories
{
    public class EfCoreAuditLogRepository 
        : EfCoreRepository<IActivityDbContext, AuditLog, Guid>, IAuditLogRepository
    {
        public EfCoreAuditLogRepository(
            IDbContextProvider<IActivityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<AuditLog>> GetListAsync(
            ISpecification<AuditLog> spec, 
            int maxResultCount = 100)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .OrderByDescending(a => a.Timestamp)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(ISpecification<AuditLog> spec)
        {
            var query = await GetQueryableAsync();
            return await query.Where(spec.ToExpression()).LongCountAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.ReflectionAndSupport.ActivityTracking.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.Data
{
    public interface IAuditLogRepository : IRepository<AuditLog, Guid>
    {
        Task<List<AuditLog>> GetListAsync(ISpecification<AuditLog> spec, int maxResultCount = 100);
        Task<long> GetCountAsync(ISpecification<AuditLog> spec);
    }
}

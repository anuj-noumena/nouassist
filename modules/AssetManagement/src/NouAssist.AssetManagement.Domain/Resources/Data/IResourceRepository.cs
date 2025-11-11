using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.AssetManagement.Resources.Data
{
    public interface IResourceRepository : IRepository<Resource, Guid>
    {
        Task<Resource> GetWithVersionsAsync(Guid id);
        Task<Resource> GetWithAclAsync(Guid id);
        Task<List<Resource>> GetListAsync(ISpecification<Resource> spec, int maxResultCount = 50);
        Task<List<Resource>> GetByOwnerAsync(Guid ownerId, ResourceKind? kind = null);
        Task<long> GetTotalSizeByOwnerAsync(Guid ownerId);
    }
}

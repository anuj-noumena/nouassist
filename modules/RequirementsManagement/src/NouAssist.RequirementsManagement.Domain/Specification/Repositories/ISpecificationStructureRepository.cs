using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.Specification.Repositories
{
    /// <summary>
    /// Repository interface for SpecificationStructure aggregate
    /// </summary>
    public interface ISpecificationStructureRepository : IRepository<SpecificationStructure, Guid>
    {
        Task<SpecificationStructure> FindByNameAsync(string name, CancellationToken cancellationToken = default);
        
        Task<List<SpecificationStructure>> GetByDocumentTypeAsync(
            string documentType,
            CancellationToken cancellationToken = default);
    }
}

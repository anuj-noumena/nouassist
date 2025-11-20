using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Specifications;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.DocumentManagement.Repositories
{
    /// <summary>
    /// Repository for Document aggregate
    /// </summary>
    public interface IDocumentRepository : IRepository<Document, Guid>
    {
        /// <summary>
        /// Gets documents matching the specification
        /// </summary>
        Task<List<Document>> GetListAsync(
            DocumentSpecification specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets the count of documents matching the specification
        /// </summary>
        Task<int> CountAsync(
            DocumentSpecification specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Checks if any documents match the specification
        /// </summary>
        Task<bool> AnyAsync(
            DocumentSpecification specification,
            CancellationToken cancellationToken = default);
    }
}

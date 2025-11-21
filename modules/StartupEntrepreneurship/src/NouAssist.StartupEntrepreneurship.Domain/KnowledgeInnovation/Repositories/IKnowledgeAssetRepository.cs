using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories
{
    /// <summary>
    /// Repository for KnowledgeAsset aggregate
    /// </summary>
    public interface IKnowledgeAssetRepository : IRepository<KnowledgeAsset, Guid>
    {
        /// <summary>
        /// Find assets by specification
        /// </summary>
        Task<List<KnowledgeAsset>> GetListAsync(
            Specification<KnowledgeAsset> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Count assets matching specification
        /// </summary>
        Task<int> CountAsync(
            Specification<KnowledgeAsset> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if any assets match specification
        /// </summary>
        Task<bool> AnyAsync(
            Specification<KnowledgeAsset> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Find assets by type
        /// </summary>
        Task<List<KnowledgeAsset>> GetByTypeAsync(
            KnowledgeAssetType assetType, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Find assets with tag
        /// </summary>
        Task<List<KnowledgeAsset>> GetByTagAsync(
            string tag, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Find assets with title containing search text
        /// </summary>
        Task<List<KnowledgeAsset>> SearchByTitleAsync(
            string searchText, 
            int maxResults = 10, 
            CancellationToken cancellationToken = default);
    }
}

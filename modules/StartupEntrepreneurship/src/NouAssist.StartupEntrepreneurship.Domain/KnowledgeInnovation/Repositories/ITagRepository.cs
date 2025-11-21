using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories
{
    /// <summary>
    /// Repository for Tag entity
    /// </summary>
    public interface ITagRepository : IRepository
    {
        /// <summary>
        /// Find tag by name
        /// </summary>
        Task<Tag> FindByNameAsync(
            string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all tags
        /// </summary>
        Task<List<Tag>> GetAllAsync(
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get popular tags with usage count above threshold
        /// </summary>
        Task<List<Tag>> GetPopularTagsAsync(
            int usageCountThreshold = 5,
            int maxTagsToReturn = 20,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create tag if it doesn't exist
        /// </summary>
        Task<Tag> CreateIfNotExistsAsync(
            string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete tag
        /// </summary>
        Task DeleteAsync(
            Tag tag,
            CancellationToken cancellationToken = default);
    }
}

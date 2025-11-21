using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories
{
    /// <summary>
    /// Repository for Idea aggregate
    /// </summary>
    public interface IIdeaRepository : IRepository<Idea, Guid>
    {
        /// <summary>
        /// Find ideas by specification
        /// </summary>
        Task<List<Idea>> GetListAsync(
            Specification<Idea> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Count ideas matching specification
        /// </summary>
        Task<int> CountAsync(
            Specification<Idea> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if any ideas match specification
        /// </summary>
        Task<bool> AnyAsync(
            Specification<Idea> specification, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get ideas by status
        /// </summary>
        Task<List<Idea>> GetByStatusAsync(
            IdeaStatus status, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get ideas by creator
        /// </summary>
        Task<List<Idea>> GetByCreatorAsync(
            Guid creatorId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get ideas with high priority
        /// </summary>
        Task<List<Idea>> GetHighPriorityIdeasAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get ideas that can be evaluated
        /// </summary>
        Task<List<Idea>> GetEvaluableIdeasAsync(
            CancellationToken cancellationToken = default);
    }
}

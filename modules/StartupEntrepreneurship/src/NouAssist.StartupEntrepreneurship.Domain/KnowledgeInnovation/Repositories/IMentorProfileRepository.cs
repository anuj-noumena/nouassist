using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories
{
    /// <summary>
    /// Repository for MentorProfile aggregate
    /// </summary>
    public interface IMentorProfileRepository : IRepository<MentorProfile, Guid>
    {
        /// <summary>
        /// Find mentor profiles by specification
        /// </summary>
        Task<List<MentorProfile>> GetListAsync(
            Specification<MentorProfile> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Count mentor profiles matching specification
        /// </summary>
        Task<int> CountAsync(
            Specification<MentorProfile> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if any mentor profiles match specification
        /// </summary>
        Task<bool> AnyAsync(
            Specification<MentorProfile> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get mentors by expertise
        /// </summary>
        Task<List<MentorProfile>> GetByExpertiseAsync(
            string expertiseKeyword,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get experienced mentors with minimum years of experience
        /// </summary>
        Task<List<MentorProfile>> GetExperiencedMentorsAsync(
            int minimumYears,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get mentors with specific tag
        /// </summary>
        Task<List<MentorProfile>> GetByTagAsync(
            string tag,
            CancellationToken cancellationToken = default);
    }
}

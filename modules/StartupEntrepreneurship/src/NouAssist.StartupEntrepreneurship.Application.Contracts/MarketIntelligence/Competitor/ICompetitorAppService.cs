using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor
{
    /// <summary>
    /// Interface for competitor management application service
    /// </summary>
    public interface ICompetitorAppService : IApplicationService
    {
        /// <summary>
        /// Gets a competitor by ID
        /// </summary>
        Task<CompetitorDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a paged list of competitors with optional filtering
        /// </summary>
        Task<PagedResultDto<CompetitorDto>> GetListAsync(GetCompetitorListDto input);

        /// <summary>
        /// Creates a new competitor
        /// </summary>
        Task<CompetitorDto> CreateAsync(CreateCompetitorDto input);

        /// <summary>
        /// Updates an existing competitor
        /// </summary>
        Task<CompetitorDto> UpdateAsync(Guid id, UpdateCompetitorDto input);

        /// <summary>
        /// Deletes a competitor
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Adds an update log to a competitor
        /// </summary>
        Task<CompetitorUpdateLogDto> AddUpdateAsync(Guid id, CreateCompetitorUpdateDto input);

        /// <summary>
        /// Updates the threat level of a competitor
        /// </summary>
        Task<CompetitorDto> UpdateThreatLevelAsync(Guid id, UpdateThreatLevelDto input);

        /// <summary>
        /// Archives a competitor
        /// </summary>
        Task<CompetitorDto> ArchiveAsync(Guid id);

        /// <summary>
        /// Gets competitors by threat level
        /// </summary>
        Task<List<CompetitorDto>> GetByThreatLevelAsync(CompetitorThreatLevel threatLevel);

        /// <summary>
        /// Gets competitors with recent updates
        /// </summary>
        Task<List<CompetitorDto>> GetWithRecentUpdatesAsync(int days = 30, int maxCount = 10);
    }
}

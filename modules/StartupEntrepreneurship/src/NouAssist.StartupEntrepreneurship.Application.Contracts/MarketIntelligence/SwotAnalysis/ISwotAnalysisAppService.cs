using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis
{
    /// <summary>
    /// Interface for SWOT analysis management application service
    /// </summary>
    public interface ISwotAnalysisAppService : IApplicationService
    {
        /// <summary>
        /// Gets a SWOT analysis by ID
        /// </summary>
        Task<SwotAnalysisDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a paged list of SWOT analyses with optional filtering
        /// </summary>
        Task<PagedResultDto<SwotAnalysisDto>> GetListAsync(GetSwotAnalysisListDto input);

        /// <summary>
        /// Creates a new SWOT analysis
        /// </summary>
        Task<SwotAnalysisDto> CreateAsync(CreateSwotAnalysisDto input);

        /// <summary>
        /// Updates an existing SWOT analysis
        /// </summary>
        Task<SwotAnalysisDto> UpdateAsync(Guid id, UpdateSwotAnalysisDto input);

        /// <summary>
        /// Deletes a SWOT analysis
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Adds a SWOT item to an analysis
        /// </summary>
        Task<SwotItemDto> AddItemAsync(Guid id, CreateSwotItemDto input);

        /// <summary>
        /// Updates a SWOT item
        /// </summary>
        Task<SwotItemDto> UpdateItemAsync(Guid analysisId, Guid itemId, CreateSwotItemDto input);

        /// <summary>
        /// Removes a SWOT item from an analysis
        /// </summary>
        Task DeleteItemAsync(Guid analysisId, Guid itemId);

        /// <summary>
        /// Completes a SWOT analysis
        /// </summary>
        Task<SwotAnalysisDto> CompleteAsync(Guid id, CompleteSwotAnalysisDto input);

        /// <summary>
        /// Reopens a completed SWOT analysis
        /// </summary>
        Task<SwotAnalysisDto> ReopenAsync(Guid id);

        /// <summary>
        /// Gets recent SWOT analyses
        /// </summary>
        Task<List<SwotAnalysisDto>> GetRecentAsync(int maxCount = 5);

        /// <summary>
        /// Gets completed SWOT analyses
        /// </summary>
        Task<List<SwotAnalysisDto>> GetCompletedAsync();

        /// <summary>
        /// Evaluates SWOT analysis quality
        /// </summary>
        Task<SwotAnalysisQualityDto> EvaluateQualityAsync(Guid id);

        /// <summary>
        /// Gets SWOT items by category
        /// </summary>
        Task<Dictionary<SwotCategory, List<SwotItemDto>>> GetItemsByCategoryAsync(Guid id);
    }
}

using System;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Traceability.TraceLink.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Traceability.TraceLink
{
    /// <summary>
    /// Application service interface for trace link management
    /// </summary>
    public interface ITraceLinkAppService : IApplicationService
    {
        /// <summary>
        /// Creates a new trace link
        /// </summary>
        Task<TraceLinkDto> CreateAsync(CreateTraceLinkDto input);
        
        /// <summary>
        /// Gets a trace link by ID
        /// </summary>
        Task<TraceLinkDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a paginated list of trace links with filtering
        /// </summary>
        Task<PagedResultDto<TraceLinkDto>> GetListAsync(GetTraceLinkListDto input);
        
        /// <summary>
        /// Updates an existing trace link
        /// </summary>
        Task<TraceLinkDto> UpdateAsync(Guid id, UpdateTraceLinkDto input);
        
        /// <summary>
        /// Deletes a trace link
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Marks a trace link as verified
        /// </summary>
        Task<TraceLinkDto> VerifyAsync(Guid id);
    }
}

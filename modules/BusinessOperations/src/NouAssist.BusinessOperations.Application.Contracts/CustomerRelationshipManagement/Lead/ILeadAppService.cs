using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead
{
    /// <summary>
    /// Application service for lead management
    /// </summary>
    public interface ILeadAppService : IApplicationService
    {
        /// <summary>
        /// Gets a lead by ID
        /// </summary>
        Task<LeadDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of leads with filtering and paging
        /// </summary>
        Task<PagedResultDto<LeadDto>> GetListAsync(GetLeadListDto input);
        
        /// <summary>
        /// Creates a new lead
        /// </summary>
        Task<LeadDto> CreateAsync(CreateLeadDto input);
        
        /// <summary>
        /// Updates an existing lead
        /// </summary>
        Task<LeadDto> UpdateAsync(Guid id, UpdateLeadDto input);
        
        /// <summary>
        /// Deletes a lead
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Changes the status of a lead
        /// </summary>
        Task<LeadDto> ChangeStatusAsync(Guid id, ChangeLeadStatusDto input);
        
        /// <summary>
        /// Converts a lead to a contact
        /// </summary>
        Task<LeadConversionResultDto> ConvertToContactAsync(Guid id, ConvertLeadDto input);
        
        /// <summary>
        /// Adds tags to a lead
        /// </summary>
        Task<LeadDto> AddTagsAsync(Guid id, LeadTagsDto input);
        
        /// <summary>
        /// Removes tags from a lead
        /// </summary>
        Task<LeadDto> RemoveTagsAsync(Guid id, LeadTagsDto input);
    }
}

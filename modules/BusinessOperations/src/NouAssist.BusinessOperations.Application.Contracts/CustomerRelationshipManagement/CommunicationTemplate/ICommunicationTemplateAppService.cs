using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.CommunicationTemplate.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.CommunicationTemplate
{
    /// <summary>
    /// Application service for communication template management
    /// </summary>
    public interface ICommunicationTemplateAppService : IApplicationService
    {
        /// <summary>
        /// Gets a communication template by ID
        /// </summary>
        Task<CommunicationTemplateDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a communication template by code
        /// </summary>
        Task<CommunicationTemplateDto> GetByCodeAsync(string code);
        
        /// <summary>
        /// Gets a list of communication templates with filtering and paging
        /// </summary>
        Task<PagedResultDto<CommunicationTemplateDto>> GetListAsync(GetCommunicationTemplateListDto input);
        
        /// <summary>
        /// Creates a new communication template
        /// </summary>
        Task<CommunicationTemplateDto> CreateAsync(CreateCommunicationTemplateDto input);
        
        /// <summary>
        /// Updates an existing communication template
        /// </summary>
        Task<CommunicationTemplateDto> UpdateAsync(Guid id, UpdateCommunicationTemplateDto input);
        
        /// <summary>
        /// Deletes a communication template
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Activates a communication template
        /// </summary>
        Task<CommunicationTemplateDto> ActivateAsync(Guid id);
        
        /// <summary>
        /// Deactivates a communication template
        /// </summary>
        Task<CommunicationTemplateDto> DeactivateAsync(Guid id);
        
        /// <summary>
        /// Renders a template with the provided variables
        /// </summary>
        Task<RenderedTemplateDto> RenderAsync(Guid id, RenderTemplateDto input);
        
        /// <summary>
        /// Renders a template by code with the provided variables
        /// </summary>
        Task<RenderedTemplateDto> RenderByCodeAsync(string code, RenderTemplateDto input);
    }
}

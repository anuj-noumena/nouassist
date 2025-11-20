using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations
{
    /// <summary>
    /// Application service for personal automations
    /// </summary>
    public interface IPersonalAutomationAppService : IApplicationService
    {
        /// <summary>
        /// Gets an automation by ID
        /// </summary>
        Task<PersonalAutomationDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets automations for a user
        /// </summary>
        Task<PagedResultDto<PersonalAutomationDto>> GetListAsync(GetAutomationsDto input);
        
        /// <summary>
        /// Creates a new automation
        /// </summary>
        Task<PersonalAutomationDto> CreateAsync(CreateAutomationDto input);
        
        /// <summary>
        /// Updates an automation
        /// </summary>
        Task<PersonalAutomationDto> UpdateAsync(Guid id, UpdateAutomationDto input);
        
        /// <summary>
        /// Deletes an automation
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Adds a trigger to an automation
        /// </summary>
        Task<PersonalAutomationDto> AddTriggerAsync(AddTriggerDto input);
        
        /// <summary>
        /// Removes a trigger from an automation
        /// </summary>
        Task<PersonalAutomationDto> RemoveTriggerAsync(Guid automationId, Guid triggerId);
        
        /// <summary>
        /// Adds an action to an automation
        /// </summary>
        Task<PersonalAutomationDto> AddActionAsync(AddActionDto input);
        
        /// <summary>
        /// Removes an action from an automation
        /// </summary>
        Task<PersonalAutomationDto> RemoveActionAsync(Guid automationId, Guid actionId);
        
        /// <summary>
        /// Changes the order of an action
        /// </summary>
        Task<PersonalAutomationDto> ChangeActionOrderAsync(Guid automationId, Guid actionId, int newOrder);
        
        /// <summary>
        /// Executes an automation manually
        /// </summary>
        Task ExecuteAutomationAsync(Guid automationId);
    }
}

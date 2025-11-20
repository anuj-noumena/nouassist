using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Interaction.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Interaction
{
    /// <summary>
    /// Application service for interaction management
    /// </summary>
    public interface IInteractionAppService : IApplicationService
    {
        /// <summary>
        /// Gets an interaction log by ID
        /// </summary>
        Task<InteractionLogDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of interaction logs with filtering and paging
        /// </summary>
        Task<PagedResultDto<InteractionLogDto>> GetListAsync(GetInteractionLogListDto input);
        
        /// <summary>
        /// Creates a new interaction log
        /// </summary>
        Task<InteractionLogDto> CreateAsync(CreateInteractionLogDto input);
        
        /// <summary>
        /// Updates the summary of an interaction log
        /// </summary>
        Task<InteractionLogDto> UpdateSummaryAsync(Guid id, string summary);
        
        /// <summary>
        /// Deletes an interaction log
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Adds a communication record to an interaction log
        /// </summary>
        Task<CommunicationRecordDto> AddCommunicationRecordAsync(Guid interactionLogId, CreateCommunicationRecordDto input);
        
        /// <summary>
        /// Adds an attachment to a communication record
        /// </summary>
        Task AddAttachmentAsync(Guid interactionLogId, Guid communicationRecordId, AddAttachmentDto input);
        
        /// <summary>
        /// Adds a follow-up task to an interaction log
        /// </summary>
        Task<FollowUpTaskDto> AddFollowUpTaskAsync(Guid interactionLogId, CreateFollowUpTaskDto input);
        
        /// <summary>
        /// Updates a follow-up task
        /// </summary>
        Task<FollowUpTaskDto> UpdateFollowUpTaskAsync(Guid interactionLogId, Guid taskId, UpdateFollowUpTaskDto input);
        
        /// <summary>
        /// Starts a follow-up task
        /// </summary>
        Task<FollowUpTaskDto> StartFollowUpTaskAsync(Guid interactionLogId, Guid taskId);
        
        /// <summary>
        /// Completes a follow-up task
        /// </summary>
        Task<FollowUpTaskDto> CompleteFollowUpTaskAsync(Guid interactionLogId, Guid taskId, CompleteFollowUpTaskDto input);
        
        /// <summary>
        /// Cancels a follow-up task
        /// </summary>
        Task<FollowUpTaskDto> CancelFollowUpTaskAsync(Guid interactionLogId, Guid taskId, CompleteFollowUpTaskDto input);
        
        /// <summary>
        /// Gets interaction logs for a specific entity
        /// </summary>
        Task<PagedResultDto<InteractionLogDto>> GetEntityInteractionsAsync(Guid entityId, string entityType, int skipCount = 0, int maxResultCount = 10, string sorting = null);
    }
}

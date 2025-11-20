namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for managing interactions with leads and contacts
    /// </summary>
    public interface IInteractionManager
    {
        /// <summary>
        /// Creates a new interaction log
        /// </summary>
        Task<InteractionLog> CreateInteractionLogAsync(
            string entityType,
            Guid entityId,
            string summary);
            
        /// <summary>
        /// Updates the summary of an interaction log
        /// </summary>
        Task<InteractionLog> UpdateSummaryAsync(
            Guid id,
            string summary);
            
        /// <summary>
        /// Adds a communication record to an interaction log
        /// </summary>
        Task<CommunicationRecord> AddCommunicationRecordAsync(
            Guid interactionLogId,
            InteractionType type,
            string subject,
            string content,
            DateTime communicationTime,
            bool isInbound,
            string sender,
            string recipient);
            
        /// <summary>
        /// Adds an attachment to a communication record
        /// </summary>
        Task AddAttachmentAsync(
            Guid interactionLogId,
            CommunicationRecord record,
            Guid documentId,
            string fileName,
            string contentType);
            
        /// <summary>
        /// Adds a follow-up task to an interaction log
        /// </summary>
        Task<FollowUpTask> AddFollowUpTaskAsync(
            Guid interactionLogId,
            string title,
            string description,
            DateTime dueDate,
            FollowUpPriority priority,
            Guid? assignedUserId = null);
            
        /// <summary>
        /// Updates a follow-up task
        /// </summary>
        Task<FollowUpTask> UpdateFollowUpTaskAsync(
            Guid interactionLogId,
            FollowUpTask task,
            string title = null,
            string description = null,
            DateTime? dueDate = null,
            FollowUpPriority? priority = null,
            Guid? assignedUserId = null);
            
        /// <summary>
        /// Marks a follow-up task as in progress
        /// </summary>
        Task<FollowUpTask> StartFollowUpTaskAsync(
            Guid interactionLogId,
            FollowUpTask task);
            
        /// <summary>
        /// Completes a follow-up task
        /// </summary>
        Task<FollowUpTask> CompleteFollowUpTaskAsync(
            Guid interactionLogId,
            FollowUpTask task,
            string completionNotes = null);
            
        /// <summary>
        /// Cancels a follow-up task
        /// </summary>
        Task<FollowUpTask> CancelFollowUpTaskAsync(
            Guid interactionLogId,
            FollowUpTask task,
            string cancellationNotes = null);
            
        /// <summary>
        /// Gets interactions for a specific entity
        /// </summary>
        Task<List<InteractionLog>> GetEntityInteractionsAsync(
            Guid entityId,
            string entityType,
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null);
    }
}

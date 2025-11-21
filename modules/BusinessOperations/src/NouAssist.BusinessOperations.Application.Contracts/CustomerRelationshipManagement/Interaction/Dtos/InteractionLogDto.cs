using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Interaction.Dtos
{
    /// <summary>
    /// Data transfer object for InteractionLog entity
    /// </summary>
    public class InteractionLogDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Type of entity this interaction is with
        /// </summary>
        public string EntityType { get; set; }
        
        /// <summary>
        /// ID of the entity this interaction is with (Lead or Contact)
        /// </summary>
        public Guid EntityId { get; set; }
        
        /// <summary>
        /// Entity name if available
        /// </summary>
        public string EntityName { get; set; }
        
        /// <summary>
        /// Summary of the interaction
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// Communication records associated with this interaction
        /// </summary>
        public List<CommunicationRecordDto> CommunicationRecords { get; set; }
        
        /// <summary>
        /// Follow-up tasks associated with this interaction
        /// </summary>
        public List<FollowUpTaskDto> FollowUpTasks { get; set; }
    }

    /// <summary>
    /// Data transfer object for CommunicationRecord entity
    /// </summary>
    public class CommunicationRecordDto : EntityDto
    {
        /// <summary>
        /// Type of communication
        /// </summary>
        public InteractionType Type { get; set; }
        
        /// <summary>
        /// Subject or title of the communication
        /// </summary>
        public string Subject { get; set; }
        
        /// <summary>
        /// Content of the communication
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// When the communication occurred
        /// </summary>
        public DateTime CommunicationTime { get; set; }
        
        /// <summary>
        /// Direction of communication (inbound/outbound)
        /// </summary>
        public bool IsInbound { get; set; }
        
        /// <summary>
        /// Sender information (email, phone, etc.)
        /// </summary>
        public string Sender { get; set; }
        
        /// <summary>
        /// Recipient information (email, phone, etc.)
        /// </summary>
        public string Recipient { get; set; }
        
        /// <summary>
        /// Document attachments
        /// </summary>
        public List<DocumentIdDto> Attachments { get; set; }
    }

    /// <summary>
    /// Data transfer object for DocumentId value object
    /// </summary>
    public class DocumentIdDto
    {
        /// <summary>
        /// The unique identifier of the document
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The original filename of the document
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// MIME type of the document
        /// </summary>
        public string ContentType { get; set; }
    }

    /// <summary>
    /// Data transfer object for FollowUpTask entity
    /// </summary>
    public class FollowUpTaskDto : EntityDto
    {
        /// <summary>
        /// Title of the task
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Due date for the task
        /// </summary>
        public DateTime DueDate { get; set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        public FollowUpPriority Priority { get; set; }
        
        /// <summary>
        /// Current status of the task
        /// </summary>
        public FollowUpStatus Status { get; set; }
        
        /// <summary>
        /// User assigned to the task
        /// </summary>
        public Guid? AssignedUserId { get; set; }
        
        /// <summary>
        /// Assigned user name if available
        /// </summary>
        public string AssignedUserName { get; set; }
        
        /// <summary>
        /// Completion date if the task is completed
        /// </summary>
        public DateTime? CompletionDate { get; set; }
        
        /// <summary>
        /// Notes about task completion
        /// </summary>
        public string CompletionNotes { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating an interaction log
    /// </summary>
    public class CreateInteractionLogDto
    {
        /// <summary>
        /// Type of entity this interaction is with
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }
        
        /// <summary>
        /// ID of the entity this interaction is with (Lead or Contact)
        /// </summary>
        [Required]
        public Guid EntityId { get; set; }
        
        /// <summary>
        /// Summary of the interaction
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Summary { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating a communication record
    /// </summary>
    public class CreateCommunicationRecordDto
    {
        /// <summary>
        /// Type of communication
        /// </summary>
        [Required]
        public InteractionType Type { get; set; }
        
        /// <summary>
        /// Subject or title of the communication
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Subject { get; set; }
        
        /// <summary>
        /// Content of the communication
        /// </summary>
        [StringLength(4000)]
        public string Content { get; set; }
        
        /// <summary>
        /// When the communication occurred
        /// </summary>
        [Required]
        public DateTime CommunicationTime { get; set; }
        
        /// <summary>
        /// Direction of communication (inbound/outbound)
        /// </summary>
        [Required]
        public bool IsInbound { get; set; }
        
        /// <summary>
        /// Sender information (email, phone, etc.)
        /// </summary>
        [StringLength(256)]
        public string Sender { get; set; }
        
        /// <summary>
        /// Recipient information (email, phone, etc.)
        /// </summary>
        [StringLength(256)]
        public string Recipient { get; set; }
    }

    /// <summary>
    /// Data transfer object for adding an attachment
    /// </summary>
    public class AddAttachmentDto
    {
        /// <summary>
        /// The unique identifier of the document
        /// </summary>
        [Required]
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// The original filename of the document
        /// </summary>
        [Required]
        [StringLength(256)]
        public string FileName { get; set; }
        
        /// <summary>
        /// MIME type of the document
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ContentType { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating a follow-up task
    /// </summary>
    public class CreateFollowUpTaskDto
    {
        /// <summary>
        /// Title of the task
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }
        
        /// <summary>
        /// Due date for the task
        /// </summary>
        [Required]
        public DateTime DueDate { get; set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        [Required]
        public FollowUpPriority Priority { get; set; }
        
        /// <summary>
        /// User assigned to the task
        /// </summary>
        public Guid? AssignedUserId { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating a follow-up task
    /// </summary>
    public class UpdateFollowUpTaskDto
    {
        /// <summary>
        /// Title of the task
        /// </summary>
        [StringLength(200)]
        public string Title { get; set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }
        
        /// <summary>
        /// Due date for the task
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        public FollowUpPriority? Priority { get; set; }
        
        /// <summary>
        /// User assigned to the task
        /// </summary>
        public Guid? AssignedUserId { get; set; }
    }

    /// <summary>
    /// Data transfer object for completing a follow-up task
    /// </summary>
    public class CompleteFollowUpTaskDto
    {
        /// <summary>
        /// Notes about task completion
        /// </summary>
        [StringLength(1000)]
        public string CompletionNotes { get; set; }
    }

    /// <summary>
    /// Data transfer object for interaction log list filtering and pagination
    /// </summary>
    public class GetInteractionLogListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to search in interaction summary and content
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by entity type
        /// </summary>
        public string EntityType { get; set; }
        
        /// <summary>
        /// Filter by entity ID
        /// </summary>
        public Guid? EntityId { get; set; }
        
        /// <summary>
        /// Filter by interaction type
        /// </summary>
        public InteractionType? InteractionType { get; set; }
        
        /// <summary>
        /// Filter for interactions with pending follow-ups
        /// </summary>
        public bool? HasPendingFollowUps { get; set; }
        
        /// <summary>
        /// Filter by interaction date range start
        /// </summary>
        public DateTime? From { get; set; }
        
        /// <summary>
        /// Filter by interaction date range end
        /// </summary>
        public DateTime? To { get; set; }
    }
}

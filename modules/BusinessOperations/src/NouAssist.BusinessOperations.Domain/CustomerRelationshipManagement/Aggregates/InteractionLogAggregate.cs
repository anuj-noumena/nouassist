namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates
{
    /// <summary>
    /// Interaction log aggregate root for tracking all communications with leads and contacts
    /// </summary>
    public class InteractionLog : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Type of entity this interaction is with
        /// </summary>
        public string EntityType { get; private set; }
        
        /// <summary>
        /// ID of the entity this interaction is with (Lead or Contact)
        /// </summary>
        public Guid EntityId { get; private set; }
        
        /// <summary>
        /// Summary of the interaction
        /// </summary>
        public string Summary { get; private set; }
        
        /// <summary>
        /// Communication records associated with this interaction
        /// </summary>
        private readonly List<CommunicationRecord> _communicationRecords;
        
        /// <summary>
        /// Read-only access to communication records
        /// </summary>
        public IReadOnlyCollection<CommunicationRecord> CommunicationRecords => _communicationRecords.AsReadOnly();
        
        /// <summary>
        /// Follow-up tasks associated with this interaction
        /// </summary>
        private readonly List<FollowUpTask> _followUpTasks;
        
        /// <summary>
        /// Read-only access to follow-up tasks
        /// </summary>
        public IReadOnlyCollection<FollowUpTask> FollowUpTasks => _followUpTasks.AsReadOnly();

        protected InteractionLog()
        {
            _communicationRecords = new List<CommunicationRecord>();
            _followUpTasks = new List<FollowUpTask>();
        }

        public static InteractionLog Create(
            Guid id,
            string entityType,
            Guid entityId,
            string summary,
            Guid? tenantId = null)
        {
            return new InteractionLog(id, entityType, entityId, summary, tenantId);
        }

        private InteractionLog(
            Guid id,
            string entityType,
            Guid entityId,
            string summary,
            Guid? tenantId = null) : base(id)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = entityId;
            Summary = Check.NotNullOrWhiteSpace(summary, nameof(summary));
            TenantId = tenantId;
            _communicationRecords = new List<CommunicationRecord>();
            _followUpTasks = new List<FollowUpTask>();
        }

        public void UpdateSummary(string summary)
        {
            Summary = Check.NotNullOrWhiteSpace(summary, nameof(summary));
        }

        public CommunicationRecord AddCommunicationRecord(
            InteractionType type,
            string subject,
            string content,
            DateTime communicationTime,
            bool isInbound,
            string sender,
            string recipient)
        {
            var record = new CommunicationRecord(
                Id,
                type,
                subject,
                content,
                communicationTime,
                isInbound,
                sender,
                recipient);
                
            _communicationRecords.Add(record);
            
            AddDistributedEvent(new InteractionLoggedEto
            {
                Id = Id,
                RelatedEntityId = EntityId,
                RelatedEntityType = EntityType,
                InteractionType = type,
                InteractionTime = communicationTime,
                Summary = subject,
                TenantId = TenantId
            });
            
            return record;
        }

        public void AddAttachmentToRecord(
            CommunicationRecord record,
            DocumentId attachment)
        {
            if (!_communicationRecords.Contains(record))
            {
                throw new BusinessException("Communication record is not part of this interaction log");
            }
            
            record.AddAttachment(attachment);
        }

        public FollowUpTask AddFollowUpTask(
            string title,
            string description,
            DateTime dueDate,
            FollowUpPriority priority,
            Guid? assignedUserId = null)
        {
            var task = new FollowUpTask(
                Id,
                title,
                description,
                dueDate,
                priority,
                assignedUserId);
                
            _followUpTasks.Add(task);
            return task;
        }

        public void UpdateFollowUpTask(
            FollowUpTask task,
            string title = null,
            string description = null,
            DateTime? dueDate = null,
            FollowUpPriority? priority = null,
            Guid? assignedUserId = null)
        {
            if (!_followUpTasks.Contains(task))
            {
                throw new BusinessException("Follow-up task is not part of this interaction log");
            }
            
            task.UpdateDetails(title, description, dueDate, priority, assignedUserId);
        }

        public void StartFollowUpTask(FollowUpTask task)
        {
            if (!_followUpTasks.Contains(task))
            {
                throw new BusinessException("Follow-up task is not part of this interaction log");
            }
            
            task.MarkInProgress();
        }

        public void CompleteFollowUpTask(FollowUpTask task, string completionNotes = null)
        {
            if (!_followUpTasks.Contains(task))
            {
                throw new BusinessException("Follow-up task is not part of this interaction log");
            }
            
            task.Complete(completionNotes);
        }

        public void CancelFollowUpTask(FollowUpTask task, string cancellationNotes = null)
        {
            if (!_followUpTasks.Contains(task))
            {
                throw new BusinessException("Follow-up task is not part of this interaction log");
            }
            
            task.Cancel(cancellationNotes);
        }
    }
}

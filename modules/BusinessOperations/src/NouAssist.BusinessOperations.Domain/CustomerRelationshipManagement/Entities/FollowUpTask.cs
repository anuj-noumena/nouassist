namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities
{
    /// <summary>
    /// Represents a follow-up task for a lead or contact
    /// </summary>
    public class FollowUpTask : Entity
    {
        /// <summary>
        /// The interaction log this task is associated with
        /// </summary>
        public Guid InteractionLogId { get; private set; }
        
        /// <summary>
        /// Title of the task
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Due date for the task
        /// </summary>
        public DateTime DueDate { get; private set; }
        
        /// <summary>
        /// Priority of the task
        /// </summary>
        public FollowUpPriority Priority { get; private set; }
        
        /// <summary>
        /// Current status of the task
        /// </summary>
        public FollowUpStatus Status { get; private set; }
        
        /// <summary>
        /// User assigned to the task
        /// </summary>
        public Guid? AssignedUserId { get; private set; }
        
        /// <summary>
        /// Completion date if the task is completed
        /// </summary>
        public DateTime? CompletionDate { get; private set; }
        
        /// <summary>
        /// Notes about task completion
        /// </summary>
        public string CompletionNotes { get; private set; }

        protected FollowUpTask()
        {
        }

        internal FollowUpTask(
            Guid interactionLogId,
            string title,
            string description,
            DateTime dueDate,
            FollowUpPriority priority,
            Guid? assignedUserId)
        {
            InteractionLogId = interactionLogId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            
            if (dueDate <= Clock.Now)
            {
                throw new BusinessException("CRM:ErrorMessage:InvalidFollowUpDate");
            }
            
            DueDate = dueDate;
            Priority = priority;
            Status = FollowUpStatus.Pending;
            AssignedUserId = assignedUserId;
        }

        internal void UpdateDetails(
            string title = null,
            string description = null,
            DateTime? dueDate = null,
            FollowUpPriority? priority = null,
            Guid? assignedUserId = null)
        {
            if (Status == FollowUpStatus.Completed || Status == FollowUpStatus.Cancelled)
            {
                throw new BusinessException("Cannot update a completed or cancelled task");
            }

            if (title != null)
            {
                Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            }

            if (description != null)
            {
                Description = description;
            }

            if (dueDate.HasValue)
            {
                if (dueDate.Value <= Clock.Now)
                {
                    throw new BusinessException("CRM:ErrorMessage:InvalidFollowUpDate");
                }
                
                DueDate = dueDate.Value;
            }

            if (priority.HasValue)
            {
                Priority = priority.Value;
            }

            if (assignedUserId.HasValue || assignedUserId == null)
            {
                AssignedUserId = assignedUserId;
            }
        }

        internal void MarkInProgress()
        {
            if (Status != FollowUpStatus.Pending)
            {
                throw new BusinessException("Only pending tasks can be marked in progress");
            }

            Status = FollowUpStatus.InProgress;
        }

        internal void Complete(string completionNotes = null)
        {
            if (Status == FollowUpStatus.Completed || Status == FollowUpStatus.Cancelled)
            {
                throw new BusinessException("Task is already completed or cancelled");
            }

            Status = FollowUpStatus.Completed;
            CompletionDate = Clock.Now;
            CompletionNotes = completionNotes;
        }

        internal void Cancel(string completionNotes = null)
        {
            if (Status == FollowUpStatus.Completed || Status == FollowUpStatus.Cancelled)
            {
                throw new BusinessException("Task is already completed or cancelled");
            }

            Status = FollowUpStatus.Cancelled;
            CompletionDate = Clock.Now;
            CompletionNotes = completionNotes;
        }

        public override object[] GetKeys()
        {
            return new object[] { InteractionLogId, DueDate };
        }
    }
}

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates
{
    /// <summary>
    /// Aggregate root for discussion boards, managing threads and comments.
    /// </summary>
    public class DiscussionBoard : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public TargetResource Resource { get; private set; }
        public bool IsActive { get; private set; }
        
        private readonly List<DiscussionThread> _threads;
        public IReadOnlyCollection<DiscussionThread> Threads => _threads.AsReadOnly();
        
        protected DiscussionBoard()
        {
            _threads = new List<DiscussionThread>();
        }
        
        /// <summary>
        /// Factory method to create a new DiscussionBoard.
        /// </summary>
        public static DiscussionBoard Create(
            Guid id,
            TargetResource resource,
            Guid? tenantId)
        {
            return new DiscussionBoard(id, resource, tenantId);
        }
        
        private DiscussionBoard(
            Guid id,
            TargetResource resource,
            Guid? tenantId) : base(id)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
            TenantId = tenantId;
            IsActive = true;
            _threads = new List<DiscussionThread>();
        }
        
        /// <summary>
        /// Creates a new discussion thread.
        /// </summary>
        public DiscussionThread CreateThread(
            Guid threadId,
            ThreadSubject subject,
            Guid creatorId,
            CommentContent initialComment,
            DateTime creationTime)
        {
            if (!IsActive)
            {
                throw new CollaborationDomainException(
                    "Collaboration:ErrorMessage:InvalidOperation",
                    "Cannot create threads in an inactive discussion board.");
            }
            
            var thread = new DiscussionThread(
                threadId,
                Id,
                subject,
                creatorId,
                TenantId,
                creationTime);
                
            // Add the initial comment to the thread
            thread.AddComment(
                GuidGenerator.Create(),
                creatorId,
                initialComment,
                creationTime);
                
            _threads.Add(thread);
            
            // Raise domain event for thread creation
            AddDistributedEvent(new DiscussionThreadCreatedEto
            {
                Id = GuidGenerator.Create(),
                TenantId = TenantId,
                EventType = NotificationEventType.CommentAdded,
                ResourceType = Resource.Type,
                ResourceId = Resource.Id.GetValueOrDefault(),
                Timestamp = creationTime,
                InitiatorUserId = creatorId,
                DiscussionBoardId = Id,
                ThreadId = threadId,
                Subject = subject.Value,
                InitialContent = initialComment.Value
            });
            
            return thread;
        }
        
        /// <summary>
        /// Adds a comment to an existing thread.
        /// </summary>
        public Comment AddComment(
            Guid threadId,
            Guid commentId,
            Guid authorId,
            CommentContent content,
            DateTime creationTime)
        {
            if (!IsActive)
            {
                throw new CollaborationDomainException(
                    "Collaboration:ErrorMessage:InvalidOperation",
                    "Cannot add comments in an inactive discussion board.");
            }
            
            var thread = _threads.FirstOrDefault(t => t.Id == threadId);
            if (thread == null)
            {
                throw new EntityNotFoundException(nameof(DiscussionThread), threadId);
            }
            
            var comment = thread.AddComment(commentId, authorId, content, creationTime);
            
            // Raise domain event for comment addition
            AddDistributedEvent(new CommentAddedEto
            {
                Id = GuidGenerator.Create(),
                TenantId = TenantId,
                EventType = NotificationEventType.CommentAdded,
                ResourceType = Resource.Type,
                ResourceId = Resource.Id.GetValueOrDefault(),
                Timestamp = creationTime,
                InitiatorUserId = authorId,
                DiscussionBoardId = Id,
                ThreadId = threadId,
                CommentId = commentId,
                Content = content.Value,
                MentionedUserIds = content.MentionedUserIds
            });
            
            return comment;
        }
        
        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        public void UpdateComment(
            Guid threadId,
            Guid commentId,
            Guid authorId,
            CommentContent newContent,
            DateTime editTime)
        {
            if (!IsActive)
            {
                throw new CollaborationDomainException(
                    "Collaboration:ErrorMessage:InvalidOperation",
                    "Cannot update comments in an inactive discussion board.");
            }
            
            var thread = _threads.FirstOrDefault(t => t.Id == threadId);
            if (thread == null)
            {
                throw new EntityNotFoundException(nameof(DiscussionThread), threadId);
            }
            
            var comment = thread.GetComment(commentId);
            if (comment == null)
            {
                throw new EntityNotFoundException(nameof(Comment), commentId);
            }
            
            // Verify that the user is the author of the comment
            if (comment.AuthorId != authorId)
            {
                throw new PermissionDeniedException(
                    authorId,
                    PermissionType.Write,
                    TargetResourceType.Requirement, // Using requirement as example, actual resource type would be derived
                    Resource.Id.GetValueOrDefault());
            }
            
            comment.UpdateContent(newContent, editTime);
            
            // Raise domain event for comment update
            AddDistributedEvent(new CommentUpdatedEto
            {
                Id = GuidGenerator.Create(),
                TenantId = TenantId,
                CommentId = commentId,
                ThreadId = threadId,
                NewContent = newContent.Value,
                UpdatedByUserId = authorId,
                UpdateTime = editTime
            });
        }
        
        /// <summary>
        /// Closes a discussion thread.
        /// </summary>
        public void CloseThread(Guid threadId, Guid userId, DateTime closedTime)
        {
            var thread = _threads.FirstOrDefault(t => t.Id == threadId);
            if (thread == null)
            {
                throw new EntityNotFoundException(nameof(DiscussionThread), threadId);
            }
            
            thread.Close(closedTime);
            
            // Raise domain event for thread closure
            AddDistributedEvent(new ThreadStatusChangedEto
            {
                Id = GuidGenerator.Create(),
                TenantId = TenantId,
                ThreadId = threadId,
                IsClosed = true,
                ChangedByUserId = userId,
                ChangeTime = closedTime
            });
        }
        
        /// <summary>
        /// Reopens a discussion thread.
        /// </summary>
        public void ReopenThread(Guid threadId, Guid userId, DateTime reopenTime)
        {
            var thread = _threads.FirstOrDefault(t => t.Id == threadId);
            if (thread == null)
            {
                throw new EntityNotFoundException(nameof(DiscussionThread), threadId);
            }
            
            thread.Reopen();
            
            // Raise domain event for thread reopening
            AddDistributedEvent(new ThreadStatusChangedEto
            {
                Id = GuidGenerator.Create(),
                TenantId = TenantId,
                ThreadId = threadId,
                IsClosed = false,
                ChangedByUserId = userId,
                ChangeTime = reopenTime
            });
        }
        
        /// <summary>
        /// Activates the discussion board.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }
        
        /// <summary>
        /// Deactivates the discussion board.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }
        
        /// <summary>
        /// Gets a thread by its ID.
        /// </summary>
        public DiscussionThread GetThread(Guid threadId)
        {
            return _threads.FirstOrDefault(t => t.Id == threadId);
        }
    }
}

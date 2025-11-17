using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates
{
    /// <summary>
    /// TaskCollaborationAggregate - Manages collaboration features for a task instance
    /// Promoted from value object to aggregate for better lifecycle management and scalability
    /// Can be extended to include comments and attachments as child entities in the future
    /// </summary>
    public class TaskCollaborationAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskInstanceId { get; private set; }
        public Guid? TenantId { get; private set; }

        // === WATCHERS ===
        private readonly List<Guid> _watchers = new();
        public IReadOnlyCollection<Guid> Watchers => _watchers.AsReadOnly();

        // === METRICS ===
        public int CommentCount { get; private set; }
        public int AttachmentCount { get; private set; }

        protected TaskCollaborationAggregate() { }

        public TaskCollaborationAggregate(
            Guid id,
            Guid taskInstanceId,
            Guid? tenantId = null) : base(id)
        {
            TaskInstanceId = taskInstanceId;
            TenantId = tenantId;
            CommentCount = 0;
            AttachmentCount = 0;

            AddDistributedEvent(new TaskCollaborationAggregateCreatedEto
            {
                TaskCollaborationAggregateId = Id,
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                CreatedAt = DateTime.UtcNow
            });
        }

        // === WATCHER MANAGEMENT ===

        /// <summary>
        /// Adds a watcher to this task instance
        /// </summary>
        public void AddWatcher(Guid userId)
        {
            if (_watchers.Contains(userId))
            {
                return; // Already watching
            }

            _watchers.Add(userId);

            AddDistributedEvent(new TaskWatcherAddedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                UserId = userId,
                AddedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Removes a watcher from this task instance
        /// </summary>
        public void RemoveWatcher(Guid userId)
        {
            if (!_watchers.Contains(userId))
            {
                return; // Not watching
            }

            _watchers.Remove(userId);

            AddDistributedEvent(new TaskWatcherRemovedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                UserId = userId,
                RemovedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Adds multiple watchers at once
        /// </summary>
        public void AddWatchers(IEnumerable<Guid> userIds)
        {
            foreach (var userId in userIds.Where(id => !_watchers.Contains(id)))
            {
                AddWatcher(userId);
            }
        }

        /// <summary>
        /// Removes all watchers
        /// </summary>
        public void ClearWatchers()
        {
            _watchers.Clear();

            AddDistributedEvent(new TaskWatchersClearedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                ClearedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Checks if a user is watching this task
        /// </summary>
        public bool IsWatching(Guid userId)
        {
            return _watchers.Contains(userId);
        }

        // === COMMENT MANAGEMENT ===

        /// <summary>
        /// Increments the comment count
        /// </summary>
        public void IncrementComments()
        {
            CommentCount++;

            AddDistributedEvent(new TaskCommentAddedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                CommentCount = CommentCount,
                AddedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Decrements the comment count
        /// </summary>
        public void DecrementComments()
        {
            if (CommentCount > 0)
            {
                CommentCount--;
            }
        }

        /// <summary>
        /// Sets the comment count directly (for synchronization)
        /// </summary>
        public void SetCommentCount(int count)
        {
            if (count < 0)
            {
                throw new BusinessException("TaskCollaboration:InvalidCommentCount",
                    "Comment count cannot be negative.");
            }

            CommentCount = count;
        }

        // === ATTACHMENT MANAGEMENT ===

        /// <summary>
        /// Increments the attachment count
        /// </summary>
        public void IncrementAttachments()
        {
            AttachmentCount++;

            AddDistributedEvent(new TaskAttachmentAddedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                AttachmentCount = AttachmentCount,
                AddedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Decrements the attachment count
        /// </summary>
        public void DecrementAttachments()
        {
            if (AttachmentCount > 0)
            {
                AttachmentCount--;
            }
        }

        /// <summary>
        /// Sets the attachment count directly (for synchronization)
        /// </summary>
        public void SetAttachmentCount(int count)
        {
            if (count < 0)
            {
                throw new BusinessException("TaskCollaboration:InvalidAttachmentCount",
                    "Attachment count cannot be negative.");
            }

            AttachmentCount = count;
        }

        // === COMPUTED PROPERTIES ===
        public int WatcherCount => _watchers.Count;
        public bool HasWatchers => _watchers.Any();
        public bool HasComments => CommentCount > 0;
        public bool HasAttachments => AttachmentCount > 0;
        public bool HasActivity => HasComments || HasAttachments;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance
{
    /// <summary>
    /// Manages the collaborative layer of a task: watchers, comments, and attachments.
    /// The state previously in the TaskCollaboration value object is now directly managed by this aggregate.
    /// </summary>
    public class TaskCollaborationAggregate : FullAuditedAggregateRoot<Guid>
    {
        public Guid TaskInstanceId { get; private set; }

        // Properties previously in TaskCollaboration Value Object
        private HashSet<Guid> _watchers;
        public IReadOnlyCollection<Guid> Watchers => _watchers.ToList().AsReadOnly();
        public int CommentCount { get; private set; }
        public int AttachmentCount { get; private set; }

        // Convenience properties
        public int WatcherCount => _watchers.Count;
        public bool HasWatchers => _watchers.Any();

        protected TaskCollaborationAggregate()
        {
            // Initialize state directly in the aggregate
            _watchers = new HashSet<Guid>();
            CommentCount = 0;
            AttachmentCount = 0;
        }

        public TaskCollaborationAggregate(Guid id, Guid taskInstanceId) : base(id)
        {
            TaskInstanceId = taskInstanceId;
            // State is initialized by the protected constructor
        }

        // === Collaboration Methods ===
        // These methods now mutate the aggregate's state directly instead of replacing a value object.

        public void AddWatcher(Guid userId)
        {
            if (_watchers.Add(userId)) // Returns true if the item was added
            {
                AddDistributedEvent(new TaskWatcherAddedEto { TaskCollaborationAggregateId = Id, UserId = userId });
            }
        }

        public void RemoveWatcher(Guid userId)
        {
            if (_watchers.Remove(userId)) // Returns true if the item was removed
            {
                AddDistributedEvent(new TaskWatcherRemovedEto { TaskCollaborationAggregateId = Id, UserId = userId });
            }
        }

        public void IncrementComments()
        {
            CommentCount++;
        }

        public void IncrementAttachments()
        {
            AttachmentCount++;
        }
    }
}
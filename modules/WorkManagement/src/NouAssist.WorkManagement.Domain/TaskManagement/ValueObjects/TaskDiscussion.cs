using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects
{
    public class TaskDiscussion : ValueObject
    {
        public int CommentsCount { get; private set; }
        public int AttachmentsCount { get; private set; }
        private readonly List<Guid> _watchers = new();
        public IReadOnlyCollection<Guid> Watchers => new ReadOnlyCollection<Guid>(_watchers);
        public bool IsDiscussionLocked { get; private set; }

        private TaskDiscussion() { }

        public TaskDiscussion(
            int commentsCount = 0,
            int attachmentsCount = 0,
            IEnumerable<Guid>? watchers = null,
            bool isDiscussionLocked = false)
        {
            if (commentsCount < 0)
            {
                throw new ArgumentException("Comments count cannot be negative.", nameof(commentsCount));
            }

            if (attachmentsCount < 0)
            {
                throw new ArgumentException("Attachments count cannot be negative.", nameof(attachmentsCount));
            }

            CommentsCount = commentsCount;
            AttachmentsCount = attachmentsCount;
            _watchers = watchers?.Distinct().ToList() ?? new List<Guid>();
            IsDiscussionLocked = isDiscussionLocked;
        }

        // ------------------------------
        // Factory and update methods
        // ------------------------------

        public static TaskDiscussion Empty() => new();

        public TaskDiscussion AddComment()
        {
            return new TaskDiscussion(CommentsCount + 1, AttachmentsCount, _watchers, IsDiscussionLocked);
        }

        public TaskDiscussion AddAttachment()
        {
            return new TaskDiscussion(CommentsCount, AttachmentsCount + 1, _watchers, IsDiscussionLocked);
        }

        public TaskDiscussion AddWatcher(Guid userId)
        {
            if (_watchers.Contains(userId))
            {
                return this;
            }

            var updated = _watchers.ToList();
            updated.Add(userId);

            return new TaskDiscussion(CommentsCount, AttachmentsCount, updated, IsDiscussionLocked);
        }

        public TaskDiscussion RemoveWatcher(Guid userId)
        {
            if (!_watchers.Contains(userId))
            {
                return this;
            }

            var updated = _watchers.Where(w => w != userId).ToList();
            return new TaskDiscussion(CommentsCount, AttachmentsCount, updated, IsDiscussionLocked);
        }

        public TaskDiscussion LockDiscussion()
        {
            return new TaskDiscussion(CommentsCount, AttachmentsCount, _watchers, true);
        }

        public TaskDiscussion UnlockDiscussion()
        {
            return new TaskDiscussion(CommentsCount, AttachmentsCount, _watchers, false);
        }
        
        // ------------------------------
        // Utility properties
        // ------------------------------
        public bool HasWatchers => _watchers.Any();
        public bool HasComments => CommentsCount > 0;
        public bool HasAttachments => AttachmentsCount > 0;
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CommentsCount;
            yield return AttachmentsCount;
            yield return IsDiscussionLocked;

            foreach (var watcher in _watchers.OrderBy(x => x))
            {
                yield return watcher;
            }
        }
    }
}

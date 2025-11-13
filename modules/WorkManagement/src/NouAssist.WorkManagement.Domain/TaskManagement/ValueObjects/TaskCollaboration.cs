using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects;

/// <summary>
    /// Simplified collaboration tracking
    /// </summary>
    public class TaskCollaboration : ValueObject
    {
        private HashSet<Guid> _watchers;
        public IReadOnlyCollection<Guid> Watchers => _watchers.ToList().AsReadOnly();
        public int CommentCount { get; private set; }
        public int AttachmentCount { get; private set; }

        private TaskCollaboration()
        {
            _watchers = new HashSet<Guid>();
        }

        public static TaskCollaboration Empty() => new();

        public TaskCollaboration AddWatcher(Guid userId)
        {
            var newWatchers = new HashSet<Guid>(_watchers) { userId };
            return new TaskCollaboration
            {
                _watchers = newWatchers,
                CommentCount = this.CommentCount,
                AttachmentCount = this.AttachmentCount
            };
        }

        public TaskCollaboration RemoveWatcher(Guid userId)
        {
            var newWatchers = new HashSet<Guid>(_watchers);
            newWatchers.Remove(userId);
            
            return new TaskCollaboration
            {
                _watchers = newWatchers,
                CommentCount = this.CommentCount,
                AttachmentCount = this.AttachmentCount
            };
        }

        public TaskCollaboration IncrementComments()
        {
            return new TaskCollaboration
            {
                _watchers = new HashSet<Guid>(_watchers),
                CommentCount = this.CommentCount + 1,
                AttachmentCount = this.AttachmentCount
            };
        }

        public TaskCollaboration IncrementAttachments()
        {
            return new TaskCollaboration
            {
                _watchers = new HashSet<Guid>(_watchers),
                CommentCount = this.CommentCount,
                AttachmentCount = this.AttachmentCount + 1
            };
        }

        public int WatcherCount => _watchers.Count;
        public bool HasWatchers => _watchers.Any();

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var watcher in _watchers.OrderBy(w => w))
            {
                yield return watcher;
            }
            yield return CommentCount;
            yield return AttachmentCount;
        }
    }
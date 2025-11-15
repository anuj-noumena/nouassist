using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Learning.Enums;
using NouAssist.Learning.Events;
using NouAssist.Learning.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Learning.Aggregates
{
    /// <summary>
    /// Aggregate root representing a hierarchical learning topic or folder
    /// </summary>
    public class Topic : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid? ParentId { get; private set; }
        public bool IsFolder { get; private set; }
        public string Category { get; private set; }
        public DifficultyLevel Difficulty { get; private set; }
        public int EstimatedHours { get; private set; }
        public string LearningGoals { get; private set; }
        public int Progress { get; private set; }
        public TopicStatus Status { get; private set; }

        // Reference to LearningSession by ID (aggregate reference pattern)
        public Guid? LearningSessionId { get; private set; }

        private readonly List<string> _tags = new();
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        private readonly List<Topic> _children = new();
        public IReadOnlyCollection<Topic> Children => _children.AsReadOnly();

        protected Topic() { /* ORM */ }

        public Topic(
            Guid id,
            Guid userId,
            string title,
            bool isFolder,
            DifficultyLevel difficulty,
            int estimatedHours,
            Guid? tenantId = null,
            Guid? parentId = null)
            : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            IsFolder = isFolder;
            Difficulty = difficulty;
            EstimatedHours = estimatedHours > 0
                ? estimatedHours
                : throw new BusinessException("Learning:InvalidEstimatedHours");
            TenantId = tenantId;
            ParentId = parentId;
            Status = TopicStatus.Planned;
            Progress = 0;

            AddDistributedEvent(new TopicCreatedEto
            {
                TopicId = Id,
                UserId = UserId,
                TenantId = TenantId,
                Title = Title,
                IsFolder = IsFolder,
                CreatedAt = CreationTime
            });
        }

        public void UpdateTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdateCategory(string category)
        {
            Category = category;
        }

        public void UpdateDifficulty(DifficultyLevel difficulty)
        {
            Difficulty = difficulty;
        }

        public void UpdateEstimatedHours(int hours)
        {
            EstimatedHours = hours > 0
                ? hours
                : throw new BusinessException("Learning:InvalidEstimatedHours");
        }

        public void UpdateLearningGoals(string goals)
        {
            LearningGoals = goals;
        }

        public void UpdateProgress(int progress)
        {
            if (progress < 0 || progress > 100)
            {
                throw new BusinessException("Learning:InvalidProgress");
            }
            Progress = progress;

            // Auto-complete if progress reaches 100%
            if (progress == 100 && Status != TopicStatus.Completed)
            {
                Status = TopicStatus.Completed;
            }
        }

        public void UpdateStatus(TopicStatus status)
        {
            Status = status;
        }

        public void AddTag(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag) && !_tags.Contains(tag))
            {
                _tags.Add(tag);
            }
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
        }

        public void AddChild(Topic childTopic)
        {
            if (!IsFolder)
            {
                throw new BusinessException("Learning:CannotAddChildToNonFolder");
            }

            if (childTopic.ParentId != Id)
            {
                throw new BusinessException("Learning:InvalidParentId",
                    "Child topic's ParentId must match this topic's Id.");
            }

            // Prevent circular references
            if (childTopic.Id == Id || IsDescendantOf(childTopic.Id))
            {
                throw new BusinessException("Learning:CircularReferenceDetected");
            }

            _children.Add(childTopic);
        }

        public void RemoveChild(Guid childId)
        {
            var child = _children.FirstOrDefault(c => c.Id == childId);
            if (child != null)
            {
                _children.Remove(child);
            }
        }

        public void AssociateLearningSession(Guid sessionId)
        {
            if (IsFolder)
            {
                throw new BusinessException("Learning:CannotAssignSessionToFolder",
                    "Learning sessions can only be associated with non-folder topics.");
            }
            LearningSessionId = sessionId;
        }

        public void StartLearning()
        {
            if (Status == TopicStatus.Planned)
            {
                Status = TopicStatus.Active;
            }
        }

        public void Pause()
        {
            if (Status == TopicStatus.Active)
            {
                Status = TopicStatus.Paused;
            }
        }

        public void Resume()
        {
            if (Status == TopicStatus.Paused)
            {
                Status = TopicStatus.Active;
            }
        }

        public void Complete()
        {
            Status = TopicStatus.Completed;
            Progress = 100;
        }

        public void Archive()
        {
            Status = TopicStatus.Archived;
        }

        public TopicMetrics CalculateMetrics(int totalTimeSpentMs)
        {
            var estimatedTimeMs = EstimatedHours * 3600000;
            var remainingTimeMs = Math.Max(0, estimatedTimeMs - totalTimeSpentMs);

            var activeChildren = _children.Count(c => c.Status == TopicStatus.Active);
            var completedChildren = _children.Count(c => c.Status == TopicStatus.Completed);
            var avgProgress = _children.Any()
                ? _children.Average(c => c.Progress)
                : 0;

            return new TopicMetrics(
                totalTimeSpentMs,
                remainingTimeMs,
                Progress,
                activeChildren,
                completedChildren,
                avgProgress
            );
        }

        private bool IsDescendantOf(Guid potentialAncestorId)
        {
            if (!ParentId.HasValue) return false;
            if (ParentId == potentialAncestorId) return true;
            // Note: This would require fetching parent from repository in real scenario
            return false;
        }
    }
}

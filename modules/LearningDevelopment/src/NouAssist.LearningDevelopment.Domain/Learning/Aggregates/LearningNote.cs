using System;
using System.Collections.Generic;
using NouAssist.Learning.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.LearningDevelopment.Learning.Aggregates
{
    /// <summary>
    /// Aggregate root for learning notes (Cornell-style or general)
    /// </summary>
    public class LearningNote : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid TopicId { get; private set; }
        public Guid? TenantId { get; private set; }
        public LearningNoteType Type { get; private set; }

        // Cornell Note Structure
        public string Questions { get; private set; }  // Left column - Cues
        public string Notes { get; private set; }       // Right column - Main content
        public string Summary { get; private set; }     // Bottom - Summary

        // General content for non-Cornell notes
        public string Content { get; private set; }

        private readonly List<string> _keyPoints = new();
        public IReadOnlyCollection<string> KeyPoints => _keyPoints.AsReadOnly();

        private readonly Dictionary<string, object> _metadata = new();
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        protected LearningNote() { /* ORM */ }

        public LearningNote(
            Guid id,
            Guid userId,
            Guid topicId,
            LearningNoteType type,
            Guid? tenantId = null)
            : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            TopicId = Check.NotNull(topicId, nameof(topicId));
            Type = type;
            TenantId = tenantId;
        }

        public void UpdateCornellNote(string questions, string notes, string summary)
        {
            if (Type != LearningNoteType.Cornell)
            {
                throw new BusinessException("Learning:InvalidNoteType",
                    "This method is only valid for Cornell notes.");
            }

            Questions = questions;
            Notes = Check.NotNullOrWhiteSpace(notes, nameof(notes));
            Summary = summary;
        }

        public void UpdateContent(string content)
        {
            Content = Check.NotNullOrWhiteSpace(content, nameof(content));
        }

        public void AddKeyPoint(string keyPoint)
        {
            if (!string.IsNullOrWhiteSpace(keyPoint) && !_keyPoints.Contains(keyPoint))
            {
                _keyPoints.Add(keyPoint);
            }
        }

        public void RemoveKeyPoint(string keyPoint)
        {
            _keyPoints.Remove(keyPoint);
        }

        public void SetMetadata(string key, object value)
        {
            _metadata[key] = value;
        }

        public void RemoveMetadata(string key)
        {
            _metadata.Remove(key);
        }
    }
}

using System;
using System.Collections.Generic;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Aggregates
{
    public class SkillLog : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid SkillId { get; private set; }
        public Guid? TenantId { get; private set; }
        public SkillLogType Type { get; private set; }
        public string Content { get; private set; }
        public int? ProficiencyScore { get; private set; }
        
        private readonly Dictionary<string, object> _metadata = new();
        public IReadOnlyDictionary<string, object> Metadata => _metadata;
        
        private readonly List<string> _tags = new();
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        protected SkillLog() { /* ORM */ }

        public SkillLog(
            Guid id,
            Guid userId,
            Guid skillId,
            SkillLogType type,
            string content,
            Guid? tenantId = null)
            : base(id)
        {
            UserId = userId;
            SkillId = skillId;
            Type = type;
            Content = Check.NotNullOrWhiteSpace(content, nameof(content));
            TenantId = tenantId;
        }

        public void UpdateContent(string content)
        {
            Content = Check.NotNullOrWhiteSpace(content, nameof(content));
        }

        public void SetProficiencyScore(int score)
        {
            if (score < 0 || score > 100)
                throw new BusinessException("SkillDevelopment:InvalidProficiencyScore")
                    .WithData("Score", score);
            
            ProficiencyScore = score;
        }

        public void AddMetadata(string key, object value)
        {
            _metadata[key] = value;
        }

        public void AddTag(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag) && !_tags.Contains(tag))
                _tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
        }
    }
}

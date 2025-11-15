using System;
using System.Collections.Generic;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SkillDevelopment.Aggregates
{
    public class ResourceHistory : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string ResourceId { get; private set; } // External resource identifier
        public string Title { get; private set; }
        public ResourceType Type { get; private set; }
        public string SourceUrl { get; private set; }
        public DateTime AccessedAt { get; private set; }
        public int Progress { get; private set; } // 0-100
        
        private readonly List<Guid> _relatedSkillIds = new();
        public IReadOnlyCollection<Guid> RelatedSkillIds => _relatedSkillIds.AsReadOnly();

        protected ResourceHistory() { /* ORM */ }

        public ResourceHistory(
            Guid id,
            Guid userId,
            string resourceId,
            string title,
            ResourceType type,
            Guid? tenantId = null)
            : base(id)
        {
            UserId = userId;
            ResourceId = Check.NotNullOrWhiteSpace(resourceId, nameof(resourceId));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Type = type;
            TenantId = tenantId;
            AccessedAt = Clock.Now;
            Progress = 0;
        }

        public void UpdateProgress(int progress)
        {
            if (progress < 0 || progress > 100)
                throw new BusinessException("SkillDevelopment:InvalidProgress")
                    .WithData("Progress", progress);
            
            Progress = progress;
            AccessedAt = Clock.Now;
        }

        public void SetSourceUrl(string url)
        {
            SourceUrl = url;
        }

        public void LinkToSkill(Guid skillId)
        {
            if (!_relatedSkillIds.Contains(skillId))
                _relatedSkillIds.Add(skillId);
        }

        public void UnlinkFromSkill(Guid skillId)
        {
            _relatedSkillIds.Remove(skillId);
        }

        public bool IsCompleted() => Progress >= 100;
    }
}

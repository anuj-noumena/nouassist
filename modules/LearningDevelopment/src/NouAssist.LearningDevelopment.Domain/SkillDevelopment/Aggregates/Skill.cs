using System;
using System.Collections.Generic;
using NouAssist.LearningDevelopment.SkillDevelopment.ValueObjects;
using NouAssist.SkillDevelopment.Enums;
using NouAssist.SkillDevelopment.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Aggregates
{
    public class Skill : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid? ParentId { get; private set; }
        public bool IsCategory { get; private set; }
        public string Category { get; private set; }
        public ProficiencyLevel TargetProficiency { get; private set; }
        
        private readonly List<Guid> _prerequisites = new();
        public IReadOnlyCollection<Guid> Prerequisites => _prerequisites.AsReadOnly();
        
        private readonly List<string> _resources = new();
        public IReadOnlyCollection<string> Resources => _resources.AsReadOnly();
        
        public int EstimatedPracticeHours { get; private set; }
        public string DevelopmentGoals { get; private set; }
        public ProficiencyScore CurrentProficiency { get; private set; }
        public SkillStatus Status { get; private set; }
        
        private readonly List<Guid> _subSkillIds = new();
        public IReadOnlyCollection<Guid> SubSkillIds => _subSkillIds.AsReadOnly();

        // Reference to active development session (by ID, not object)
        public Guid? ActiveDevelopmentSessionId { get; private set; }

        // Integration: Reference to related Topic for theory
        public Guid? RelatedTopicId { get; private set; }

        protected Skill() { /* ORM */ }

        public Skill(
            Guid id,
            Guid userId,
            string title,
            ProficiencyLevel targetProficiency,
            int estimatedPracticeHours,
            Guid? tenantId = null,
            Guid? parentId = null,
            bool isCategory = false)
            : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
            TargetProficiency = targetProficiency;
            EstimatedPracticeHours = estimatedPracticeHours > 0 ? estimatedPracticeHours :
                throw new ArgumentException("Estimated practice hours must be positive", nameof(estimatedPracticeHours));
            ParentId = parentId;
            IsCategory = isCategory;
            Status = SkillStatus.NotStarted;
            CurrentProficiency = new ProficiencyScore(0, DateTime.UtcNow, "Initial assessment");
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void SetCategory(string category)
        {
            Category = category;
        }

        public void UpdateDevelopmentGoals(string goals)
        {
            DevelopmentGoals = goals;
        }

        public void AddPrerequisite(Guid skillId)
        {
            if (_prerequisites.Contains(skillId))
            {
                throw new BusinessException("SkillDevelopment:DuplicatePrerequisite");
            }

            _prerequisites.Add(skillId);
        }

        public void RemovePrerequisite(Guid skillId)
        {
            _prerequisites.Remove(skillId);
        }

        public void AddResource(string resource)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentException("Resource cannot be empty", nameof(resource));
            }

            if (!_resources.Contains(resource))
            {
                _resources.Add(resource);
            }
        }

        public void AddSubSkill(Guid subSkillId)
        {
            if (IsCategory == false)
            {
                throw new BusinessException("SkillDevelopment:OnlyCategoriesCanHaveSubSkills");
            }

            if (_subSkillIds.Contains(subSkillId))
            {
                throw new BusinessException("SkillDevelopment:DuplicateSubSkill");
            }

            _subSkillIds.Add(subSkillId);
        }

        public void StartDevelopment(Guid sessionId)
        {
            if (Status != SkillStatus.NotStarted && Status != SkillStatus.Archived)
            {
                throw new BusinessException("SkillDevelopment:CannotStartDevelopment")
                    .WithData("CurrentStatus", Status);
            }

            ActiveDevelopmentSessionId = sessionId;
            Status = SkillStatus.InProgress;
        }

        public void UpdateProficiency(int score, string notes, Guid userId)
        {
            var previousScore = CurrentProficiency.Score;
            CurrentProficiency = new ProficiencyScore(score, DateTime.UtcNow, notes);
            
            // Publish progress event
            AddDistributedEvent(new SkillProgressUpdatedEto
            {
                SkillId = Id,
                UserId = UserId,
                TenantId = TenantId,
                PreviousProficiency = previousScore,
                CurrentProficiency = score,
                UpdatedAt = DateTime.UtcNow
            });

            // Auto-update status based on proficiency
            if (score >= 90 && Status != SkillStatus.Mastered)
            {
                MarkAsMastered(userId);
            }
            else if (score >= 70)
            {
                Status = SkillStatus.Reviewing;
            }
            else if (score >= 40)
            {
                Status = SkillStatus.Practicing;
            }
        }

        public void MarkAsMastered(Guid userId)
        {
            if (CurrentProficiency.Score < 90)
            {
                throw new BusinessException("SkillDevelopment:InsufficientProficiencyForMastery")
                    .WithData("RequiredScore", 90)
                    .WithData("CurrentScore", CurrentProficiency.Score);
            }

            Status = SkillStatus.Mastered;
            
            AddDistributedEvent(new SkillMasteredEto
            {
                SkillId = Id,
                UserId = UserId,
                TenantId = TenantId,
                SkillTitle = Title,
                MasteredAt = DateTime.UtcNow,
                FinalProficiencyScore = CurrentProficiency.Score,
                TotalPracticeHours = EstimatedPracticeHours
            });
        }

        public void Archive()
        {
            if (Status == SkillStatus.Mastered)
            {
                throw new BusinessException("SkillDevelopment:CannotArchiveMasteredSkill");
            }

            Status = SkillStatus.Archived;
            ActiveDevelopmentSessionId = null;
        }

        public bool CanStartDevelopment() =>
            Status == SkillStatus.NotStarted || Status == SkillStatus.Archived;

        public void LinkToTopic(Guid topicId)
        {
            RelatedTopicId = topicId;
        }

        public void UnlinkFromTopic()
        {
            RelatedTopicId = null;
        }
    }
}

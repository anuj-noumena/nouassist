using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Events;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates
{
    /// <summary>
    /// Aggregate root for learning resources
    /// </summary>
    [Table("AppLearningResources")]
    public class LearningResource : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Title { get; private set; }
        public ResourceType ResourceType { get; private set; }
        public string ResourceUrl { get; private set; }
        public LearningModuleDescription Description { get; private set; }
        public List<string> Tags { get; private set; }
        public bool IsPublished { get; private set; }
        public bool IsFeatured { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<UserProgress> _userProgressRecords;
        public IReadOnlyCollection<UserProgress> UserProgressRecords => _userProgressRecords.AsReadOnly();

        protected LearningResource()
        {
            _userProgressRecords = new List<UserProgress>();
            Tags = new List<string>();
        }

        public static LearningResource Create(
            Guid id,
            string title,
            ResourceType resourceType,
            string resourceUrl,
            LearningModuleDescription description,
            List<string> tags = null,
            bool isPublished = false,
            bool isFeatured = false,
            Guid? tenantId = null)
        {
            return new LearningResource(
                id,
                title,
                resourceType,
                resourceUrl,
                description,
                tags,
                isPublished,
                isFeatured,
                tenantId);
        }

        private LearningResource(
            Guid id,
            string title,
            ResourceType resourceType,
            string resourceUrl,
            LearningModuleDescription description,
            List<string> tags = null,
            bool isPublished = false,
            bool isFeatured = false,
            Guid? tenantId = null) : base(id)
        {
            SetTitle(title);
            ResourceType = resourceType;
            SetResourceUrl(resourceUrl);
            Description = description;
            Tags = tags ?? new List<string>();
            IsPublished = isPublished;
            IsFeatured = isFeatured;
            TenantId = tenantId;
            _userProgressRecords = new List<UserProgress>();
        }

        public void UpdateDetails(
            string title,
            ResourceType resourceType,
            string resourceUrl,
            LearningModuleDescription description,
            List<string> tags)
        {
            SetTitle(title);
            ResourceType = resourceType;
            SetResourceUrl(resourceUrl);
            Description = description;
            Tags = tags ?? new List<string>();
        }

        public void Publish()
        {
            IsPublished = true;
        }

        public void Unpublish()
        {
            IsPublished = false;
            IsFeatured = false; // Cannot be featured if not published
        }

        public void SetFeatured(bool isFeatured)
        {
            if (isFeatured && !IsPublished)
            {
                throw new BusinessRuleViolationException("Cannot feature an unpublished resource");
            }

            IsFeatured = isFeatured;
        }

        public UserProgress GetUserProgress(Guid userId)
        {
            return _userProgressRecords.FirstOrDefault(p => p.UserId == userId);
        }

        public UserProgress TrackUserProgress(Guid userId, int progressPercentage, DateTime accessTime)
        {
            var progress = GetUserProgress(userId);
            
            if (progress == null)
            {
                progress = new UserProgress(Id, userId, CompletionStatus.NotStarted, progressPercentage, accessTime, TenantId);
                _userProgressRecords.Add(progress);
            }
            else
            {
                progress.UpdateProgress(progressPercentage, accessTime);
            }

            if (progress.Status == CompletionStatus.Completed)
            {
                AddDistributedEvent(new LearningResourceCompletedEto
                {
                    ResourceId = Id,
                    ResourceTitle = Title,
                    UserId = userId,
                    CompletionTime = progress.CompletedAt.Value,
                    TenantId = TenantId
                });
            }

            return progress;
        }

        public void MarkResourceCompleted(Guid userId, DateTime completionTime)
        {
            var progress = GetUserProgress(userId);
            
            if (progress == null)
            {
                progress = new UserProgress(Id, userId, CompletionStatus.Completed, 100, completionTime, TenantId);
                progress.MarkAsCompleted(completionTime);
                _userProgressRecords.Add(progress);
            }
            else
            {
                progress.MarkAsCompleted(completionTime);
            }

            AddDistributedEvent(new LearningResourceCompletedEto
            {
                ResourceId = Id,
                ResourceTitle = Title,
                UserId = userId,
                CompletionTime = completionTime,
                TenantId = TenantId
            });
        }

        public void MarkResourceAbandoned(Guid userId, DateTime abandonTime)
        {
            var progress = GetUserProgress(userId);
            
            if (progress == null)
            {
                progress = new UserProgress(Id, userId, CompletionStatus.Abandoned, 0, abandonTime, TenantId);
                _userProgressRecords.Add(progress);
            }
            else
            {
                progress.MarkAsAbandoned(abandonTime);
            }
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                KnowledgeInnovationConsts.LearningResource.TitleMinLength,
                KnowledgeInnovationConsts.LearningResource.TitleMaxLength);
        }

        private void SetResourceUrl(string resourceUrl)
        {
            ResourceUrl = Check.NotNullOrWhiteSpace(
                resourceUrl,
                nameof(resourceUrl),
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Url);
        }
    }
}

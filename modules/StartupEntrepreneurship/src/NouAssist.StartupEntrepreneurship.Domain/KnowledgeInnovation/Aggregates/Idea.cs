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
    /// Aggregate root for ideas in the innovation pipeline
    /// </summary>
    [Table("AppIdeas")]
    public class Idea : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public IdeaStatus Status { get; private set; }
        public IdeaSource Source { get; private set; }
        public List<string> Tags { get; private set; }
        public DateTime SubmittedDate { get; private set; }
        public string ImpactStatement { get; private set; }
        public int Priority { get; private set; } // 1-10 scale
        public Guid? TenantId { get; private set; }

        private readonly List<IdeaEvaluation> _evaluations;
        public IReadOnlyCollection<IdeaEvaluation> Evaluations => _evaluations.AsReadOnly();

        protected Idea()
        {
            _evaluations = new List<IdeaEvaluation>();
            Tags = new List<string>();
        }

        public static Idea Create(
            Guid id,
            string title,
            string description,
            IdeaSource source,
            List<string> tags = null,
            string impactStatement = null,
            int priority = 5,
            Guid? tenantId = null)
        {
            return new Idea(
                id,
                title,
                description,
                source,
                tags,
                impactStatement,
                priority,
                tenantId);
        }

        private Idea(
            Guid id,
            string title,
            string description,
            IdeaSource source,
            List<string> tags = null,
            string impactStatement = null,
            int priority = 5,
            Guid? tenantId = null) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            Source = source;
            Tags = tags ?? new List<string>();
            ImpactStatement = impactStatement;
            SetPriority(priority);
            Status = IdeaStatus.Draft;
            SubmittedDate = DateTime.UtcNow;
            TenantId = tenantId;
            _evaluations = new List<IdeaEvaluation>();
        }

        public void UpdateDetails(
            string title,
            string description,
            IdeaSource source,
            List<string> tags,
            string impactStatement,
            int priority)
        {
            // Only allow updates if idea is in Draft or Submitted status
            if (Status != IdeaStatus.Draft && Status != IdeaStatus.Submitted)
            {
                throw new BusinessRuleViolationException(
                    $"Cannot update idea details when in {Status} status");
            }

            SetTitle(title);
            SetDescription(description);
            Source = source;
            Tags = tags ?? new List<string>();
            ImpactStatement = impactStatement;
            SetPriority(priority);
        }

        public void Submit()
        {
            if (Status != IdeaStatus.Draft)
            {
                throw new InvalidStatusTransitionException(
                    "Idea", Status, IdeaStatus.Submitted);
            }

            Status = IdeaStatus.Submitted;
            SubmittedDate = DateTime.UtcNow;

            AddDistributedEvent(new IdeaStatusChangedEto
            {
                Id = Id,
                Title = Title,
                OldStatus = IdeaStatus.Draft,
                NewStatus = IdeaStatus.Submitted,
                TenantId = TenantId,
                StatusChangeTime = SubmittedDate,
                StatusChangedById = LastModifierId ?? CreatorId
            });
        }

        public void StartEvaluation()
        {
            if (Status != IdeaStatus.Submitted)
            {
                throw new InvalidStatusTransitionException(
                    "Idea", Status, IdeaStatus.UnderEvaluation);
            }

            Status = IdeaStatus.UnderEvaluation;

            AddDistributedEvent(new IdeaStatusChangedEto
            {
                Id = Id,
                Title = Title,
                OldStatus = IdeaStatus.Submitted,
                NewStatus = IdeaStatus.UnderEvaluation,
                TenantId = TenantId,
                StatusChangeTime = DateTime.UtcNow,
                StatusChangedById = LastModifierId ?? CreatorId
            });
        }

        public IdeaEvaluation AddEvaluation(
            Guid evaluatorId,
            string evaluatorName,
            string comments,
            bool isApproved,
            DateTime evaluationDate)
        {
            if (Status != IdeaStatus.UnderEvaluation)
            {
                throw new BusinessRuleViolationException(
                    $"Cannot add evaluation when idea is in {Status} status");
            }

            var evaluation = new IdeaEvaluation(
                Id,
                evaluatorId,
                evaluatorName,
                comments,
                isApproved,
                evaluationDate);
            
            _evaluations.Add(evaluation);
            return evaluation;
        }

        public void AddEvaluationScore(Guid evaluatorId, EvaluationScore score)
        {
            var evaluation = _evaluations.FirstOrDefault(e => e.EvaluatorId == evaluatorId);
            if (evaluation == null)
            {
                throw new BusinessRuleViolationException(
                    "Evaluation not found for the specified evaluator");
            }

            evaluation.AddScore(score);
        }

        public void Approve()
        {
            if (Status != IdeaStatus.UnderEvaluation)
            {
                throw new InvalidStatusTransitionException(
                    "Idea", Status, IdeaStatus.Approved);
            }

            // Business rule: At least one evaluation required for approval
            if (!_evaluations.Any())
            {
                throw new BusinessRuleViolationException(
                    "At least one evaluation is required before approval");
            }

            var oldStatus = Status;
            Status = IdeaStatus.Approved;

            AddDistributedEvent(new IdeaStatusChangedEto
            {
                Id = Id,
                Title = Title,
                OldStatus = oldStatus,
                NewStatus = Status,
                TenantId = TenantId,
                StatusChangeTime = DateTime.UtcNow,
                StatusChangedById = LastModifierId ?? CreatorId
            });
        }

        public void Reject()
        {
            if (Status != IdeaStatus.UnderEvaluation)
            {
                throw new InvalidStatusTransitionException(
                    "Idea", Status, IdeaStatus.Rejected);
            }

            // Business rule: At least one evaluation required for rejection
            if (!_evaluations.Any())
            {
                throw new BusinessRuleViolationException(
                    "At least one evaluation is required before rejection");
            }

            var oldStatus = Status;
            Status = IdeaStatus.Rejected;

            AddDistributedEvent(new IdeaStatusChangedEto
            {
                Id = Id,
                Title = Title,
                OldStatus = oldStatus,
                NewStatus = Status,
                TenantId = TenantId,
                StatusChangeTime = DateTime.UtcNow,
                StatusChangedById = LastModifierId ?? CreatorId
            });
        }

        public void StartImplementation()
        {
            if (Status != IdeaStatus.Approved)
            {
                throw new InvalidStatusTransitionException(
                    "Idea", Status, IdeaStatus.InProgress);
            }

            var oldStatus = Status;
            Status = IdeaStatus.InProgress;

            AddDistributedEvent(new IdeaStatusChangedEto
            {
                Id = Id,
                Title = Title,
                OldStatus = oldStatus,
                NewStatus = Status,
                TenantId = TenantId,
                StatusChangeTime = DateTime.UtcNow,
                StatusChangedById = LastModifierId ?? CreatorId
            });
        }

        public void MarkAsImplemented()
        {
            if (Status != IdeaStatus.InProgress)
            {
                throw new InvalidStatusTransitionException(
                    "Idea", Status, IdeaStatus.Implemented);
            }

            var oldStatus = Status;
            Status = IdeaStatus.Implemented;

            AddDistributedEvent(new IdeaStatusChangedEto
            {
                Id = Id,
                Title = Title,
                OldStatus = oldStatus,
                NewStatus = Status,
                TenantId = TenantId,
                StatusChangeTime = DateTime.UtcNow,
                StatusChangedById = LastModifierId ?? CreatorId
            });
        }

        public void Archive()
        {
            // Can archive from any status except archived
            if (Status == IdeaStatus.Archived)
            {
                throw new BusinessRuleViolationException("Idea is already archived");
            }

            var oldStatus = Status;
            Status = IdeaStatus.Archived;

            AddDistributedEvent(new IdeaStatusChangedEto
            {
                Id = Id,
                Title = Title,
                OldStatus = oldStatus,
                NewStatus = Status,
                TenantId = TenantId,
                StatusChangeTime = DateTime.UtcNow,
                StatusChangedById = LastModifierId ?? CreatorId
            });
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                KnowledgeInnovationConsts.Idea.TitleMinLength,
                KnowledgeInnovationConsts.Idea.TitleMaxLength);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: KnowledgeInnovationConsts.Idea.DescriptionMaxLength);
        }

        private void SetPriority(int priority)
        {
            Priority = Check.Range(priority, nameof(priority), 1, 10);
        }
    }
}

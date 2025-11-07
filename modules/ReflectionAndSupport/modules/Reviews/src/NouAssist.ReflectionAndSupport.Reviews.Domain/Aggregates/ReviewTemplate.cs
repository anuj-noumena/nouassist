using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.ReflectionAndSupport.Reviews.Enums;
using NouAssist.ReflectionAndSupport.Reviews.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.ReflectionAndSupport.Reviews.Aggregates
{
    public class ReviewTemplate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public ReviewType ReviewType { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public bool IsSystem { get; private set; }
        public Guid? OwnerId { get; private set; }
        public string RecommendedFrequency { get; set; }
        public int? EstimatedMinutes { get; set; }
        public Guid? TenantId { get; private set; }

        private readonly List<ReviewChecklistStep> _checklistItems = new();
        public IReadOnlyCollection<ReviewChecklistStep> ChecklistItems => _checklistItems.AsReadOnly();

        private readonly List<string> _reflectionPrompts = new();
        public IReadOnlyCollection<string> ReflectionPrompts => _reflectionPrompts.AsReadOnly();

        protected ReviewTemplate() { /* ORM */ }

        public ReviewTemplate(
            Guid id,
            ReviewType reviewType,
            string title,
            Guid? ownerId = null,
            Guid? tenantId = null
        ) : base(id)
        {
            ReviewType = reviewType;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
            OwnerId = ownerId;
            IsSystem = ownerId == null; // System templates have no owner
            TenantId = tenantId;
        }

        // Computed Properties
        public bool IsCustomTemplate => !IsSystem;
        public bool HasChecklist => _checklistItems.Any();
        public bool HasReflectionPrompts => _reflectionPrompts.Any();
        public int TotalChecklistItems => _checklistItems.Sum(s => s.ItemCount);

        // Checklist Management
        public void AddChecklistStep(ReviewChecklistStep step)
        {
            if (_checklistItems.Count >= 50)
            {
                throw new BusinessException("Reviews:MaxChecklistSteps", 
                    "Maximum 50 checklist steps allowed");
            }

            if (_checklistItems.Any(s => s.Step == step.Step))
            {
                throw new BusinessException("Reviews:DuplicateStep", 
                    $"Step {step.Step} already exists");
            }

            _checklistItems.Add(step);
        }

        public void RemoveChecklistStep(int stepNumber)
        {
            var step = _checklistItems.FirstOrDefault(s => s.Step == stepNumber);
            if (step == null)
            {
                throw new BusinessException("Reviews:StepNotFound", 
                    "Checklist step not found");
            }

            _checklistItems.Remove(step);
        }

        public void ClearChecklist()
        {
            _checklistItems.Clear();
        }

        // Reflection Prompts Management
        public void AddReflectionPrompt(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                throw new BusinessException("Reviews:EmptyPrompt", 
                    "Reflection prompt cannot be empty");
            }

            if (_reflectionPrompts.Count >= 20)
            {
                throw new BusinessException("Reviews:MaxPromptsReached", 
                    "Maximum 20 reflection prompts allowed");
            }

            if (_reflectionPrompts.Contains(prompt, StringComparer.OrdinalIgnoreCase))
            {
                return; // Already exists
            }

            _reflectionPrompts.Add(prompt);
        }

        public void RemoveReflectionPrompt(string prompt)
        {
            _reflectionPrompts.Remove(prompt);
        }

        public void ClearReflectionPrompts()
        {
            _reflectionPrompts.Clear();
        }

        // Template Information
        public void UpdateTitle(string newTitle)
        {
            Title = Check.NotNullOrWhiteSpace(newTitle, nameof(newTitle), maxLength: 200);
        }

        public void UpdateEstimatedTime(int minutes)
        {
            if (minutes < 0 || minutes > 1440)
            {
                throw new BusinessException("Reviews:InvalidEstimatedTime", 
                    "Estimated time must be between 0 and 1440 minutes");
            }
            EstimatedMinutes = minutes;
        }
    }
}

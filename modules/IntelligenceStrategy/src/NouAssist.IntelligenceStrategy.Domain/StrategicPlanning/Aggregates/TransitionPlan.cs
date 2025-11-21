using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates
{
    /// <summary>
    /// Transition Plan aggregate root.
    /// Manages business exit scenarios, personal evolution roadmaps, and capability development.
    /// </summary>
    public class TransitionPlan : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string PlanName { get; private set; }
        public string Description { get; private set; }
        public TransitionType Type { get; private set; }
        public DateTime PlannedStartDate { get; private set; }
        public DateTime EstimatedCompletionDate { get; private set; }
        public string CurrentPhase { get; private set; }
        public decimal EstimatedCost { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<CapabilityRoadmapItem> _capabilityRoadmap;
        public IReadOnlyCollection<CapabilityRoadmapItem> CapabilityRoadmap => _capabilityRoadmap.AsReadOnly();

        private readonly List<string> _milestones;
        public IReadOnlyCollection<string> Milestones => _milestones.AsReadOnly();

        protected TransitionPlan()
        {
            _capabilityRoadmap = new List<CapabilityRoadmapItem>();
            _milestones = new List<string>();
        }

        /// <summary>
        /// Factory method to create a new transition plan
        /// </summary>
        public static TransitionPlan Create(
            Guid id,
            string planName,
            string description,
            TransitionType type,
            DateTime plannedStartDate,
            DateTime estimatedCompletionDate,
            decimal estimatedCost = 0,
            Guid? tenantId = null)
        {
            return new TransitionPlan(id, planName, description, type, plannedStartDate, estimatedCompletionDate, estimatedCost, tenantId);
        }

        private TransitionPlan(
            Guid id,
            string planName,
            string description,
            TransitionType type,
            DateTime plannedStartDate,
            DateTime estimatedCompletionDate,
            decimal estimatedCost,
            Guid? tenantId) : base(id)
        {
            SetPlanName(planName);
            SetDescription(description);
            Type = type;
            SetTransitionPeriod(plannedStartDate, estimatedCompletionDate);
            EstimatedCost = estimatedCost;
            TenantId = tenantId;
            CurrentPhase = "Planning";
            IsActive = true;
            _capabilityRoadmap = new List<CapabilityRoadmapItem>();
            _milestones = new List<string>();
        }

        /// <summary>
        /// Adds a capability development item to the roadmap
        /// </summary>
        public void AddCapabilityRoadmapItem(
            string skillName,
            string skillCategory,
            CapabilityLevel currentLevel,
            CapabilityLevel targetLevel,
            DateTime targetDate,
            decimal investmentRequired = 0)
        {
            var skill = new SkillName(skillName, skillCategory);
            var roadmapItem = new CapabilityRoadmapItem(
                Guid.NewGuid(),
                Id,
                skill,
                currentLevel,
                targetLevel,
                targetDate,
                investmentRequired);

            _capabilityRoadmap.Add(roadmapItem);
        }

        /// <summary>
        /// Updates progress for a capability roadmap item
        /// </summary>
        public void UpdateCapabilityProgress(Guid roadmapItemId, CapabilityLevel newLevel)
        {
            var item = _capabilityRoadmap.FirstOrDefault(r => r.Id == roadmapItemId);
            if (item == null)
                throw new EntityNotFoundException(typeof(CapabilityRoadmapItem), roadmapItemId);

            item.UpdateProgress(newLevel);
        }

        /// <summary>
        /// Adds a milestone to the transition plan
        /// </summary>
        public void AddMilestone(string milestoneDescription)
        {
            if (string.IsNullOrWhiteSpace(milestoneDescription))
                throw new ArgumentException("Milestone description cannot be empty");

            if (milestoneDescription.Length > 500)
                throw new ArgumentException("Milestone description is too long");

            _milestones.Add(milestoneDescription.Trim());
        }

        /// <summary>
        /// Updates the current phase of the transition
        /// </summary>
        public void UpdateCurrentPhase(string newPhase)
        {
            if (string.IsNullOrWhiteSpace(newPhase))
                throw new ArgumentException("Phase cannot be empty");

            CurrentPhase = newPhase.Trim();
        }

        /// <summary>
        /// Calculates overall transition progress
        /// </summary>
        public decimal CalculateOverallProgress()
        {
            if (!_capabilityRoadmap.Any()) return 0;

            var totalProgress = _capabilityRoadmap.Sum(r => r.ProgressPercentage);
            return totalProgress / _capabilityRoadmap.Count;
        }

        /// <summary>
        /// Gets capability items that are behind schedule
        /// </summary>
        public IEnumerable<CapabilityRoadmapItem> GetItemsBehindSchedule()
        {
            var currentDate = DateTime.UtcNow;
            return _capabilityRoadmap.Where(r => 
                r.TargetDate < currentDate && !r.IsCompleted);
        }

        /// <summary>
        /// Estimates remaining cost based on incomplete capabilities
        /// </summary>
        public decimal EstimateRemainingCost()
        {
            return _capabilityRoadmap
                .Where(r => !r.IsCompleted)
                .Sum(r => r.InvestmentRequired);
        }

        /// <summary>
        /// Activates the transition plan
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            if (CurrentPhase == "Planning")
                CurrentPhase = "In Progress";
        }

        /// <summary>
        /// Completes the transition plan
        /// </summary>
        public void Complete()
        {
            IsActive = false;
            CurrentPhase = "Completed";
        }

        /// <summary>
        /// Pauses the transition plan
        /// </summary>
        public void Pause()
        {
            CurrentPhase = "Paused";
        }

        private void SetPlanName(string planName)
        {
            PlanName = Check.NotNullOrWhiteSpace(planName, nameof(planName), maxLength: 150);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 2000);
        }

        private void SetTransitionPeriod(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
                throw new BusinessException("Transition completion date must be after start date");

            PlannedStartDate = startDate;
            EstimatedCompletionDate = endDate;
        }
    }
}

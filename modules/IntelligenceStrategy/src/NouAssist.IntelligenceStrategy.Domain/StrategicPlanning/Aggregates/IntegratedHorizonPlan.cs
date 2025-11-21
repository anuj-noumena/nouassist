using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates
{
    /// <summary>
    /// Integrated Horizon Plan aggregate root.
    /// Manages timeline, backward planning, future self dialogues, and legacy design.
    /// </summary>
    public class IntegratedHorizonPlan : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string PlanName { get; private set; }
        public string Description { get; private set; }
        public DateTime PlanStartDate { get; private set; }
        public DateTime PlanEndDate { get; private set; }
        public PlanningHorizon PrimaryHorizon { get; private set; }
        public string VisionStatement { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<TimelineEventMarker> _timelineEvents;
        public IReadOnlyCollection<TimelineEventMarker> TimelineEvents => _timelineEvents.AsReadOnly();

        private readonly List<string> _futureSelfDialogues;
        public IReadOnlyCollection<string> FutureSelfDialogues => _futureSelfDialogues.AsReadOnly();

        protected IntegratedHorizonPlan()
        {
            _timelineEvents = new List<TimelineEventMarker>();
            _futureSelfDialogues = new List<string>();
        }

        /// <summary>
        /// Factory method to create a new integrated horizon plan
        /// </summary>
        public static IntegratedHorizonPlan Create(
            Guid id,
            string planName,
            string description,
            DateTime startDate,
            DateTime endDate,
            PlanningHorizon primaryHorizon,
            string visionStatement,
            Guid? tenantId = null)
        {
            return new IntegratedHorizonPlan(id, planName, description, startDate, endDate, primaryHorizon, visionStatement, tenantId);
        }

        private IntegratedHorizonPlan(
            Guid id,
            string planName,
            string description,
            DateTime startDate,
            DateTime endDate,
            PlanningHorizon primaryHorizon,
            string visionStatement,
            Guid? tenantId) : base(id)
        {
            SetPlanName(planName);
            SetDescription(description);
            SetPlanPeriod(startDate, endDate);
            PrimaryHorizon = primaryHorizon;
            SetVisionStatement(visionStatement);
            TenantId = tenantId;
            IsActive = true;
            _timelineEvents = new List<TimelineEventMarker>();
            _futureSelfDialogues = new List<string>();
        }

        /// <summary>
        /// Adds a timeline event marker
        /// </summary>
        public void AddTimelineEvent(
            DateTime eventDate,
            string title,
            string description,
            string eventType,
            bool isCriticalPath = false)
        {
            var timelineDate = new TimelineDate(eventDate, DetermineHorizon(eventDate), title);
            var eventMarker = new TimelineEventMarker(
                Guid.NewGuid(),
                Id,
                timelineDate,
                title,
                description,
                eventType,
                isCriticalPath);

            _timelineEvents.Add(eventMarker);
        }

        /// <summary>
        /// Links a timeline event to a strategic goal
        /// </summary>
        public void LinkEventToGoal(Guid eventId, Guid goalId)
        {
            var eventMarker = _timelineEvents.FirstOrDefault(e => e.Id == eventId);
            if (eventMarker == null)
                throw new EntityNotFoundException(typeof(TimelineEventMarker), eventId);

            eventMarker.LinkToGoal(goalId);
        }

        /// <summary>
        /// Adds a future self dialogue entry
        /// </summary>
        public void AddFutureSelfDialogue(string dialogue)
        {
            if (string.IsNullOrWhiteSpace(dialogue))
                throw new ArgumentException("Future self dialogue cannot be empty");

            if (dialogue.Length > 5000)
                throw new ArgumentException("Future self dialogue is too long");

            _futureSelfDialogues.Add(dialogue.Trim());
        }

        /// <summary>
        /// Performs backward planning from end date
        /// </summary>
        public List<TimelineEventMarker> GenerateBackwardPlan()
        {
            var criticalEvents = _timelineEvents
                .Where(e => e.IsCriticalPath)
                .OrderByDescending(e => e.EventDate.Date)
                .ToList();

            // Logic to generate backward planning milestones
            var backwardPlan = new List<TimelineEventMarker>();
            var currentDate = PlanEndDate;

            foreach (var criticalEvent in criticalEvents)
            {
                // Calculate dependencies and create intermediate milestones
                var daysFromEnd = (currentDate - criticalEvent.EventDate.Date).Days;
                if (daysFromEnd > 30) // Create intermediate milestones for long gaps
                {
                    var intermediateDate = criticalEvent.EventDate.Date.AddDays(daysFromEnd / 2);
                    var intermediateEvent = new TimelineEventMarker(
                        Guid.NewGuid(),
                        Id,
                        new TimelineDate(intermediateDate, DetermineHorizon(intermediateDate), "Checkpoint"),
                        $"Checkpoint for {criticalEvent.Title}",
                        "Backward planning checkpoint",
                        "Checkpoint",
                        true);
                    
                    backwardPlan.Add(intermediateEvent);
                }
                
                currentDate = criticalEvent.EventDate.Date;
            }

            return backwardPlan;
        }

        /// <summary>
        /// Gets events within a specific horizon
        /// </summary>
        public IEnumerable<TimelineEventMarker> GetEventsByHorizon(PlanningHorizon horizon)
        {
            return _timelineEvents.Where(e => e.EventDate.Horizon == horizon);
        }

        /// <summary>
        /// Updates the vision statement
        /// </summary>
        public void UpdateVision(string newVisionStatement)
        {
            SetVisionStatement(newVisionStatement);
        }

        /// <summary>
        /// Activates the plan
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates the plan
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        private PlanningHorizon DetermineHorizon(DateTime date)
        {
            var monthsFromNow = (date.Year - DateTime.UtcNow.Year) * 12 + date.Month - DateTime.UtcNow.Month;
            
            return monthsFromNow switch
            {
                <= 24 => PlanningHorizon.H1,
                <= 60 => PlanningHorizon.H2,
                _ => PlanningHorizon.H3
            };
        }

        private void SetPlanName(string planName)
        {
            PlanName = Check.NotNullOrWhiteSpace(planName, nameof(planName), maxLength: 150);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 2000);
        }

        private void SetVisionStatement(string visionStatement)
        {
            VisionStatement = Check.NotNullOrWhiteSpace(visionStatement, nameof(visionStatement), maxLength: 1000);
        }

        private void SetPlanPeriod(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
                throw new BusinessException("Plan end date must be after start date");

            PlanStartDate = startDate;
            PlanEndDate = endDate;
        }
    }
}

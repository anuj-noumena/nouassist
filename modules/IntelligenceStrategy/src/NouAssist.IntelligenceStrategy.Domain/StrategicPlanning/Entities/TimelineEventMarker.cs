using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Entities
{
    /// <summary>
    /// Represents a significant event or milestone on the integrated timeline
    /// </summary>
    public class TimelineEventMarker : Entity<Guid>
    {
        public Guid IntegratedHorizonPlanId { get; private set; }
        public TimelineDate EventDate { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string EventType { get; private set; }
        public List<Guid> RelatedGoalIds { get; private set; }
        public bool IsCriticalPath { get; private set; }
        public string Dependencies { get; private set; }

        protected TimelineEventMarker()
        {
            RelatedGoalIds = new List<Guid>();
        }

        internal TimelineEventMarker(
            Guid id,
            Guid integratedHorizonPlanId,
            TimelineDate eventDate,
            string title,
            string description,
            string eventType,
            bool isCriticalPath = false) : base(id)
        {
            IntegratedHorizonPlanId = integratedHorizonPlanId;
            EventDate = eventDate;
            SetTitle(title);
            SetDescription(description);
            SetEventType(eventType);
            IsCriticalPath = isCriticalPath;
            RelatedGoalIds = new List<Guid>();
        }

        internal void LinkToGoal(Guid goalId)
        {
            if (!RelatedGoalIds.Contains(goalId))
                RelatedGoalIds.Add(goalId);
        }

        internal void SetDependencies(string dependencies)
        {
            Dependencies = dependencies;
        }

        internal void MarkAsCriticalPath()
        {
            IsCriticalPath = true;
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 1000);
        }

        private void SetEventType(string eventType)
        {
            EventType = Check.NotNullOrWhiteSpace(eventType, nameof(eventType), maxLength: 50);
        }
    }
}

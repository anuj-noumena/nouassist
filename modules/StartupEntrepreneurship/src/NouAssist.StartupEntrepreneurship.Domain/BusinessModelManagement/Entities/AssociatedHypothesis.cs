using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities
{
    /// <summary>
    /// Represents a hypothesis associated with a canvas component
    /// </summary>
    public class AssociatedHypothesis : Entity<Guid>
    {
        public Guid CanvasId { get; private set; }
        public Guid ComponentId { get; private set; }
        public CanvasComponentType ComponentType { get; private set; }
        public HypothesisDetails Details { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid CreatedBy { get; private set; }

        protected AssociatedHypothesis() { }

        internal AssociatedHypothesis(
            Guid id,
            Guid canvasId,
            Guid componentId,
            CanvasComponentType componentType,
            string assumption,
            Guid createdBy,
            DateTime createdAt) : base(id)
        {
            CanvasId = canvasId;
            ComponentId = componentId;
            ComponentType = componentType;
            Details = new HypothesisDetails(assumption);
            CreatedBy = createdBy;
            CreatedAt = createdAt;
        }

        public void StartTest(string testDescription, DateTime testStartDate)
        {
            Details = Details.StartTest(testDescription, testStartDate);
        }

        public void CompleteTest(string testResult, ValidationStatus finalStatus, DateTime testEndDate)
        {
            Details = Details.CompleteTest(testResult, finalStatus, testEndDate);
        }
    }
}

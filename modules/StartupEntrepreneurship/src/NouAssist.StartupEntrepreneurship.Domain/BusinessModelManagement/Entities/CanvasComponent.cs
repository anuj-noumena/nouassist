using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities
{
    /// <summary>
    /// Represents a component of a business model canvas
    /// </summary>
    public class CanvasComponent : Entity<Guid>
    {
        public Guid CanvasId { get; private set; }
        public Guid VersionId { get; private set; }
        public CanvasComponentType ComponentType { get; private set; }
        public ComponentContent Content { get; private set; }
        public ValidationState ValidationState { get; private set; }
        public DateTime LastModified { get; private set; }
        public Guid LastModifiedBy { get; private set; }

        private readonly List<AssociatedHypothesis> _hypotheses;
        public IReadOnlyCollection<AssociatedHypothesis> Hypotheses => _hypotheses.AsReadOnly();

        protected CanvasComponent()
        {
            _hypotheses = new List<AssociatedHypothesis>();
        }

        internal CanvasComponent(
            Guid id,
            Guid canvasId,
            Guid versionId,
            CanvasComponentType componentType,
            ComponentContent content,
            Guid lastModifiedBy,
            DateTime lastModified) : base(id)
        {
            CanvasId = canvasId;
            VersionId = versionId;
            ComponentType = componentType;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            ValidationState = new ValidationState(ValidationStatus.NotTested);
            LastModified = lastModified;
            LastModifiedBy = lastModifiedBy;
            _hypotheses = new List<AssociatedHypothesis>();
        }

        public void UpdateContent(ComponentContent newContent, Guid modifiedBy, DateTime modifiedTime)
        {
            Content = newContent ?? throw new ArgumentNullException(nameof(newContent));
            LastModified = modifiedTime;
            LastModifiedBy = modifiedBy;
        }

        public void UpdateValidationState(ValidationState newValidationState)
        {
            ValidationState = newValidationState ?? throw new ArgumentNullException(nameof(newValidationState));
        }

        public AssociatedHypothesis AddHypothesis(string assumption, Guid createdBy, DateTime createdTime)
        {
            var hypothesis = new AssociatedHypothesis(
                Guid.NewGuid(),
                CanvasId,
                Id,
                ComponentType,
                assumption,
                createdBy,
                createdTime
            );

            _hypotheses.Add(hypothesis);
            return hypothesis;
        }

        public void RemoveHypothesis(Guid hypothesisId)
        {
            var hypothesis = _hypotheses.FirstOrDefault(h => h.Id == hypothesisId);
            if (hypothesis != null)
            {
                _hypotheses.Remove(hypothesis);
            }
        }
    }
}

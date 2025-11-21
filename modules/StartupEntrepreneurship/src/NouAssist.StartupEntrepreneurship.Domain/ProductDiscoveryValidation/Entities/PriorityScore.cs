using System;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities
{
    /// <summary>
    /// Entity representing a prioritization score for a feature
    /// </summary>
    public class PriorityScore : Entity<Guid>
    {
        public Guid FeatureId { get; private set; }
        public PriorityInput Input { get; private set; }
        public FeaturePriority Priority { get; private set; }
        public decimal CalculatedScore { get; private set; }
        public DateTime CalculatedAt { get; private set; }
        public string CalculatedBy { get; private set; }
        public string Notes { get; private set; }

        protected PriorityScore() { }

        internal PriorityScore(
            Guid id,
            Guid featureId,
            PriorityInput input,
            DateTime calculatedAt,
            string calculatedBy) : base(id)
        {
            FeatureId = featureId;
            Input = input ?? throw new ArgumentNullException(nameof(input));
            CalculatedScore = input.CalculatePriorityScore();
            Priority = input.DeterminePriority();
            CalculatedAt = calculatedAt;
            CalculatedBy = Check.NotNullOrWhiteSpace(calculatedBy, nameof(calculatedBy));
        }

        internal void UpdateInput(PriorityInput input)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            CalculatedScore = input.CalculatePriorityScore();
            Priority = input.DeterminePriority();
        }

        internal void UpdateNotes(string notes)
        {
            Notes = notes;
        }
    }
}

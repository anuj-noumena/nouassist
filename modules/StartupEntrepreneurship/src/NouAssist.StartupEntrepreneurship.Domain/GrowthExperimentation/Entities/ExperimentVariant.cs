using System;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities
{
    /// <summary>
    /// Entity that represents a variant in an experiment
    /// </summary>
    public class ExperimentVariant : Entity<Guid>
    {
        /// <summary>
        /// The experiment ID this variant belongs to
        /// </summary>
        public Guid ExperimentId { get; private set; }
        
        /// <summary>
        /// Variant name
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Variant description
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Variant type (control or treatment)
        /// </summary>
        public VariantType Type { get; private set; }
        
        /// <summary>
        /// Setup configuration for this variant
        /// </summary>
        public VariantSetup Setup { get; private set; }
        
        protected ExperimentVariant() { }
        
        internal ExperimentVariant(
            Guid id,
            Guid experimentId, 
            string name, 
            string description, 
            VariantType type, 
            VariantSetup setup)
            : base(id)
        {
            ExperimentId = experimentId;
            SetName(name);
            Description = description;
            Type = type;
            Setup = setup;
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: GrowthExperimentationConstants.Experiments.MaxVariantNameLength
            );
        }
        
        internal void UpdateSetup(VariantSetup setup)
        {
            Setup = setup;
        }
    }
}

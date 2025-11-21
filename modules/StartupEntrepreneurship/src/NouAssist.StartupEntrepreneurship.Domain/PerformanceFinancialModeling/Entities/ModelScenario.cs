using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities
{
    /// <summary>
    /// Represents a scenario within a financial model
    /// </summary>
    public class ModelScenario : Entity<Guid>
    {
        public Guid ModelId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ScenarioType Type { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<Guid> _associatedAssumptionSets;
        public IReadOnlyCollection<Guid> AssociatedAssumptionSets => _associatedAssumptionSets.AsReadOnly();

        protected ModelScenario() 
        {
            _associatedAssumptionSets = new List<Guid>();
        }

        internal ModelScenario(Guid modelId, string name, ScenarioType type, string description = null) : base(Guid.NewGuid())
        {
            ModelId = modelId;
            SetName(name);
            Type = type;
            SetDescription(description);
            IsActive = true;
            _associatedAssumptionSets = new List<Guid>();
        }

        public void UpdateInfo(string name, string description)
        {
            SetName(name);
            SetDescription(description);
        }

        public void AssociateAssumptionSet(Guid assumptionSetId)
        {
            if (!_associatedAssumptionSets.Contains(assumptionSetId))
            {
                _associatedAssumptionSets.Add(assumptionSetId);
            }
        }

        public void DisassociateAssumptionSet(Guid assumptionSetId)
        {
            _associatedAssumptionSets.Remove(assumptionSetId);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: PerformanceFinancialModelingConsts.MaxScenarioNameLength
            );
        }

        private void SetDescription(string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                Description = Check.Length(
                    description.Trim(),
                    nameof(description),
                    maxLength: PerformanceFinancialModelingConsts.MaxScenarioDescriptionLength
                );
            }
        }
    }
}

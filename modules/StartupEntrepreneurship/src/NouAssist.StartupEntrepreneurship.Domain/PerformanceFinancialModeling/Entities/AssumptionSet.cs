using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities
{
    /// <summary>
    /// Represents a set of assumptions used in financial modeling
    /// </summary>
    public class AssumptionSet : Entity<Guid>
    {
        public Guid ModelId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<AssumptionValue> _assumptions;
        public IReadOnlyCollection<AssumptionValue> Assumptions => _assumptions.AsReadOnly();

        protected AssumptionSet() 
        {
            _assumptions = new List<AssumptionValue>();
        }

        internal AssumptionSet(Guid modelId, string name, string description = null) : base(Guid.NewGuid())
        {
            ModelId = modelId;
            SetName(name);
            SetDescription(description);
            IsActive = true;
            _assumptions = new List<AssumptionValue>();
        }

        public void UpdateInfo(string name, string description)
        {
            SetName(name);
            SetDescription(description);
        }

        public void AddAssumption(AssumptionValue assumption)
        {
            if (assumption == null) throw new ArgumentNullException(nameof(assumption));

            // Remove existing assumption with the same name
            _assumptions.RemoveAll(a => a.Name.Equals(assumption.Name, StringComparison.OrdinalIgnoreCase));
            _assumptions.Add(assumption);
        }

        public void RemoveAssumption(string name)
        {
            _assumptions.RemoveAll(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public AssumptionValue GetAssumption(string name)
        {
            return _assumptions.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void UpdateAssumption(string name, decimal newValue)
        {
            var assumption = GetAssumption(name);
            if (assumption == null)
            {
                throw new BusinessException("PerformanceFinancialModeling:ErrorMessage:AssumptionNotFound")
                    .WithData("assumptionName", name);
            }

            var updatedAssumption = assumption.WithValue(newValue);
            _assumptions.RemoveAll(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            _assumptions.Add(updatedAssumption);
        }

        public Dictionary<string, decimal> GetAssumptionValues()
        {
            return _assumptions.ToDictionary(a => a.Name, a => a.Value);
        }

        public bool ValidateAllAssumptions()
        {
            return _assumptions.All(a => a.IsWithinRange());
        }

        public List<string> GetInvalidAssumptions()
        {
            return _assumptions
                .Where(a => !a.IsWithinRange())
                .Select(a => a.Name)
                .ToList();
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
                maxLength: PerformanceFinancialModelingConsts.MaxAssumptionNameLength
            );
        }

        private void SetDescription(string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                Description = Check.Length(
                    description.Trim(),
                    nameof(description),
                    maxLength: PerformanceFinancialModelingConsts.MaxAssumptionDescriptionLength
                );
            }
        }
    }
}

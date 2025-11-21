using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects
{
    /// <summary>
    /// Value object representing an emergency procedure
    /// </summary>
    public class EmergencyProcedure : ValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<string> Steps { get; private set; }
        public TimeSpan EstimatedDuration { get; private set; }
        public CrisisLevel RequiredLevel { get; private set; }

        private EmergencyProcedure()
        {
            Steps = new List<string>();
        }

        public EmergencyProcedure(string name, string description, CrisisLevel requiredLevel, TimeSpan estimatedDuration)
        {
            SetName(name);
            SetDescription(description);
            RequiredLevel = requiredLevel;
            EstimatedDuration = estimatedDuration;
            Steps = new List<string>();
        }

        public void AddStep(string step)
        {
            if (string.IsNullOrWhiteSpace(step))
                throw new ArgumentException("Procedure step cannot be empty");
            
            Steps.Add(step.Trim());
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 100);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 1000);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
            yield return RequiredLevel;
            yield return EstimatedDuration;
            foreach (var step in Steps)
                yield return step;
        }
    }
}

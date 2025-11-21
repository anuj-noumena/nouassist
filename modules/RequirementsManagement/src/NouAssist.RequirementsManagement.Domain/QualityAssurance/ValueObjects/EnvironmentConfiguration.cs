using System.Collections.Generic;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.QualityAssurance.ValueObjects
{
    /// <summary>
    /// Represents the configuration details for a testing environment
    /// </summary>
    public class EnvironmentConfiguration : ValueObject
    {
        /// <summary>
        /// The name of the environment (e.g., Development, Staging, UAT)
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the environment configuration
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// URL of the environment
        /// </summary>
        public string Url { get; private set; }

        protected EnvironmentConfiguration() { }

        public EnvironmentConfiguration(string name, string description, string url)
        {
            SetName(name);
            SetDescription(description);
            SetUrl(url);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: QADomainConstants.MaxEnvironmentConfigurationLength);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: QADomainConstants.MaxEnvironmentConfigurationLength);
        }

        private void SetUrl(string url)
        {
            Url = url; // URL could be null if not available yet
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
            yield return Url ?? string.Empty;
        }
    }
}

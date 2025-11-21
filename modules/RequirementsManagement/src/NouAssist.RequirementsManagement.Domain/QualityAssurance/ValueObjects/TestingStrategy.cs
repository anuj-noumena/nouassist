using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.QualityAssurance.ValueObjects
{
    /// <summary>
    /// Represents a testing strategy with approach and coverage details
    /// </summary>
    public class TestingStrategy : ValueObject
    {
        /// <summary>
        /// The approach to testing (e.g., Exploratory, Regression, Smoke)
        /// </summary>
        public string Approach { get; private set; }
        
        /// <summary>
        /// Description of what will be covered in testing
        /// </summary>
        public string CoverageDescription { get; private set; }
        
        /// <summary>
        /// Areas or features that are explicitly out of scope
        /// </summary>
        public string OutOfScope { get; private set; }

        protected TestingStrategy() { }

        public TestingStrategy(string approach, string coverageDescription, string outOfScope)
        {
            SetApproach(approach);
            SetCoverageDescription(coverageDescription);
            SetOutOfScope(outOfScope);
        }

        private void SetApproach(string approach)
        {
            Approach = Check.NotNullOrWhiteSpace(approach, nameof(approach));
        }

        private void SetCoverageDescription(string coverageDescription)
        {
            CoverageDescription = Check.NotNullOrWhiteSpace(coverageDescription, nameof(coverageDescription));
        }

        private void SetOutOfScope(string outOfScope)
        {
            OutOfScope = outOfScope ?? string.Empty;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Approach;
            yield return CoverageDescription;
            yield return OutOfScope;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestRuns.Dtos
{
    /// <summary>
    /// Input for adding a test run to a test cycle
    /// </summary>
    public class AddTestRunDto
    {
        /// <summary>
        /// ID of the test cycle this run belongs to
        /// </summary>
        [Required]
        public Guid TestCycleId { get; set; }
        
        /// <summary>
        /// ID of the test case from V&V BC
        /// </summary>
        [Required]
        public Guid TestCaseId { get; set; }
        
        /// <summary>
        /// Title of the test case
        /// </summary>
        [Required]
        [StringLength(200)]
        public string TestCaseTitle { get; set; }
        
        /// <summary>
        /// The expected outcome of the test
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxExpectedOutcomeLength)]
        public string ExpectedOutcome { get; set; }
    }
}

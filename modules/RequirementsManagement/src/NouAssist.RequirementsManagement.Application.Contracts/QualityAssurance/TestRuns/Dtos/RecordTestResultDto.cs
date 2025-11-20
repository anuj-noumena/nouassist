using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestRuns.Dtos
{
    /// <summary>
    /// Input for recording a test result
    /// </summary>
    public class RecordTestResultDto
    {
        /// <summary>
        /// ID of the test cycle this run belongs to
        /// </summary>
        [Required]
        public Guid TestCycleId { get; set; }
        
        /// <summary>
        /// ID of the test case
        /// </summary>
        [Required]
        public Guid TestCaseId { get; set; }
        
        /// <summary>
        /// The result of the test run
        /// </summary>
        [Required]
        public TestRunResult Result { get; set; }
        
        /// <summary>
        /// The actual outcome description
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxActualOutcomeLength)]
        public string ActualOutcomeDescription { get; set; }
        
        /// <summary>
        /// Additional log data from the test execution
        /// </summary>
        [StringLength(4000)]
        public string ActualOutcomeLogData { get; set; }
    }
}

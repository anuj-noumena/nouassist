using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.TestRuns.Dtos
{
    /// <summary>
    /// Data transfer object for a test run
    /// </summary>
    public class TestRunDto : EntityDto
    {
        /// <summary>
        /// ID of the test cycle this run belongs to
        /// </summary>
        public Guid TestCycleId { get; set; }
        
        /// <summary>
        /// ID of the test case from V&V BC
        /// </summary>
        public Guid TestCaseId { get; set; }
        
        /// <summary>
        /// Title of the test case
        /// </summary>
        public string TestCaseTitle { get; set; }
        
        /// <summary>
        /// The expected outcome of the test
        /// </summary>
        public string ExpectedOutcome { get; set; }
        
        /// <summary>
        /// The actual outcome description
        /// </summary>
        public string ActualOutcomeDescription { get; set; }
        
        /// <summary>
        /// Additional log data from the test execution
        /// </summary>
        public string ActualOutcomeLogData { get; set; }
        
        /// <summary>
        /// The result of the test run
        /// </summary>
        public TestRunResult Result { get; set; }
        
        /// <summary>
        /// The user who executed the test
        /// </summary>
        public Guid? ExecutorId { get; set; }
        
        /// <summary>
        /// When the test run was executed
        /// </summary>
        public DateTime? ExecutionTime { get; set; }
        
        /// <summary>
        /// When the test run was created
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}

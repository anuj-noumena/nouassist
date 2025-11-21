using System;
using NouAssist.RequirementsManagement.QualityAssurance.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.QualityAssurance.Events
{
    /// <summary>
    /// Event raised when a test cycle is completed
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.QualityAssurance.TestCycleCompleted")]
    public class TestCycleCompletedEto : QAEtoBase
    {
        /// <summary>
        /// The name of the test cycle
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The ID of the test plan this cycle belongs to
        /// </summary>
        public Guid TestPlanId { get; set; }
        
        /// <summary>
        /// The date when the test cycle was completed
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Total number of test runs in this cycle
        /// </summary>
        public int TotalTestRuns { get; set; }
        
        /// <summary>
        /// Number of passed test runs
        /// </summary>
        public int PassedTestRuns { get; set; }
        
        /// <summary>
        /// Number of failed test runs
        /// </summary>
        public int FailedTestRuns { get; set; }
    }
}

using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.ValidationVerification.Events
{
    /// <summary>
    /// Event published when test execution is completed
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.ValidationVerification.TestExecutionCompleted")]
    public class TestExecutionCompletedEto : ValidationVerificationEtoBase
    {
        public Guid TestCaseId { get; set; }
        public string TestCaseName { get; set; }
        public TestExecutionStatus ExecutionStatus { get; set; }
        public Guid ExecutedBy { get; set; }
        public DateTime ExecutedAt { get; set; }
        public int TotalSteps { get; set; }
        public int PassedSteps { get; set; }
        public int FailedSteps { get; set; }
        public List<Guid> RequirementIds { get; set; } = new();
        public string ExecutionNotes { get; set; }
    }
}

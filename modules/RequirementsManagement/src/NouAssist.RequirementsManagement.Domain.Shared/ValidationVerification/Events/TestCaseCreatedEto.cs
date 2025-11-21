using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.ValidationVerification.Events
{
    /// <summary>
    /// Event published when a test case is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.ValidationVerification.TestCaseCreated")]
    public class TestCaseCreatedEto : ValidationVerificationEtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TestCasePriority Priority { get; set; }
        public List<Guid> RequirementIds { get; set; } = new();
        public Guid CreatedBy { get; set; }
        public int StepCount { get; set; }
    }
}

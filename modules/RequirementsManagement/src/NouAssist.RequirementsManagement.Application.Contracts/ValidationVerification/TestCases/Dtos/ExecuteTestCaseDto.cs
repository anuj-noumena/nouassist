using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos
{
    /// <summary>
    /// Input DTO for starting test execution
    /// </summary>
    public class StartTestExecutionDto
    {
        public string Environment { get; set; }
        public string TestDataUsed { get; set; }
    }

    /// <summary>
    /// Input DTO for completing test execution
    /// </summary>
    public class CompleteTestExecutionDto
    {
        [Required]
        public TestExecutionStatus OverallStatus { get; set; }

        [Required]
        public Dictionary<Guid, TestStepExecutionResultDto> StepResults { get; set; } = new();

        [StringLength(2000)]
        public string ExecutionNotes { get; set; }
    }

    /// <summary>
    /// Input DTO for test step execution result
    /// </summary>
    public class TestStepExecutionResultDto
    {
        [Required]
        public TestExecutionStatus Status { get; set; }

        [Required]
        [StringLength(ValidationVerificationConsts.TestStep.MaxActualResultLength)]
        public string ActualResult { get; set; }

        public string Evidence { get; set; }

        [StringLength(ValidationVerificationConsts.TestStep.MaxCommentsLength)]
        public string Comments { get; set; }
    }
}

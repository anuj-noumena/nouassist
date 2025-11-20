using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos
{
    /// <summary>
    /// Input DTO for creating a new test case
    /// </summary>
    public class CreateTestCaseDto
    {
        [Required]
        [StringLength(ValidationVerificationConsts.TestCase.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(ValidationVerificationConsts.TestCase.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public TestCasePriority Priority { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one requirement must be linked")]
        public List<Guid> RequirementIds { get; set; } = new();

        [StringLength(ValidationVerificationConsts.TestCase.MaxPreconditionsLength)]
        public string Preconditions { get; set; }

        [StringLength(ValidationVerificationConsts.TestCase.MaxPostconditionsLength)]
        public string Postconditions { get; set; }

        public TimeSpan? EstimatedDuration { get; set; }

        public List<CreateTestStepDto> TestSteps { get; set; } = new();
    }

    /// <summary>
    /// Input DTO for creating a test step
    /// </summary>
    public class CreateTestStepDto
    {
        [Required]
        [StringLength(ValidationVerificationConsts.TestStep.MaxInstructionLength)]
        public string Instruction { get; set; }

        [Required]
        [StringLength(ValidationVerificationConsts.TestStep.MaxExpectedResultLength)]
        public string ExpectedResult { get; set; }

        public string TestData { get; set; }
        public string Prerequisites { get; set; }
        public string AcceptanceCriteria { get; set; }
    }
}

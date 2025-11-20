using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos
{
    /// <summary>
    /// Input DTO for updating a test step
    /// </summary>
    public class UpdateTestStepDto
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

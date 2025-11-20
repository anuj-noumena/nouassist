using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos
{
    /// <summary>
    /// Input DTO for updating a test case
    /// </summary>
    public class UpdateTestCaseDto
    {
        [Required]
        [StringLength(ValidationVerificationConsts.TestCase.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(ValidationVerificationConsts.TestCase.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public TestCasePriority Priority { get; set; }

        [StringLength(ValidationVerificationConsts.TestCase.MaxPreconditionsLength)]
        public string Preconditions { get; set; }

        [StringLength(ValidationVerificationConsts.TestCase.MaxPostconditionsLength)]
        public string Postconditions { get; set; }

        public TimeSpan? EstimatedDuration { get; set; }
    }
}

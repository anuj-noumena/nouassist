using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class ValidateHypothesisDto
    {
        [Required]
        [StringLength(BusinessModelManagementConsts.Hypothesis.MaxTestDescriptionLength)]
        public string TestDescription { get; set; }

        [Required]
        [StringLength(BusinessModelManagementConsts.Hypothesis.MaxResultDescriptionLength)]
        public string TestResult { get; set; }

        [Required]
        public ValidationStatus FinalStatus { get; set; }

        public DateTime? TestStartDate { get; set; }
        public DateTime? TestEndDate { get; set; }
    }
}

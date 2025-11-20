using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class CompletePivotDto
    {
        [Required]
        [StringLength(BusinessModelManagementConsts.StrategicPivot.MaxActualOutcomeLength)]
        public string ActualOutcome { get; set; }

        [Required]
        public bool WasSuccessful { get; set; }

        public DateTime? CompletionDate { get; set; }
    }
}

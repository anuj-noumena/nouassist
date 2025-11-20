using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class CreateStrategicPivotDto
    {
        [Required]
        public PivotType Type { get; set; }

        [Required]
        public PivotRationaleDto Rationale { get; set; }

        [Required]
        [StringLength(BusinessModelManagementConsts.StrategicPivot.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public DateTime TargetCompletionDate { get; set; }
    }
}

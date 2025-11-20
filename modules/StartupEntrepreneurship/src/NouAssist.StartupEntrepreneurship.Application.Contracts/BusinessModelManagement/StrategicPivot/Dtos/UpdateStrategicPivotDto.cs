using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class UpdateStrategicPivotDto
    {
        [Required]
        [StringLength(BusinessModelManagementConsts.StrategicPivot.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public DateTime TargetCompletionDate { get; set; }
    }
}

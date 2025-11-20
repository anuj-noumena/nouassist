using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class CreateBusinessObjectiveLinkDto
    {
        [Required]
        public Guid ObjectiveId { get; set; }

        [StringLength(1000)]
        public string Justification { get; set; }
    }
}

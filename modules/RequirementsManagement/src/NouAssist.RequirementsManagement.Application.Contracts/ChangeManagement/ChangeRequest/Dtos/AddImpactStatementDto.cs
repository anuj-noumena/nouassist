using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// Input DTO for adding an impact statement
    /// </summary>
    public class AddImpactStatementDto
    {
        [Required]
        public ImpactType ImpactType { get; set; }

        public decimal? CostImpact { get; set; }

        public int? TimeImpactDays { get; set; }

        [StringLength(1000)]
        public string Dependencies { get; set; }

        [Required]
        [StringLength(ChangeManagementConsts.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.VersioningBaselining.Constants;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Baselining.Dtos
{
    /// <summary>
    /// DTO for creating a new baseline
    /// </summary>
    public class CreateBaselineDto
    {
        /// <summary>
        /// Name of the baseline
        /// </summary>
        [Required]
        [StringLength(VersionControlConsts.MaxBaselineNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the baseline
        /// </summary>
        [StringLength(VersionControlConsts.MaxBaselineDescriptionLength)]
        public string Description { get; set; }
    }
}

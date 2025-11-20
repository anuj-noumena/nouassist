using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos
{
    /// <summary>
    /// Input for verifying a defect fix
    /// </summary>
    public class VerifyDefectFixDto
    {
        /// <summary>
        /// Version or build where the fix was verified
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FixedInVersion { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos
{
    /// <summary>
    /// Input for marking a defect as fixed
    /// </summary>
    public class MarkDefectAsFixedDto
    {
        /// <summary>
        /// The commit ID that fixed the defect
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FixCommitId { get; set; }
    }
}

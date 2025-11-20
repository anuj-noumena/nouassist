using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos
{
    /// <summary>
    /// Input for adding a comment to a defect
    /// </summary>
    public class AddDefectCommentDto
    {
        /// <summary>
        /// The ID of the defect
        /// </summary>
        [Required]
        public Guid DefectId { get; set; }
        
        /// <summary>
        /// The text content of the comment
        /// </summary>
        [Required]
        [StringLength(QADomainConstants.MaxCommentLength)]
        public string Text { get; set; }
    }
}

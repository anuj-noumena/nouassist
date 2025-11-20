using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos
{
    /// <summary>
    /// Data transfer object for a defect comment
    /// </summary>
    public class DefectCommentDto : EntityDto
    {
        /// <summary>
        /// The text content of the comment
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// When the comment was created
        /// </summary>
        public DateTime CreationTime { get; set; }
        
        /// <summary>
        /// Who created the comment
        /// </summary>
        public Guid? CreatorId { get; set; }
    }
}

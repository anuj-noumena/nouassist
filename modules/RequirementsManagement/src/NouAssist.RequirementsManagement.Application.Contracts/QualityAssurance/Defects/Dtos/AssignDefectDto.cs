using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos
{
    /// <summary>
    /// Input for assigning a defect to a user
    /// </summary>
    public class AssignDefectDto
    {
        /// <summary>
        /// The ID of the user to assign the defect to
        /// </summary>
        [Required]
        public Guid AssigneeId { get; set; }
    }
}

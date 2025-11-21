using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Versioning.Dtos
{
    /// <summary>
    /// DTO for getting the version history of a requirement
    /// </summary>
    public class GetVersionHistoryDto
    {
        /// <summary>
        /// ID of the requirement
        /// </summary>
        [Required]
        public Guid RequirementId { get; set; }
    }
}

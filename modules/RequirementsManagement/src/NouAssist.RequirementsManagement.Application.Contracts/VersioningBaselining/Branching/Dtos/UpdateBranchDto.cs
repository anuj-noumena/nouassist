using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos
{
    /// <summary>
    /// DTO for updating a branch
    /// </summary>
    public class UpdateBranchDto
    {
        /// <summary>
        /// Description of the branch
        /// </summary>
        [StringLength(2000)]
        public string Description { get; set; }
    }
}

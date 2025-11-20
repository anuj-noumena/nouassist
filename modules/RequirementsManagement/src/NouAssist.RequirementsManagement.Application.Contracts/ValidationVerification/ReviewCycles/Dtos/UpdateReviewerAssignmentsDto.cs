using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos
{
    /// <summary>
    /// Input DTO for updating reviewer assignments
    /// </summary>
    public class UpdateReviewerAssignmentsDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one reviewer must be assigned")]
        public List<CreateReviewerAssignmentDto> ReviewerAssignments { get; set; } = new();
    }
}

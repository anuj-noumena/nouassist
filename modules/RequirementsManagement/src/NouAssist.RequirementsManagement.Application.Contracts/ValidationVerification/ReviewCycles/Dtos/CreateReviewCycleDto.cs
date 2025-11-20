using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos
{
    /// <summary>
    /// Input DTO for creating a new review cycle
    /// </summary>
    public class CreateReviewCycleDto
    {
        [Required]
        [StringLength(ValidationVerificationConsts.RequirementReviewCycle.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(ValidationVerificationConsts.RequirementReviewCycle.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public Guid RequirementId { get; set; }

        [Required]
        public CreateWorkflowDefinitionDto WorkflowDefinition { get; set; }

        public DateTime? TargetCompletionDate { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one reviewer must be assigned")]
        public List<CreateReviewerAssignmentDto> ReviewerAssignments { get; set; } = new();
    }

    /// <summary>
    /// Input DTO for workflow definition
    /// </summary>
    public class CreateWorkflowDefinitionDto
    {
        [Required]
        public Guid DefinitionId { get; set; }

        [Required]
        [StringLength(ValidationVerificationConsts.WorkflowDefinition.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(ValidationVerificationConsts.WorkflowDefinition.MaxVersionLength)]
        public string Version { get; set; }
    }

    /// <summary>
    /// Input DTO for reviewer assignment
    /// </summary>
    public class CreateReviewerAssignmentDto
    {
        [Required]
        public Guid ReviewerId { get; set; }

        [Required]
        [StringLength(256)]
        public string ReviewerName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string ReviewerEmail { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsRequired { get; set; } = true;
    }
}

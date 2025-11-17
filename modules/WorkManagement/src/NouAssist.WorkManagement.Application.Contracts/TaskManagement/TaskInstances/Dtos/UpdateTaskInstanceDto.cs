using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class UpdateTaskInstanceDto
    {
        public Guid? AssigneeId { get; set; }
        
        // GTD Organization
        public Guid? AreaId { get; set; }
        public Guid? ContextId { get; set; }
        
        // Schedule updates
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedDueDate { get; set; }
        public DateTime? ActualDueDate { get; set; }

        // Priority updates
        [Range(0.0, 1.0)]
        public double? Importance { get; set; }

        [Range(0.0, 1.0)]
        public double? Urgency { get; set; }

        // Work tracking
        public bool? IsNextAction { get; set; }
        public int? EstimatedMinutes { get; set; }

        [StringLength(4000)]
        public string Notes { get; set; }

        // Complexity and Cognitive Load
        public CreateComplexityEstimationDto ComplexityEstimation { get; set; }
        public CognitiveLoad? CognitiveLoad { get; set; }

        // Flags
        public bool? IsFlagged { get; set; }
        [StringLength(1000)]
        public string FlaggedReason { get; set; }
    }
}
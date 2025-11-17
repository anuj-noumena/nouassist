using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class CreateTaskInstanceDto
    {
        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public Guid WorkflowId { get; set; }

        [Required]
        public Guid WorkflowStatusId { get; set; }

        // Either ProjectId OR AreaId must be set, not both
        public Guid? ProjectId { get; set; }
        public Guid? AreaId { get; set; }
        
        // GTD Context
        public Guid? ContextId { get; set; }

        public Guid? AssigneeId { get; set; }
        public Guid? ReporterId { get; set; }

        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedDueDate { get; set; }

        [Range(0.0, 1.0)]
        public double? Importance { get; set; }

        [Range(0.0, 1.0)]
        public double? Urgency { get; set; }

        public int? EstimatedMinutes { get; set; }

        public CreateComplexityEstimationDto ComplexityEstimation { get; set; }
        public CognitiveLoad? CognitiveLoad { get; set; }
    }
}
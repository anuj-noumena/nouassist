using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Tasks.TaskInstances.Dtos
{
    public class CreateTaskInstanceDto
    {
        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid WorkflowStatusId { get; set; }

        public Guid? AssigneeId { get; set; }

        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedDueDate { get; set; }

        [Range(0.0, 1.0)]
        public double? Importance { get; set; }

        [Range(0.0, 1.0)]
        public double? Urgency { get; set; }
    }
}

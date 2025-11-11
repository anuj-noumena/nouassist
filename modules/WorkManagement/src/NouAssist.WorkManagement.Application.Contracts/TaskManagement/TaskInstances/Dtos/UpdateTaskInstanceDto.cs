using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class UpdateTaskInstanceDto
    {
        public Guid? AssigneeId { get; set; }
        
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedDueDate { get; set; }
        public DateTime? ActualDueDate { get; set; }

        [Range(0.0, 1.0)]
        public double? Importance { get; set; }

        [Range(0.0, 1.0)]
        public double? Urgency { get; set; }

        public bool? IsNextAction { get; set; }
        public bool? IsFlagged { get; set; }

        [StringLength(1000)]
        public string FlaggedReason { get; set; }
    }
}

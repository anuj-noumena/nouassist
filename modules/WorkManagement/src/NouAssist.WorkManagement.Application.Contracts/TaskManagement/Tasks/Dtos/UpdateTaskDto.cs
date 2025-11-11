using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.TaskManagement.Tasks.Dtos
{
    public class UpdateTaskDto
    {
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        [StringLength(4000)]
        public string Notes { get; set; }

        [StringLength(2000)]
        public string ExpectedResult { get; set; }

        [Range(0.0, 1.0)]
        public double? DefaultImportance { get; set; }

        [Range(0.0, 1.0)]
        public double? DefaultUrgency { get; set; }

        public Guid? DefaultContextId { get; set; }
        public Guid? DefaultAreaId { get; set; }
        public Guid? DefaultSprintId { get; set; }
        public Guid? TaskTypeId { get; set; }

        public int? EstimateMinutes { get; set; }
        public DateTime? DefaultDueDate { get; set; }
    }
}

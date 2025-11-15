using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.TaskManagement.Tasks.Dtos
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        public Guid? ProjectId { get; set; }
        
        // GTD Organization
        public Guid? AreaId { get; set; }
        public Guid? ContextId { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        [StringLength(2000)]
        public string ExpectedResult { get; set; }

        [Range(0.0, 1.0)]
        public double DefaultImportance { get; set; } = 0.5;

        [Range(0.0, 1.0)]
        public double DefaultUrgency { get; set; } = 0.5;

        // Default values for instances
        public Guid? DefaultContextId { get; set; }
        public Guid? DefaultAreaId { get; set; }
        public Guid? TaskTypeId { get; set; }

        public int? DefaultEstimateMinutes { get; set; }
        public DateTime? DefaultDueDate { get; set; }
        public Guid? DefaultWorkflowId { get; set; }
        public Guid? DefaultAssigneeId { get; set; }

        public RecurrenceRuleDto RecurrenceRule { get; set; }

        // Updated from ActionSteps to ChecklistItems
        public List<CreateChecklistItemDto> ChecklistItems { get; set; }
        public List<CreateAcceptanceCriterionDto> AcceptanceCriteria { get; set; }
    }

    // Renamed from CreateTaskActionStepDto
    public class CreateChecklistItemDto
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public bool IsRequired { get; set; }
        public int OrderIndex { get; set; }
    }

    public class CreateAcceptanceCriterionDto
    {
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public int? Priority { get; set; }

        [StringLength(500)]
        public string GivenContext { get; set; }

        [StringLength(500)]
        public string WhenAction { get; set; }

        [StringLength(500)]
        public string ThenOutcome { get; set; }

        [StringLength(2000)]
        public string TestScenario { get; set; }
    }
}
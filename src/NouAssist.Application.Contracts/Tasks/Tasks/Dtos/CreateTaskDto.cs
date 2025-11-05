using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Tasks.Tasks.Dtos
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        public Guid? ProjectId { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        [StringLength(4000)]
        public string Notes { get; set; }

        [StringLength(2000)]
        public string ExpectedResult { get; set; }

        [Range(0.0, 1.0)]
        public double DefaultImportance { get; set; } = 0.5;

        [Range(0.0, 1.0)]
        public double DefaultUrgency { get; set; } = 0.5;

        public Guid? DefaultContextId { get; set; }
        public Guid? DefaultAreaId { get; set; }
        public Guid? DefaultSprintId { get; set; }
        public Guid? TaskTypeId { get; set; }

        public int? EstimateMinutes { get; set; }
        public DateTime? DefaultDueDate { get; set; }

        public RecurrenceRuleDto RecurrenceRule { get; set; }

        public List<CreateTaskActionStepDto> ActionSteps { get; set; }
        public List<CreateAcceptanceCriterionDto> AcceptanceCriteria { get; set; }
    }

    public class CreateTaskActionStepDto
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        public bool IsRequired { get; set; }
        public int Sequence { get; set; }
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

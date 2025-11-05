using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Tasks.Tasks.Dtos
{
    public class TaskDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public Guid? ProjectId { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string ExpectedResult { get; set; }
        
        public double DefaultImportance { get; set; }
        public double DefaultUrgency { get; set; }
        
        public Guid? DefaultContextId { get; set; }
        public Guid? DefaultAreaId { get; set; }
        public Guid? DefaultSprintId { get; set; }
        public Guid? TaskTypeId { get; set; }
        
        public int? EstimateMinutes { get; set; }
        public DateTime? DefaultDueDate { get; set; }
        
        public bool IsRecurring { get; set; }
        public RecurrenceRuleDto RecurrenceRule { get; set; }
        
        public bool IsActive { get; set; }
        public int Version { get; set; }

        public List<TaskActionStepDto> ActionSteps { get; set; }
        public List<AcceptanceCriterionDto> AcceptanceCriteria { get; set; }
    }

    public class TaskActionStepDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string Notes { get; set; }
        public bool IsRequired { get; set; }
        public int Sequence { get; set; }
    }

    public class AcceptanceCriterionDto : EntityDto<Guid>
    {
        public int? Priority { get; set; }
        public string Description { get; set; }
        public string GivenContext { get; set; }
        public string WhenAction { get; set; }
        public string ThenOutcome { get; set; }
        public string TestScenario { get; set; }
        public string Notes { get; set; }
        public bool HasGivenWhenThen { get; set; }
    }

    public class RecurrenceRuleDto
    {
        public string Frequency { get; set; }
        public int Interval { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Count { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; set; }
    }
}

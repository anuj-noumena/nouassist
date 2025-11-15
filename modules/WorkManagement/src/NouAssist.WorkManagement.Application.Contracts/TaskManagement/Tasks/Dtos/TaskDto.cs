using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.Tasks.Dtos
{
    public class TaskDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public Guid? ProjectId { get; set; }
        
        // GTD Organization
        public Guid? AreaId { get; set; }
        public Guid? ContextId { get; set; }
        
        public string Description { get; set; }
        public string ExpectedResult { get; set; }
        
        public double DefaultImportance { get; set; }
        public double DefaultUrgency { get; set; }
        
        // Default values for instances
        public Guid? DefaultContextId { get; set; }
        public Guid? DefaultAreaId { get; set; }
        public Guid? TaskTypeId { get; set; }
        public Guid? DefaultWorkflowId { get; set; }
        public Guid? DefaultAssigneeId { get; set; }
        
        public int? DefaultEstimateMinutes { get; set; }
        public DateTime? DefaultDueDate { get; set; }
        
        public bool IsRecurring { get; set; }
        public RecurrenceRuleDto RecurrenceRule { get; set; }
        
        public bool IsActive { get; set; }
        public int Version { get; set; }

        // Updated from ActionSteps to ChecklistItems
        public List<ChecklistItemDto> ChecklistItems { get; set; }
        public List<AcceptanceCriterionDto> AcceptanceCriteria { get; set; }
        
        // Computed properties
        public bool IsInArea { get; set; }
        public bool IsInProject { get; set; }
        public bool IsInContext { get; set; }
        public bool IsOrganized { get; set; }
    }

    // Renamed from TaskActionStepDto
    public class ChecklistItemDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public int OrderIndex { get; set; }
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
using System;
using System.Collections.Generic;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class TaskInstanceDetailDto : TaskInstanceDto
    {
        // Updated from StepCompletions to ChecklistItemCompletions
        public List<ChecklistItemCompletionDto> ChecklistItemCompletions { get; set; }
        public List<AcceptanceCriterionCompletionDto> CriterionCompletions { get; set; }
        public List<TaskDependencyDto> Dependencies { get; set; }
    }

    // Renamed from TaskActionStepCompletionDto
    public class ChecklistItemCompletionDto
    {
        public Guid ChecklistItemId { get; set; }
        public bool IsChecked { get; set; }
        public DateTime? CheckedAt { get; set; }
        public Guid? CheckedBy { get; set; }
    }

    public class AcceptanceCriterionCompletionDto
    {
        public Guid CriterionId { get; set; }
        public bool IsMet { get; set; }
        public DateTime? MetAt { get; set; }
        public Guid? MetBy { get; set; }
    }

    public class TaskDependencyDto
    {
        public Guid? DependsOnTaskId { get; set; }
        public Guid? DependsOnTaskInstanceId { get; set; }
        public string DependencyType { get; set; }
        public int LagMinutes { get; set; }
        public bool IsBlocking { get; set; }
    }
}
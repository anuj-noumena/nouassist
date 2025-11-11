using System;
using System.Collections.Generic;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class TaskInstanceDetailDto : TaskInstanceDto
    {
        public List<TaskActionStepCompletionDto> StepCompletions { get; set; }
        public List<AcceptanceCriterionCompletionDto> CriterionCompletions { get; set; }
        public List<TaskDependencyDto> Dependencies { get; set; }
    }

    public class TaskActionStepCompletionDto
    {
        public Guid TaskActionStepId { get; set; }
        public bool IsDone { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid? CompletedBy { get; set; }
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

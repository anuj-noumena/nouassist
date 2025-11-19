using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.TaskManagement.Enums;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class ChangeWorkflowStatusDto
    {
        [Required]
        public Guid WorkflowStatusId { get; set; }
        
        [StringLength(500)]
        public string Comment { get; set; }
        
        [StringLength(500)]
        public string Note { get; set; }
    }

    public class AssignTaskDto
    {
        [Required]
        public Guid AssigneeId { get; set; }
        
        [StringLength(500)]
        public string Note { get; set; }
    }

    public class MoveTaskDto
    {
        [Required]
        public Guid NewProjectId { get; set; }
        
        [Required]
        public Guid NewWorkflowStatusId { get; set; }
    }

    public class SetParentTaskDto
    {
        [Required]
        public Guid ParentTaskInstanceId { get; set; }
    }

    public class AddDependencyDto
    {
        [Required]
        public Guid DependsOnTaskInstanceId { get; set; }
        
        [Required]
        public DependencyType DependencyType { get; set; }
        
        public int LagMinutes { get; set; }
        
        public bool IsBlocking { get; set; } = true;
    }

    public class RemoveDependencyDto
    {
        [Required]
        public Guid DependsOnTaskInstanceId { get; set; }
    }

    public class BulkUpdateTaskInstancesDto
    {
        [Required]
        public Guid[] TaskInstanceIds { get; set; }
        
        public Guid? AssigneeId { get; set; }
        public Guid? WorkflowStatusId { get; set; }
        public Guid? ProjectId { get; set; }
        public bool? IsNextAction { get; set; }
        public bool? IsFlagged { get; set; }
    }

    public class ReorderSubtasksDto
    {
        [Required]
        public Guid[] OrderedTaskInstanceIds { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace NouAssist.WorkManagement.Workflow.Workflows.Dtos
{
    public class WorkflowTransitionDto
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid FromStatusId { get; set; }
        public Guid ToStatusId { get; set; }
        public List<string> AllowedRoles { get; set; } = new();
        public List<Guid> AllowedUserIds { get; set; } = new();
        public bool RequiresComment { get; set; }
        public bool RequiresResolution { get; set; }
        public bool IsGlobal { get; set; }
        public int OrderIndex { get; set; }
        public string ButtonLabel { get; set; }
        public string ButtonColor { get; set; }
        public bool HasConditions { get; set; }
        public bool HasValidators { get; set; }
        public bool HasRestrictions { get; set; }
        public List<WorkflowActionDto> Actions { get; set; } = new();
    }
}

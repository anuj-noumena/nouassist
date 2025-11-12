using System;
using NouAssist.WorkManagement.Workflow.Enums;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class WorkflowStatusDto
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkflowStatusCategory Category { get; set; }
        public string Color { get; set; }
        public int OrderIndex { get; set; }
        public bool IsInitial { get; set; }
        public bool IsResolution { get; set; }
        public bool IsTodo { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsDone { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsTerminal { get; set; }
    }
}

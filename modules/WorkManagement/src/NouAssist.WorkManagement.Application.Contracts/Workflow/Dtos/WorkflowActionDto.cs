using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.Workflow.Enums;

namespace NouAssist.WorkManagement.Workflow.Workflows.Dtos
{
    public class WorkflowActionDto
    {
        public Guid Id { get; set; }
        public Guid TransitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkflowActionType ActionType { get; set; }
        public Dictionary<string, object> Configuration { get; set; }
        public int OrderIndex { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsAsync { get; set; }
        public bool ContinueOnError { get; set; }
        public bool HasConditions { get; set; }
    }
}

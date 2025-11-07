using System;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Enums
{
    public enum WorkflowStatusCategory : byte
    {
        Todo = 1,
        InProgress = 2,
        Done = 3,
        Blocked = 4,
        Cancelled = 5
    }
}
namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Enums
{
    public enum ApprovalWorkflowType : byte
    {
        TaskApproval = 1,
        ProjectApproval = 2,
        ExpenseApproval = 3,
        LeaveApproval = 4,
        DocumentApproval = 5,
        ChangeRequest = 6,
        Custom = 99
    }
}

using Volo.Abp;

namespace NouAssist.WorkManagement.Approval.Exceptions
{
    public class ApprovalWorkflowException : BusinessException
    {
        public ApprovalWorkflowException(string message, string code = null) 
            : base(code ?? "Approval:WorkflowError", message)
        {
        }
    }

    public class ApprovalRequestException : BusinessException
    {
        public ApprovalRequestException(string message, string code = null) 
            : base(code ?? "Approval:RequestError", message)
        {
        }
    }

    public class InvalidApprovalStateException : BusinessException
    {
        public InvalidApprovalStateException(string currentState, string attemptedAction) 
            : base("Approval:InvalidState", 
                  $"Cannot {attemptedAction} when in {currentState} state")
        {
            WithData("CurrentState", currentState);
            WithData("AttemptedAction", attemptedAction);
        }
    }
}

using System;

namespace NouAssist.RequirementsManagement.ChangeManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a user is not authorized to approve a change request
    /// </summary>
    public class UnauthorizedApprovalException : ChangeManagementDomainException
    {
        public UnauthorizedApprovalException(Guid userId, Guid changeRequestId)
            : base("ChangeManagement:ErrorMessage:UnauthorizedApproval")
        {
            WithData("userId", userId);
            WithData("changeRequestId", changeRequestId);
        }
    }
}

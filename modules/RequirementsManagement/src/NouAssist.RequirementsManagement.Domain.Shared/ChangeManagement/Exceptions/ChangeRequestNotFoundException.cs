using System;

namespace NouAssist.RequirementsManagement.ChangeManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a change request is not found
    /// </summary>
    public class ChangeRequestNotFoundException : ChangeManagementDomainException
    {
        public ChangeRequestNotFoundException(Guid changeRequestId)
            : base("ChangeManagement:ErrorMessage:ChangeRequestNotFound")
        {
            WithData("changeRequestId", changeRequestId);
        }

        public ChangeRequestNotFoundException(string requestIdentifier)
            : base("ChangeManagement:ErrorMessage:ChangeRequestNotFound")
        {
            WithData("requestIdentifier", requestIdentifier);
        }
    }
}

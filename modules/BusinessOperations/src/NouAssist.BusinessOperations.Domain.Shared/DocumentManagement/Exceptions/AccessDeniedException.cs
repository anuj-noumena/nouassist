using System;
using NouAssist.BusinessOperations.DocumentManagement.Enums;

namespace NouAssist.BusinessOperations.DocumentManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when access to a document is denied
    /// </summary>
    public class AccessDeniedException : DocumentManagementDomainException
    {
        public Guid DocumentId { get; }
        public Guid PrincipalId { get; }
        public AccessPermissionLevel RequiredPermission { get; }

        public AccessDeniedException(
            Guid documentId, 
            Guid principalId, 
            AccessPermissionLevel requiredPermission)
            : base("DocumentManagement:ErrorMessage:AccessDenied")
        {
            DocumentId = documentId;
            PrincipalId = principalId;
            RequiredPermission = requiredPermission;
            
            WithData("documentId", documentId);
            WithData("principalId", principalId);
            WithData("requiredPermission", requiredPermission);
        }
    }
}

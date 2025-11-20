using System;

namespace NouAssist.BusinessOperations.DocumentManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid operation is attempted on a document
    /// </summary>
    public class InvalidDocumentOperationException : DocumentManagementDomainException
    {
        public Guid DocumentId { get; }
        public string Operation { get; }

        public InvalidDocumentOperationException(
            Guid documentId, 
            string operation, 
            string reason)
            : base("DocumentManagement:ErrorMessage:InvalidOperation")
        {
            DocumentId = documentId;
            Operation = operation;
            
            WithData("documentId", documentId);
            WithData("operation", operation);
            WithData("reason", reason);
        }
    }
}

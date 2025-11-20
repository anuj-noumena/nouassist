using System;

namespace NouAssist.BusinessOperations.DocumentManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a document is not found
    /// </summary>
    public class DocumentNotFoundException : DocumentManagementDomainException
    {
        public Guid DocumentId { get; }

        public DocumentNotFoundException(Guid documentId)
            : base("DocumentManagement:ErrorMessage:DocumentNotFound")
        {
            DocumentId = documentId;
            WithData("documentId", documentId);
        }
    }
}

using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects
{
    /// <summary>
    /// References a document stored in the Document Management BC
    /// </summary>
    public class DocumentId : ValueObject
    {
        /// <summary>
        /// The unique identifier of the document
        /// </summary>
        public Guid Id { get; private set; }
        
        /// <summary>
        /// The original filename of the document
        /// </summary>
        public string FileName { get; private set; }
        
        /// <summary>
        /// MIME type of the document
        /// </summary>
        public string ContentType { get; private set; }

        protected DocumentId()
        {
        }

        public DocumentId(Guid id, string fileName, string contentType)
        {
            Id = id;
            FileName = Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
            ContentType = Check.NotNullOrWhiteSpace(contentType, nameof(contentType));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return FileName;
            yield return ContentType;
        }
    }
}

namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing a reference to a document in the document management system
    /// </summary>
    [Serializable]
    public class DocumentId : ValueObject
    {
        /// <summary>
        /// The unique identifier of the document
        /// </summary>
        public string Value { get; private set; }

        protected DocumentId()
        {
        }

        public DocumentId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BusinessException("FreelanceOperations:Validation:Required", "Document ID");
            }

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
        
        public override string ToString()
        {
            return Value;
        }
        
        public static implicit operator string(DocumentId documentId)
        {
            return documentId?.Value;
        }
        
        public static explicit operator DocumentId(string value)
        {
            return new DocumentId(value);
        }
    }
}

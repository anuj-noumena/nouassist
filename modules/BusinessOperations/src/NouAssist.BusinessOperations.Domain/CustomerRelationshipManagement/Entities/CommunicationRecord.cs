namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities
{
    /// <summary>
    /// Records communication with a lead or contact
    /// </summary>
    public class CommunicationRecord : Entity
    {
        /// <summary>
        /// The interaction log this record is part of
        /// </summary>
        public Guid InteractionLogId { get; private set; }
        
        /// <summary>
        /// Type of communication
        /// </summary>
        public InteractionType Type { get; private set; }
        
        /// <summary>
        /// Subject or title of the communication
        /// </summary>
        public string Subject { get; private set; }
        
        /// <summary>
        /// Content of the communication
        /// </summary>
        public string Content { get; private set; }
        
        /// <summary>
        /// When the communication occurred
        /// </summary>
        public DateTime CommunicationTime { get; private set; }
        
        /// <summary>
        /// Direction of communication (inbound/outbound)
        /// </summary>
        public bool IsInbound { get; private set; }
        
        /// <summary>
        /// Sender information (email, phone, etc.)
        /// </summary>
        public string Sender { get; private set; }
        
        /// <summary>
        /// Recipient information (email, phone, etc.)
        /// </summary>
        public string Recipient { get; private set; }
        
        /// <summary>
        /// Optional document attachments
        /// </summary>
        private readonly List<DocumentId> _attachments;
        
        /// <summary>
        /// Read-only access to attachments
        /// </summary>
        public IReadOnlyCollection<DocumentId> Attachments => _attachments.AsReadOnly();

        protected CommunicationRecord()
        {
            _attachments = new List<DocumentId>();
        }

        internal CommunicationRecord(
            Guid interactionLogId,
            InteractionType type,
            string subject,
            string content,
            DateTime communicationTime,
            bool isInbound,
            string sender,
            string recipient)
        {
            InteractionLogId = interactionLogId;
            Type = type;
            Subject = Check.NotNullOrWhiteSpace(subject, nameof(subject));
            Content = content;
            CommunicationTime = communicationTime;
            IsInbound = isInbound;
            Sender = sender;
            Recipient = recipient;
            _attachments = new List<DocumentId>();
        }

        internal void AddAttachment(DocumentId attachment)
        {
            _attachments.Add(attachment);
        }

        internal void RemoveAttachment(Guid attachmentId)
        {
            var attachment = _attachments.FirstOrDefault(a => a.Id == attachmentId);
            if (attachment != null)
            {
                _attachments.Remove(attachment);
            }
        }

        public override object[] GetKeys()
        {
            return new object[] { InteractionLogId, CommunicationTime };
        }
    }
}

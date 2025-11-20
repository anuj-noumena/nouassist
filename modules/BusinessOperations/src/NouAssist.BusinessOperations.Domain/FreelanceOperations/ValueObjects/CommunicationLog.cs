namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing a communication log entry with a client
    /// </summary>
    public class CommunicationLog : ValueObject
    {
        /// <summary>
        /// Date and time of communication
        /// </summary>
        public DateTime Timestamp { get; private set; }
        
        /// <summary>
        /// Type of communication (Email, Call, Meeting, etc.)
        /// </summary>
        public string Type { get; private set; }
        
        /// <summary>
        /// Subject or title of the communication
        /// </summary>
        public string Subject { get; private set; }
        
        /// <summary>
        /// Content or notes from the communication
        /// </summary>
        public string Content { get; private set; }
        
        /// <summary>
        /// User ID who logged this communication
        /// </summary>
        public Guid UserId { get; private set; }

        protected CommunicationLog()
        {
        }

        public CommunicationLog(
            DateTime timestamp,
            string type,
            string subject,
            string content,
            Guid userId)
        {
            Timestamp = timestamp;
            Type = Check.NotNullOrWhiteSpace(type, nameof(type));
            Subject = Check.NotNullOrWhiteSpace(subject, nameof(subject));
            Content = Check.NotNullOrWhiteSpace(content, nameof(content));
            UserId = userId;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Timestamp;
            yield return Type;
            yield return Subject;
            yield return Content;
            yield return UserId;
        }
    }
}

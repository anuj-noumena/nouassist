namespace NouAssist.BusinessOperations.FreelanceOperations.Events
{
    /// <summary>
    /// Event raised when a freelance client is updated
    /// </summary>
    [EventName("NouAssist.BusinessOperations.FreelanceOperations.ClientUpdated")]
    public class FreelanceClientUpdatedEto : ClientEtoBase
    {
        /// <summary>
        /// Time when the client was updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}

namespace NouAssist.BusinessOperations.FreelanceOperations.Events
{
    /// <summary>
    /// Event raised when a freelance project is updated
    /// </summary>
    [EventName("NouAssist.BusinessOperations.FreelanceOperations.ProjectUpdated")]
    public class FreelanceProjectUpdatedEto : ProjectEtoBase
    {
        /// <summary>
        /// Time when the project was updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}

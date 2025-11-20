namespace NouAssist.BusinessOperations.FreelanceOperations.Events
{
    /// <summary>
    /// Event raised when a new freelance project is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.FreelanceOperations.ProjectCreated")]
    public class FreelanceProjectCreatedEto : ProjectEtoBase
    {
        /// <summary>
        /// Start date of the project
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Expected end date of the project
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}

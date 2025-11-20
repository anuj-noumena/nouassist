namespace NouAssist.BusinessOperations.ProjectExecution.Events
{
    /// <summary>
    /// Event raised when time is logged
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ProjectExecution.TimeLogged")]
    public class TimeLoggedEto : ProjectExecutionEtoBase
    {
        /// <summary>
        /// ID of the time log
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// ID of the user who logged time
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// ID of the task time was logged against
        /// </summary>
        public Guid? TaskId { get; set; }
        
        /// <summary>
        /// ID of the project time was logged against
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Duration of time logged in hours
        /// </summary>
        public double Hours { get; set; }
        
        /// <summary>
        /// Date when the work was performed
        /// </summary>
        public DateTime LogDate { get; set; }
    }
}

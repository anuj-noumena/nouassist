namespace NouAssist.BusinessOperations.ProjectExecution.ValueObjects
{
    /// <summary>
    /// Value object containing the details of a time log entry
    /// </summary>
    public class TimeLogDetails : ValueObject
    {
        /// <summary>
        /// Duration of the logged time in hours
        /// </summary>
        public double Hours { get; private set; }
        
        /// <summary>
        /// Date when the work was performed
        /// </summary>
        public DateTime LogDate { get; private set; }
        
        /// <summary>
        /// Optional notes about the logged time
        /// </summary>
        public string Notes { get; private set; }
        
        /// <summary>
        /// Optional ID of the resource instance
        /// </summary>
        public Guid? ResourceInstanceId { get; private set; }
        
        protected TimeLogDetails() { }
        
        public TimeLogDetails(
            double hours,
            DateTime logDate,
            string notes = null,
            Guid? resourceInstanceId = null)
        {
            SetHours(hours);
            LogDate = logDate;
            SetNotes(notes);
            ResourceInstanceId = resourceInstanceId;
        }
        
        private void SetHours(double hours)
        {
            if (hours <= 0)
            {
                throw new BusinessException("ProjectExecution:Validation:DurationInvalid");
            }
            
            Hours = hours;
        }
        
        private void SetNotes(string notes)
        {
            if (notes != null && notes.Length > ProjectExecutionConsts.MaxTimeLogNotesLength)
            {
                throw new BusinessException($"Notes cannot exceed {ProjectExecutionConsts.MaxTimeLogNotesLength} characters");
            }
            
            Notes = notes;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Hours;
            yield return LogDate;
            yield return Notes ?? string.Empty;
            yield return ResourceInstanceId ?? Guid.Empty;
        }
    }
}

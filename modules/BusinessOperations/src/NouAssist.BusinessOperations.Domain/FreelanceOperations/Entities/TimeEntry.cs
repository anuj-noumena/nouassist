namespace NouAssist.BusinessOperations.FreelanceOperations.Entities
{
    /// <summary>
    /// Entity representing a time entry for tracking work on a project
    /// </summary>
    public class TimeEntry : Entity
    {
        /// <summary>
        /// ID of the project this time entry belongs to
        /// </summary>
        public Guid ProjectId { get; private set; }
        
        /// <summary>
        /// Date when the work was performed
        /// </summary>
        public DateTime Date { get; private set; }
        
        /// <summary>
        /// Start time of the work
        /// </summary>
        public TimeSpan StartTime { get; private set; }
        
        /// <summary>
        /// Duration of the work in minutes
        /// </summary>
        public int DurationMinutes { get; private set; }
        
        /// <summary>
        /// Description of the work performed
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Whether the time entry has been billable
        /// </summary>
        public bool IsBillable { get; private set; }
        
        /// <summary>
        /// Whether the time entry has been invoiced
        /// </summary>
        public bool IsInvoiced { get; private set; }
        
        /// <summary>
        /// Invoice ID if this time entry has been invoiced
        /// </summary>
        public Guid? InvoiceId { get; private set; }

        protected TimeEntry()
        {
        }

        internal TimeEntry(
            Guid projectId,
            DateTime date,
            TimeSpan startTime,
            int durationMinutes,
            string description,
            bool isBillable = true)
        {
            ProjectId = projectId;
            Date = date.Date; // Strip time component
            StartTime = startTime;
            
            if (durationMinutes <= 0)
            {
                throw new BusinessException("FreelanceOperations:Validation:InvalidValue", "Duration");
            }
            
            DurationMinutes = durationMinutes;
            
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: FreelanceOperationsConsts.TimeEntry.MaxDescriptionLength
            );
            
            IsBillable = isBillable;
            IsInvoiced = false;
        }

        /// <summary>
        /// Update the description of this time entry
        /// </summary>
        internal void UpdateDescription(string description)
        {
            if (IsInvoiced)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: FreelanceOperationsConsts.TimeEntry.MaxDescriptionLength
            );
        }

        /// <summary>
        /// Update the duration of this time entry
        /// </summary>
        internal void UpdateDuration(int durationMinutes)
        {
            if (IsInvoiced)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            if (durationMinutes <= 0)
            {
                throw new BusinessException("FreelanceOperations:Validation:InvalidValue", "Duration");
            }
            
            DurationMinutes = durationMinutes;
        }

        /// <summary>
        /// Mark this time entry as invoiced
        /// </summary>
        internal void MarkAsInvoiced(Guid invoiceId)
        {
            if (IsInvoiced)
            {
                return; // Already invoiced
            }
            
            IsInvoiced = true;
            InvoiceId = invoiceId;
        }

        /// <summary>
        /// Mark this time entry as not invoiced (e.g., if invoice is canceled)
        /// </summary>
        internal void MarkAsNotInvoiced()
        {
            IsInvoiced = false;
            InvoiceId = null;
        }

        /// <summary>
        /// Toggle the billable status of this time entry
        /// </summary>
        internal void SetBillable(bool isBillable)
        {
            if (IsInvoiced && IsBillable && !isBillable)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            IsBillable = isBillable;
        }

        /// <summary>
        /// Get the formatted duration as hours and minutes
        /// </summary>
        public string GetFormattedDuration()
        {
            var hours = DurationMinutes / 60;
            var minutes = DurationMinutes % 60;
            
            return $"{hours}h {minutes}m";
        }

        /// <summary>
        /// Get the duration in decimal hours
        /// </summary>
        public decimal GetDurationInHours()
        {
            return Math.Round((decimal)DurationMinutes / 60, 2);
        }

        public override object[] GetKeys()
        {
            return new object[] { ProjectId, Date, StartTime };
        }
    }
}

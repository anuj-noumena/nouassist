namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Entities
{
    /// <summary>
    /// Represents a filing deadline for an intellectual property asset
    /// </summary>
    public class FilingDeadline : Entity<Guid>
    {
        public Guid AssetId { get; private set; }
        public string Title { get; private set; }
        public DateTime Deadline { get; private set; }
        public string Description { get; private set; }
        public bool IsRecurring { get; private set; }
        public string RecurrencePattern { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedDate { get; private set; }

        protected FilingDeadline() { }

        internal FilingDeadline(
            Guid assetId,
            string title,
            DateTime deadline,
            string description = null,
            bool isRecurring = false,
            string recurrencePattern = null)
            : base(GuidGenerator.Create())
        {
            AssetId = assetId;
            SetTitle(title);
            Deadline = deadline;
            Description = description;
            IsRecurring = isRecurring;
            RecurrencePattern = recurrencePattern;
            IsCompleted = false;
        }

        /// <summary>
        /// Marks the deadline as completed
        /// </summary>
        public void Complete(DateTime completionDate)
        {
            if (completionDate > Deadline)
            {
                throw new BusinessException("LegalCompliance:Validation:DateRange", 
                    "Completion date cannot be after the deadline");
            }
            
            IsCompleted = true;
            CompletedDate = completionDate;
        }

        /// <summary>
        /// Generates the next recurring deadline if this deadline is recurring
        /// </summary>
        public FilingDeadline GenerateNextDeadline()
        {
            if (!IsRecurring)
            {
                return null;
            }

            DateTime nextDeadline;
            
            // Simple recurrence pattern handling - in a real system, this would be more sophisticated
            if (RecurrencePattern == "Annual")
            {
                nextDeadline = Deadline.AddYears(1);
            }
            else if (RecurrencePattern == "Monthly")
            {
                nextDeadline = Deadline.AddMonths(1);
            }
            else if (RecurrencePattern == "Quarterly")
            {
                nextDeadline = Deadline.AddMonths(3);
            }
            else
            {
                // Default to annual if pattern is not recognized
                nextDeadline = Deadline.AddYears(1);
            }

            return new FilingDeadline(
                AssetId,
                Title,
                nextDeadline,
                Description,
                IsRecurring,
                RecurrencePattern);
        }

        /// <summary>
        /// Updates the deadline information
        /// </summary>
        public void Update(
            string title,
            DateTime deadline,
            string description = null,
            bool isRecurring = false,
            string recurrencePattern = null)
        {
            SetTitle(title);
            Deadline = deadline;
            Description = description;
            IsRecurring = isRecurring;
            RecurrencePattern = recurrencePattern;
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                LegalComplianceConsts.MinObligationTitleLength,
                LegalComplianceConsts.MaxObligationTitleLength
            );
        }
    }
}

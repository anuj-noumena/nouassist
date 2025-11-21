using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Entities
{
    /// <summary>
    /// Represents a reminder for a legal agreement
    /// </summary>
    public class AgreementReminder : Entity<Guid>
    {
        public Guid AgreementId { get; private set; }
        public string Title { get; private set; }
        public DateTime ReminderDate { get; private set; }
        public string Description { get; private set; }
        public int? DaysBeforeEvent { get; private set; }
        public bool IsRecurring { get; private set; }
        public string RecurrencePattern { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedDate { get; private set; }

        protected AgreementReminder() { }

        internal AgreementReminder(
            Guid agreementId,
            string title,
            DateTime reminderDate,
            string description = null,
            int? daysBeforeEvent = null,
            bool isRecurring = false,
            string recurrencePattern = null)
            : base(GuidGenerator.Create())
        {
            AgreementId = agreementId;
            SetTitle(title);
            ReminderDate = reminderDate;
            Description = description;
            DaysBeforeEvent = daysBeforeEvent;
            IsRecurring = isRecurring;
            RecurrencePattern = recurrencePattern;
            IsCompleted = false;
        }

        /// <summary>
        /// Marks the reminder as completed
        /// </summary>
        public void MarkCompleted(DateTime completedDate)
        {
            IsCompleted = true;
            CompletedDate = completedDate;
        }

        /// <summary>
        /// Updates the reminder information
        /// </summary>
        public void Update(
            string title,
            DateTime reminderDate,
            string description = null,
            int? daysBeforeEvent = null,
            bool isRecurring = false,
            string recurrencePattern = null)
        {
            SetTitle(title);
            ReminderDate = reminderDate;
            Description = description;
            DaysBeforeEvent = daysBeforeEvent;
            IsRecurring = isRecurring;
            RecurrencePattern = recurrencePattern;
        }

        /// <summary>
        /// Generates the next recurring reminder if this reminder is recurring
        /// </summary>
        public AgreementReminder GenerateNextReminder()
        {
            if (!IsRecurring)
            {
                return null;
            }

            DateTime nextReminderDate;
            
            // Simple recurrence pattern handling
            if (RecurrencePattern == "Annual")
            {
                nextReminderDate = ReminderDate.AddYears(1);
            }
            else if (RecurrencePattern == "Monthly")
            {
                nextReminderDate = ReminderDate.AddMonths(1);
            }
            else if (RecurrencePattern == "Quarterly")
            {
                nextReminderDate = ReminderDate.AddMonths(3);
            }
            else
            {
                // Default to annual if pattern is not recognized
                nextReminderDate = ReminderDate.AddYears(1);
            }

            return new AgreementReminder(
                AgreementId,
                Title,
                nextReminderDate,
                Description,
                DaysBeforeEvent,
                IsRecurring,
                RecurrencePattern);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: LegalComplianceConsts.MaxAgreementTitleLength
            );
        }
    }
}

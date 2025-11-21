using System;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Entities
{
    /// <summary>
    /// Represents a specific term within a legal agreement
    /// </summary>
    public class AgreementTerms : Entity<Guid>
    {
        public Guid AgreementId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsCritical { get; private set; }
        public DateTime? DeadlineDate { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedDate { get; private set; }

        protected AgreementTerms() { }

        internal AgreementTerms(
            Guid agreementId,
            string title,
            string description,
            bool isCritical = false,
            DateTime? deadlineDate = null)
            : base(GuidGenerator.Create())
        {
            AgreementId = agreementId;
            SetTitle(title);
            SetDescription(description);
            IsCritical = isCritical;
            DeadlineDate = deadlineDate;
            IsCompleted = false;
        }

        /// <summary>
        /// Marks the term as completed
        /// </summary>
        public void MarkCompleted(DateTime completedDate)
        {
            if (DeadlineDate.HasValue && completedDate > DeadlineDate.Value)
            {
                throw new BusinessException("LegalCompliance:Validation:DateRange", 
                    "Completion date cannot be after the deadline");
            }
            
            IsCompleted = true;
            CompletedDate = completedDate;
        }

        /// <summary>
        /// Updates the term information
        /// </summary>
        public void Update(
            string title,
            string description,
            bool isCritical = false,
            DateTime? deadlineDate = null)
        {
            SetTitle(title);
            SetDescription(description);
            IsCritical = isCritical;
            DeadlineDate = deadlineDate;
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: LegalComplianceConsts.MaxAgreementTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: LegalComplianceConsts.MaxAgreementDescriptionLength
            );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Events;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates
{
    /// <summary>
    /// Aggregate root representing an investor profile with relationships and interactions
    /// </summary>
    public class InvestorProfile : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string CompanyName { get; private set; }
        public string Title { get; private set; }
        public InvestorType Type { get; private set; }
        public InvestorStatus Status { get; private set; }
        public ContactDetails ContactDetails { get; private set; }
        public CommunicationPreferences CommunicationPreferences { get; private set; }
        public RelationshipScore RelationshipScore { get; private set; }
        public string Notes { get; private set; }
        public DateTime? LastInteractionDate { get; private set; }

        // Collections within aggregate boundary
        private readonly List<InvestorInteraction> _interactions;
        private readonly List<InvestmentRecord> _investments;

        public IReadOnlyCollection<InvestorInteraction> Interactions => _interactions.AsReadOnly();
        public IReadOnlyCollection<InvestmentRecord> Investments => _investments.AsReadOnly();

        public string FullName => $"{FirstName} {LastName}".Trim();

        protected InvestorProfile()
        {
            _interactions = new List<InvestorInteraction>();
            _investments = new List<InvestmentRecord>();
        }

        public static InvestorProfile Create(
            Guid id,
            string firstName,
            string lastName,
            string email,
            InvestorType type,
            string companyName = null,
            string title = null,
            Guid? tenantId = null)
        {
            var contactDetails = new ContactDetails(email);
            var communicationPreferences = new CommunicationPreferences(
                CommunicationChannel.Email,
                UpdateFrequency.Monthly
            );
            
            return new InvestorProfile(
                id,
                firstName,
                lastName,
                contactDetails,
                type,
                communicationPreferences,
                companyName,
                title,
                tenantId
            );
        }

        private InvestorProfile(
            Guid id,
            string firstName,
            string lastName,
            ContactDetails contactDetails,
            InvestorType type,
            CommunicationPreferences communicationPreferences,
            string companyName = null,
            string title = null,
            Guid? tenantId = null) : base(id)
        {
            SetName(firstName, lastName);
            ContactDetails = Check.NotNull(contactDetails, nameof(contactDetails));
            Type = type;
            Status = InvestorStatus.Prospect;
            CommunicationPreferences = Check.NotNull(communicationPreferences, nameof(communicationPreferences));
            RelationshipScore = new RelationshipScore(50, DateTime.UtcNow, "Initial score");
            CompanyName = Check.Length(companyName, nameof(companyName), IRMConsts.InvestorProfile.MaxCompanyNameLength);
            Title = title;
            TenantId = tenantId;
            _interactions = new List<InvestorInteraction>();
            _investments = new List<InvestmentRecord>();

            AddDistributedEvent(new InvestorProfileCreatedEto
            {
                Id = Id,
                InvestorName = FullName,
                Email = ContactDetails.Email,
                InvestorType = Type,
                Status = Status,
                TenantId = TenantId
            });
        }

        public void UpdateName(string firstName, string lastName)
        {
            SetName(firstName, lastName);
        }

        public void UpdateContactDetails(ContactDetails contactDetails)
        {
            ContactDetails = Check.NotNull(contactDetails, nameof(contactDetails));
        }

        public void UpdateCommunicationPreferences(CommunicationPreferences preferences)
        {
            CommunicationPreferences = Check.NotNull(preferences, nameof(preferences));
        }

        public void UpdateCompanyInfo(string companyName, string title = null)
        {
            CompanyName = Check.Length(companyName, nameof(companyName), IRMConsts.InvestorProfile.MaxCompanyNameLength);
            Title = title;
        }

        public void UpdateStatus(InvestorStatus status)
        {
            if (Status == status)
                return;

            // Business rule: Cannot move from Exited back to Active without proper validation
            if (Status == InvestorStatus.Exited && status == InvestorStatus.Active)
            {
                throw new BusinessException("IRM:ErrorMessage:InvalidOperation", 
                    "Cannot reactivate an exited investor without proper validation");
            }

            Status = status;
        }

        public void UpdateRelationshipScore(int score, string notes = null)
        {
            RelationshipScore = RelationshipScore.UpdateScore(score, notes);
        }

        public void UpdateNotes(string notes)
        {
            Notes = Check.Length(notes, nameof(notes), IRMConsts.InvestorProfile.MaxNotesLength);
        }

        public void AddInteraction(
            InteractionType type,
            string subject,
            DateTime interactionDate,
            string notes = null,
            string outcome = null,
            Guid? relatedEntityId = null,
            string relatedEntityType = null)
        {
            var interaction = new InvestorInteraction(
                GuidGenerator.Create(),
                Id,
                type,
                subject,
                interactionDate,
                notes,
                outcome,
                relatedEntityId,
                relatedEntityType,
                TenantId
            );

            _interactions.Add(interaction);
            LastInteractionDate = interactionDate;
        }

        public void AddInvestment(CommitmentDetails commitment, string documentReference = null)
        {
            var investment = new InvestmentRecord(
                GuidGenerator.Create(),
                Id,
                commitment,
                documentReference,
                TenantId
            );

            _investments.Add(investment);
            
            // Automatically update status to Active when investment is added
            if (Status == InvestorStatus.Prospect)
            {
                Status = InvestorStatus.Active;
            }
        }

        public decimal GetTotalInvestedAmount()
        {
            return _investments.Where(i => i.IsActive).Sum(i => i.Commitment.InvestedAmount);
        }

        public decimal GetTotalCommittedAmount()
        {
            return _investments.Where(i => i.IsActive).Sum(i => i.Commitment.CommittedAmount);
        }

        public decimal GetTotalOwnershipPercentage()
        {
            return _investments.Where(i => i.IsActive).Sum(i => i.Commitment.OwnershipPercentage);
        }

        public int GetInteractionCount(TimeSpan period)
        {
            var cutoffDate = DateTime.UtcNow.Subtract(period);
            return _interactions.Count(i => i.InteractionDate >= cutoffDate);
        }

        private void SetName(string firstName, string lastName)
        {
            FirstName = Check.NotNullOrWhiteSpace(
                firstName,
                nameof(firstName),
                maxLength: IRMConsts.InvestorProfile.MaxNameLength
            );
            LastName = Check.NotNullOrWhiteSpace(
                lastName,
                nameof(lastName),
                maxLength: IRMConsts.InvestorProfile.MaxNameLength
            );
        }
    }
}

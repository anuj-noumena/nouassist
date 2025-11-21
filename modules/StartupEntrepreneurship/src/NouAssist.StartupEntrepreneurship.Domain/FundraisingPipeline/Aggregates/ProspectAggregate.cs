using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Events;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates
{
    /// <summary>
    /// Aggregate root representing a fundraising prospect (potential investor)
    /// </summary>
    public class Prospect : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Company { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public PipelineStatus PipelineStatus { get; private set; }
        public ProspectScore Score { get; private set; }
        public OpportunityForecastValue OpportunityForecast { get; private set; }
        public Guid? FundraisingRoundId { get; private set; }
        
        private readonly List<InteractionLog> _interactions;
        public IReadOnlyCollection<InteractionLog> Interactions => _interactions.AsReadOnly();
        
        protected Prospect()
        {
            _interactions = new List<InteractionLog>();
        }
        
        /// <summary>
        /// Factory method to create a new prospect
        /// </summary>
        public static Prospect Create(
            Guid id,
            string name,
            string company,
            string title,
            ContactInfo contactInfo,
            string description = null,
            Guid? tenantId = null)
        {
            return new Prospect(id, name, company, title, contactInfo, description, tenantId);
        }
        
        private Prospect(
            Guid id,
            string name,
            string company,
            string title,
            ContactInfo contactInfo,
            string description = null,
            Guid? tenantId = null) : base(id)
        {
            SetName(name);
            SetCompany(company);
            SetTitle(title);
            Description = description;
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
            TenantId = tenantId;
            
            // Initialize with default values
            PipelineStatus = new PipelineStatus(ProspectStatus.New, DateTime.Now);
            Score = new ProspectScore(0, 0, 0); // Initial scores are zero
            _interactions = new List<InteractionLog>();
            
            AddDistributedEvent(new ProspectCreatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Name,
                Company = Company,
                Status = PipelineStatus.Status
            });
        }
        
        /// <summary>
        /// Updates the prospect's basic information
        /// </summary>
        public void UpdateBasicInfo(
            string name,
            string company,
            string title,
            string description,
            ContactInfo contactInfo)
        {
            SetName(name);
            SetCompany(company);
            SetTitle(title);
            Description = description;
            ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        }
        
        /// <summary>
        /// Updates the prospect's status in the pipeline
        /// </summary>
        public void UpdateStatus(ProspectStatus newStatus, DateTime changeTime)
        {
            var oldStatus = PipelineStatus.Status;
            PipelineStatus.UpdateStatus(newStatus, changeTime);
            
            AddDistributedEvent(new ProspectStatusChangedEto
            {
                Id = Id,
                TenantId = TenantId,
                OldStatus = oldStatus,
                NewStatus = newStatus
            });
        }
        
        /// <summary>
        /// Updates the prospect's scoring information
        /// </summary>
        public void UpdateScore(int engagementScore, int fitScore, int timelinessScore)
        {
            Score = new ProspectScore(engagementScore, fitScore, timelinessScore);
        }
        
        /// <summary>
        /// Updates the opportunity forecast for this prospect
        /// </summary>
        public void UpdateOpportunityForecast(
            decimal minAmount,
            decimal maxAmount,
            decimal expectedAmount,
            double probability,
            DateTime forecastedClosingDate)
        {
            OpportunityForecast = new OpportunityForecastValue(
                minAmount,
                maxAmount,
                expectedAmount,
                probability,
                forecastedClosingDate);
        }
        
        /// <summary>
        /// Adds a new interaction with this prospect
        /// </summary>
        public InteractionLog AddInteraction(
            InteractionType type,
            string summary,
            DateTime interactionDate,
            string notes = null)
        {
            var interaction = new InteractionLog(
                new (),
                Id,
                type,
                summary,
                interactionDate,
                notes);
            
            _interactions.Add(interaction);
            return interaction;
        }
        
        /// <summary>
        /// Assigns the prospect to a fundraising round
        /// </summary>
        public void AssignToRound(Guid fundraisingRoundId)
        {
            FundraisingRoundId = fundraisingRoundId;
        }
        
        /// <summary>
        /// Removes the prospect from its current fundraising round
        /// </summary>
        public void RemoveFromRound()
        {
            FundraisingRoundId = null;
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: FundraisingPipelineConsts.Prospect.MaxNameLength);
        }
        
        private void SetCompany(string company)
        {
            Company = Check.NotNullOrWhiteSpace(
                company,
                nameof(company),
                maxLength: FundraisingPipelineConsts.Prospect.MaxCompanyLength);
        }
        
        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: FundraisingPipelineConsts.Prospect.MaxTitleLength);
        }
    }
}

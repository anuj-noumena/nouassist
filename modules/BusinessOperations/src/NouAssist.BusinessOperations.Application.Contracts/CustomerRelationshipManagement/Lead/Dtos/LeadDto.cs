using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead.Dtos
{
    /// <summary>
    /// Data transfer object for Lead entity
    /// </summary>
    public class LeadDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Full name of the lead
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Current status in the sales process
        /// </summary>
        public LeadStatus Status { get; set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        public ContactInfoDto ContactInfo { get; set; }
        
        /// <summary>
        /// Additional lead details
        /// </summary>
        public LeadDetailsDto Details { get; set; }
        
        /// <summary>
        /// Lead score for prioritization
        /// </summary>
        public int Score { get; set; }
        
        /// <summary>
        /// Tags for categorization
        /// </summary>
        public List<string> Tags { get; set; }
        
        /// <summary>
        /// ID of the contact if this lead was converted
        /// </summary>
        public Guid? ConvertedToContactId { get; set; }
        
        /// <summary>
        /// When the lead was converted to a contact
        /// </summary>
        public DateTime? ConversionDate { get; set; }
    }

    /// <summary>
    /// Data transfer object for ContactInfo value object
    /// </summary>
    public class ContactInfoDto
    {
        /// <summary>
        /// Primary email address
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Primary phone number
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// Alternative phone number
        /// </summary>
        public string AlternativePhone { get; set; }
        
        /// <summary>
        /// Website URL
        /// </summary>
        public string Website { get; set; }
        
        /// <summary>
        /// Street address
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// State or province
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// Postal code
        /// </summary>
        public string PostalCode { get; set; }
        
        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }
    }

    /// <summary>
    /// Data transfer object for LeadDetails value object
    /// </summary>
    public class LeadDetailsDto
    {
        /// <summary>
        /// Job title of the lead
        /// </summary>
        public string JobTitle { get; set; }
        
        /// <summary>
        /// Company name associated with the lead
        /// </summary>
        public string CompanyName { get; set; }
        
        /// <summary>
        /// Industry the lead works in
        /// </summary>
        public string Industry { get; set; }
        
        /// <summary>
        /// Lead source
        /// </summary>
        public LeadSource Source { get; set; }
        
        /// <summary>
        /// Referral source if applicable
        /// </summary>
        public string ReferralSource { get; set; }
        
        /// <summary>
        /// Campaign identifier if lead came from a campaign
        /// </summary>
        public string CampaignId { get; set; }
        
        /// <summary>
        /// Additional notes about the lead
        /// </summary>
        public string Notes { get; set; }
        
        /// <summary>
        /// Estimated budget
        /// </summary>
        public decimal? EstimatedBudget { get; set; }
    }

    /// <summary>
    /// Data transfer object for lead source tracking
    /// </summary>
    public class LeadSourceRecordDto
    {
        /// <summary>
        /// Source of the lead
        /// </summary>
        public LeadSource Source { get; set; }
        
        /// <summary>
        /// Specific campaign ID if from a campaign
        /// </summary>
        public string CampaignId { get; set; }
        
        /// <summary>
        /// UTM Source parameter if available
        /// </summary>
        public string UtmSource { get; set; }
        
        /// <summary>
        /// UTM Medium parameter if available
        /// </summary>
        public string UtmMedium { get; set; }
        
        /// <summary>
        /// UTM Campaign parameter if available
        /// </summary>
        public string UtmCampaign { get; set; }
        
        /// <summary>
        /// UTM Term parameter if available
        /// </summary>
        public string UtmTerm { get; set; }
        
        /// <summary>
        /// UTM Content parameter if available
        /// </summary>
        public string UtmContent { get; set; }
        
        /// <summary>
        /// Referrer URL if available
        /// </summary>
        public string ReferrerUrl { get; set; }
        
        /// <summary>
        /// Landing page URL
        /// </summary>
        public string LandingPageUrl { get; set; }
        
        /// <summary>
        /// When the lead was captured
        /// </summary>
        public DateTime CaptureTime { get; set; }
    }

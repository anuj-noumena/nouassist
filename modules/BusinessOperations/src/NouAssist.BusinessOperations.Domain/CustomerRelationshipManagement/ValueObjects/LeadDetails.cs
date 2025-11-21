using System.Collections.Generic;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Contains detailed information about a lead
    /// </summary>
    public class LeadDetails : ValueObject
    {
        /// <summary>
        /// Job title of the lead
        /// </summary>
        public string JobTitle { get; private set; }
        
        /// <summary>
        /// Company name associated with the lead
        /// </summary>
        public string CompanyName { get; private set; }
        
        /// <summary>
        /// Industry the lead works in
        /// </summary>
        public string Industry { get; private set; }
        
        /// <summary>
        /// Lead source
        /// </summary>
        public LeadSource Source { get; private set; }
        
        /// <summary>
        /// Referral source if applicable
        /// </summary>
        public string ReferralSource { get; private set; }
        
        /// <summary>
        /// Campaign identifier if lead came from a campaign
        /// </summary>
        public string CampaignId { get; private set; }
        
        /// <summary>
        /// Additional notes about the lead
        /// </summary>
        public string Notes { get; private set; }
        
        /// <summary>
        /// Estimated budget
        /// </summary>
        public decimal? EstimatedBudget { get; private set; }

        protected LeadDetails()
        {
        }

        public LeadDetails(
            string jobTitle,
            string companyName,
            string industry,
            LeadSource source,
            string referralSource = null,
            string campaignId = null,
            string notes = null,
            decimal? estimatedBudget = null)
        {
            JobTitle = jobTitle;
            CompanyName = companyName;
            Industry = industry;
            Source = source;
            ReferralSource = referralSource;
            CampaignId = campaignId;
            Notes = notes;
            EstimatedBudget = estimatedBudget;
        }

        public LeadDetails Update(
            string jobTitle = null,
            string companyName = null,
            string industry = null,
            LeadSource? source = null,
            string referralSource = null,
            string campaignId = null,
            string notes = null,
            decimal? estimatedBudget = null)
        {
            if (jobTitle != null)
            {
                JobTitle = jobTitle;
            }

            if (companyName != null)
            {
                CompanyName = companyName;
            }

            if (industry != null)
            {
                Industry = industry;
            }

            if (source.HasValue)
            {
                Source = source.Value;
            }

            if (referralSource != null)
            {
                ReferralSource = referralSource;
            }

            if (campaignId != null)
            {
                CampaignId = campaignId;
            }

            if (notes != null)
            {
                Notes = notes;
            }

            if (estimatedBudget.HasValue)
            {
                EstimatedBudget = estimatedBudget.Value;
            }

            return this;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return JobTitle ?? string.Empty;
            yield return CompanyName ?? string.Empty;
            yield return Industry ?? string.Empty;
            yield return Source;
            yield return ReferralSource ?? string.Empty;
            yield return CampaignId ?? string.Empty;
            yield return Notes ?? string.Empty;
            yield return EstimatedBudget ?? 0m;
        }
    }
}

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities
{
    /// <summary>
    /// Tracks the source of a lead and associated metrics
    /// </summary>
    public class LeadSourceRecord : Entity
    {
        /// <summary>
        /// The lead this record is associated with
        /// </summary>
        public Guid LeadId { get; private set; }
        
        /// <summary>
        /// Source of the lead
        /// </summary>
        public LeadSource Source { get; private set; }
        
        /// <summary>
        /// Specific campaign ID if from a campaign
        /// </summary>
        public string CampaignId { get; private set; }
        
        /// <summary>
        /// UTM Source parameter if available
        /// </summary>
        public string UtmSource { get; private set; }
        
        /// <summary>
        /// UTM Medium parameter if available
        /// </summary>
        public string UtmMedium { get; private set; }
        
        /// <summary>
        /// UTM Campaign parameter if available
        /// </summary>
        public string UtmCampaign { get; private set; }
        
        /// <summary>
        /// UTM Term parameter if available
        /// </summary>
        public string UtmTerm { get; private set; }
        
        /// <summary>
        /// UTM Content parameter if available
        /// </summary>
        public string UtmContent { get; private set; }
        
        /// <summary>
        /// Referrer URL if available
        /// </summary>
        public string ReferrerUrl { get; private set; }
        
        /// <summary>
        /// Landing page URL
        /// </summary>
        public string LandingPageUrl { get; private set; }
        
        /// <summary>
        /// When the lead was captured
        /// </summary>
        public DateTime CaptureTime { get; private set; }

        protected LeadSourceRecord()
        {
        }

        internal LeadSourceRecord(
            Guid leadId,
            LeadSource source,
            DateTime captureTime,
            string campaignId = null,
            string utmSource = null,
            string utmMedium = null,
            string utmCampaign = null,
            string utmTerm = null,
            string utmContent = null,
            string referrerUrl = null,
            string landingPageUrl = null)
        {
            LeadId = leadId;
            Source = source;
            CaptureTime = captureTime;
            CampaignId = campaignId;
            UtmSource = utmSource;
            UtmMedium = utmMedium;
            UtmCampaign = utmCampaign;
            UtmTerm = utmTerm;
            UtmContent = utmContent;
            ReferrerUrl = referrerUrl;
            LandingPageUrl = landingPageUrl;
        }

        public override object[] GetKeys()
        {
            return new object[] { LeadId };
        }
    }
}

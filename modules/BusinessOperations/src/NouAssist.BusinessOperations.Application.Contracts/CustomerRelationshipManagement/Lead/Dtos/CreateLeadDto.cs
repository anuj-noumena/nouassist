/// <summary>
    /// Data transfer object for creating a lead
    /// </summary>
    public class CreateLeadDto
    {
        /// <summary>
        /// Full name of the lead
        /// </summary>
        [Required]
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string Name { get; set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        [Required]
        public CreateContactInfoDto ContactInfo { get; set; }
        
        /// <summary>
        /// Additional lead details
        /// </summary>
        [Required]
        public CreateLeadDetailsDto Details { get; set; }
        
        /// <summary>
        /// Initial lead score (optional)
        /// </summary>
        public int? Score { get; set; }
        
        /// <summary>
        /// Initial tags (optional)
        /// </summary>
        public List<string> Tags { get; set; }
        
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
    }

    /// <summary>
    /// Data transfer object for creating contact information
    /// </summary>
    public class CreateContactInfoDto
    {
        /// <summary>
        /// Primary email address
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(CRMConsts.MaxLengths.Email)]
        public string Email { get; set; }
        
        /// <summary>
        /// Primary phone number
        /// </summary>
        [Required]
        [StringLength(CRMConsts.MaxLengths.Phone)]
        public string Phone { get; set; }
        
        /// <summary>
        /// Alternative phone number
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Phone)]
        public string AlternativePhone { get; set; }
        
        /// <summary>
        /// Website URL
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Url)]
        public string Website { get; set; }
        
        /// <summary>
        /// Street address
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Address)]
        public string Address { get; set; }
        
        /// <summary>
        /// City
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string City { get; set; }
        
        /// <summary>
        /// State or province
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string State { get; set; }
        
        /// <summary>
        /// Postal code
        /// </summary>
        [StringLength(50)]
        public string PostalCode { get; set; }
        
        /// <summary>
        /// Country
        /// </summary>
        [StringLength(50)]
        public string Country { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating lead details
    /// </summary>
    public class CreateLeadDetailsDto
    {
        /// <summary>
        /// Job title of the lead
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Title)]
        public string JobTitle { get; set; }
        
        /// <summary>
        /// Company name associated with the lead
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string CompanyName { get; set; }
        
        /// <summary>
        /// Industry the lead works in
        /// </summary>
        [StringLength(100)]
        public string Industry { get; set; }
        
        /// <summary>
        /// Lead source
        /// </summary>
        [Required]
        public LeadSource Source { get; set; }
        
        /// <summary>
        /// Referral source if applicable
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string ReferralSource { get; set; }
        
        /// <summary>
        /// Campaign identifier if lead came from a campaign
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Code)]
        public string CampaignId { get; set; }
        
        /// <summary>
        /// Additional notes about the lead
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Note)]
        public string Notes { get; set; }
        
        /// <summary>
        /// Estimated budget
        /// </summary>
        public decimal? EstimatedBudget { get; set; }
    }

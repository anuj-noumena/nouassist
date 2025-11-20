/// <summary>
    /// Data transfer object for updating a lead
    /// </summary>
    public class UpdateLeadDto
    {
        /// <summary>
        /// Full name of the lead
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string Name { get; set; }
        
        /// <summary>
        /// Contact information
        /// </summary>
        public UpdateContactInfoDto ContactInfo { get; set; }
        
        /// <summary>
        /// Additional lead details
        /// </summary>
        public UpdateLeadDetailsDto Details { get; set; }
        
        /// <summary>
        /// Lead score for prioritization
        /// </summary>
        public int? Score { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating contact information
    /// </summary>
    public class UpdateContactInfoDto
    {
        /// <summary>
        /// Primary email address
        /// </summary>
        [EmailAddress]
        [StringLength(CRMConsts.MaxLengths.Email)]
        public string Email { get; set; }
        
        /// <summary>
        /// Primary phone number
        /// </summary>
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
    /// Data transfer object for updating lead details
    /// </summary>
    public class UpdateLeadDetailsDto
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
        public LeadSource? Source { get; set; }
        
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

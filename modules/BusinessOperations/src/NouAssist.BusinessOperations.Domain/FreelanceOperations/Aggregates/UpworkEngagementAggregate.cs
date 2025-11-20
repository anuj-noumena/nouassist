namespace NouAssist.BusinessOperations.FreelanceOperations.Aggregates
{
    /// <summary>
    /// Aggregate root representing an Upwork engagement
    /// </summary>
    public class UpworkEngagement : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Upwork job ID
        /// </summary>
        public string UpworkJobId { get; private set; }
        
        /// <summary>
        /// Upwork contract ID
        /// </summary>
        public string UpworkContractId { get; private set; }
        
        /// <summary>
        /// Title of the job
        /// </summary>
        public string JobTitle { get; private set; }
        
        /// <summary>
        /// Client name on Upwork
        /// </summary>
        public string ClientName { get; private set; }
        
        /// <summary>
        /// Link to the job on Upwork
        /// </summary>
        public string JobLink { get; private set; }
        
        /// <summary>
        /// Type of contract (Hourly, Fixed)
        /// </summary>
        public string ContractType { get; private set; }
        
        /// <summary>
        /// Status of the engagement
        /// </summary>
        public string Status { get; private set; }
        
        /// <summary>
        /// Start date of the engagement
        /// </summary>
        public DateTime? StartDate { get; private set; }
        
        /// <summary>
        /// End date of the engagement
        /// </summary>
        public DateTime? EndDate { get; private set; }
        
        /// <summary>
        /// ID of the corresponding project in the system, if linked
        /// </summary>
        public Guid? LinkedProjectId { get; private set; }
        
        /// <summary>
        /// ID of the corresponding client in the system, if linked
        /// </summary>
        public Guid? LinkedClientId { get; private set; }

        protected UpworkEngagement()
        {
        }

        /// <summary>
        /// Creates a new Upwork engagement
        /// </summary>
        public static UpworkEngagement Create(
            Guid id,
            string upworkJobId,
            string jobTitle,
            string clientName,
            string jobLink,
            string contractType,
            Guid? tenantId = null)
        {
            return new UpworkEngagement(
                id,
                upworkJobId,
                jobTitle,
                clientName,
                jobLink,
                contractType,
                tenantId);
        }

        private UpworkEngagement(
            Guid id,
            string upworkJobId,
            string jobTitle,
            string clientName,
            string jobLink,
            string contractType,
            Guid? tenantId = null) : base(id)
        {
            UpworkJobId = Check.NotNullOrWhiteSpace(
                upworkJobId, 
                nameof(upworkJobId), 
                maxLength: FreelanceOperationsConsts.Upwork.MaxJobIdLength
            );
            
            JobTitle = Check.NotNullOrWhiteSpace(jobTitle, nameof(jobTitle));
            ClientName = Check.NotNullOrWhiteSpace(clientName, nameof(clientName));
            JobLink = Check.NotNullOrWhiteSpace(jobLink, nameof(jobLink));
            ContractType = Check.NotNullOrWhiteSpace(contractType, nameof(contractType));
            Status = "Pending";
            TenantId = tenantId;
        }

        /// <summary>
        /// Update the contract information for this engagement
        /// </summary>
        public void SetContract(string upworkContractId, DateTime startDate)
        {
            UpworkContractId = Check.NotNullOrWhiteSpace(
                upworkContractId, 
                nameof(upworkContractId), 
                maxLength: FreelanceOperationsConsts.Upwork.MaxContractIdLength
            );
            
            StartDate = startDate;
            Status = "Active";
        }

        /// <summary>
        /// Update the status of this engagement
        /// </summary>
        public void UpdateStatus(string status, DateTime? endDate = null)
        {
            Status = Check.NotNullOrWhiteSpace(status, nameof(status));
            
            if (status == "Completed" || status == "Ended")
            {
                EndDate = endDate ?? DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Link this Upwork engagement to a project and client in the system
        /// </summary>
        public void LinkToSystem(Guid? projectId, Guid? clientId)
        {
            LinkedProjectId = projectId;
            LinkedClientId = clientId;
        }

        /// <summary>
        /// Update the basic information for this engagement
        /// </summary>
        public void UpdateBasicInfo(
            string jobTitle,
            string clientName,
            string jobLink,
            string contractType)
        {
            JobTitle = Check.NotNullOrWhiteSpace(jobTitle, nameof(jobTitle));
            ClientName = Check.NotNullOrWhiteSpace(clientName, nameof(clientName));
            JobLink = Check.NotNullOrWhiteSpace(jobLink, nameof(jobLink));
            ContractType = Check.NotNullOrWhiteSpace(contractType, nameof(contractType));
        }

        /// <summary>
        /// Check if the engagement is active
        /// </summary>
        public bool IsActive()
        {
            return Status == "Active" || Status == "In Progress";
        }

        /// <summary>
        /// Check if the engagement is completed
        /// </summary>
        public bool IsCompleted()
        {
            return Status == "Completed" || Status == "Ended";
        }

        /// <summary>
        /// Check if the engagement is pending
        /// </summary>
        public bool IsPending()
        {
            return Status == "Pending" || Status == "Proposal Sent";
        }
    }
}

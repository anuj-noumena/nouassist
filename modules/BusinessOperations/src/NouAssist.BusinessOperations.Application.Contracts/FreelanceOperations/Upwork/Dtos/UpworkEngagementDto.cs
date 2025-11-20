using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Upwork.Dtos
{
    public class UpworkEngagementDto : AuditedEntityDto<Guid>
    {
        public string UpworkJobId { get; set; }
        public string UpworkContractId { get; set; }
        public string JobTitle { get; set; }
        public string ClientName { get; set; }
        public string JobLink { get; set; }
        public string ContractType { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? LinkedProjectId { get; set; }
        public string LinkedProjectName { get; set; }
        public Guid? LinkedClientId { get; set; }
        public string LinkedClientName { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPending { get; set; }
    }
}

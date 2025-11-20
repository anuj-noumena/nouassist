using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos
{
    public class FreelanceProjectDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public ProjectStatus Status { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public BillingType BillingType { get; set; }
        public decimal? HourlyRate { get; set; }
        public string Currency { get; set; }
        public decimal? FixedPrice { get; set; }
        public string ScopeDescription { get; set; }
        public List<string> ScopeDeliverables { get; set; }
        public bool ScopeIsFlexible { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal TotalHours { get; set; }
        public decimal TotalBillableHours { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid? AcceptedProposalId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos
{
    public class ProposalDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ProposalStatus Status { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? ViewedDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public Guid? CurrentVersionId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateVersion { get; set; }
        public bool IsExpired { get; set; }
        public string FormattedStatus { get; set; }
        public ProposalVersionDto CurrentVersion { get; set; }
    }
}

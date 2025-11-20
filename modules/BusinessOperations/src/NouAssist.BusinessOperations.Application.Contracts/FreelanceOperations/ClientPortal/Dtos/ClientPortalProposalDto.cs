using System;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;

namespace NouAssist.BusinessOperations.FreelanceOperations.ClientPortal.Dtos
{
    public class ClientPortalProposalDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ProposalStatus Status { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsExpired { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string DocumentId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using NouAssist.IdentityAccess.Contacts.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IdentityAccess.Contacts.Clients.Dtos
{
    public class ClientDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public string LegalName { get; set; }
        public string Industry { get; set; }
        public string Website { get; set; }
        public Guid? PrimaryContactId { get; set; }
        public string PrimaryContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ClientStatus Status { get; set; }
        public DateTime? ClientSince { get; set; }
        public decimal? LifetimeValue { get; set; }
        public decimal? CurrentBalance { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InactiveSince { get; set; }
        public DateTime? LastContactedAt { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public List<string> Tags { get; set; }

        // Computed properties
        public bool NeedsReview { get; set; }
    }
}

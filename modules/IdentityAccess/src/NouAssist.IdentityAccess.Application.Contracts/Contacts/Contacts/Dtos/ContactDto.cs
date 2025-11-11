using System;
using System.Collections.Generic;
using NouAssist.Contacts.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Contacts.Dtos
{
    public class ContactDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }
        public string DisplayName { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public string PrimaryEmail { get; set; }
        public string PrimaryPhone { get; set; }
        public string Website { get; set; }
        public ContactType ContactType { get; set; }
        public ContactRelationship Relationship { get; set; }
        public double Importance { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public DateTime? LastContactedAt { get; set; }
        public DateTime? LastMetAt { get; set; }
        public int ContactFrequencyDays { get; set; }
        public DateTime? NextFollowupDate { get; set; }
        public List<ContactMethodDto> ContactMethods { get; set; }
        public List<string> Tags { get; set; }
        public List<SocialProfileDto> SocialProfiles { get; set; }

        // Computed properties
        public bool HasEmail { get; set; }
        public bool HasPhone { get; set; }
        public bool NeedsFollowup { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IdentityAccess.Contacts.Enums;

namespace NouAssist.IdentityAccess.Contacts.Contacts.Dtos
{
    public class UpdateContactDto
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string OrganizationName { get; set; }

        [StringLength(100)]
        public string JobTitle { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        public Guid? ClientId { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string PrimaryEmail { get; set; }

        [StringLength(50)]
        public string PrimaryPhone { get; set; }

        [StringLength(200)]
        public string Website { get; set; }

        public ContactType? ContactType { get; set; }

        public ContactRelationship? Relationship { get; set; }

        public double Importance { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        public int ContactFrequencyDays { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IdentityAccess.Contacts.Clients.Dtos
{
    public class UpdateClientDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string LegalName { get; set; }

        [StringLength(100)]
        public string Industry { get; set; }

        [StringLength(200)]
        public string Website { get; set; }

        public Guid? PrimaryContactId { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        public DateTime? NextReviewDate { get; set; }
    }
}

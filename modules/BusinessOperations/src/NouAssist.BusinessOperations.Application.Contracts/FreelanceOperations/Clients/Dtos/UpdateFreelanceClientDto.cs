using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;

namespace NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos
{
    public class UpdateFreelanceClientDto
    {
        [Required]
        [StringLength(FreelanceOperationsConsts.Client.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(FreelanceOperationsConsts.Client.MaxCompanyNameLength)]
        public string CompanyName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(FreelanceOperationsConsts.Client.MaxEmailLength)]
        public string Email { get; set; }

        [StringLength(FreelanceOperationsConsts.Client.MaxPhoneLength)]
        public string Phone { get; set; }

        public string Website { get; set; }

        public string Address { get; set; }

        [StringLength(FreelanceOperationsConsts.Client.MaxNotesLength)]
        public string Notes { get; set; }
    }
}

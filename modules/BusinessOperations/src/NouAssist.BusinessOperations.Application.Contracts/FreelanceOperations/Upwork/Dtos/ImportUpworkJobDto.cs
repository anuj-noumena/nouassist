using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;

namespace NouAssist.BusinessOperations.FreelanceOperations.Upwork.Dtos
{
    public class ImportUpworkJobDto
    {
        [Required]
        [StringLength(FreelanceOperationsConsts.Upwork.MaxJobIdLength)]
        public string UpworkJobId { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string JobLink { get; set; }

        [Required]
        public string ContractType { get; set; }
    }
}

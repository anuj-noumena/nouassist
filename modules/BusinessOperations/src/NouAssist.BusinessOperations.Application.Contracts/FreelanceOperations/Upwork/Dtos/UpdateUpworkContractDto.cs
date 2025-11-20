using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;

namespace NouAssist.BusinessOperations.FreelanceOperations.Upwork.Dtos
{
    public class UpdateUpworkContractDto
    {
        [Required]
        [StringLength(FreelanceOperationsConsts.Upwork.MaxContractIdLength)]
        public string UpworkContractId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
    }
}

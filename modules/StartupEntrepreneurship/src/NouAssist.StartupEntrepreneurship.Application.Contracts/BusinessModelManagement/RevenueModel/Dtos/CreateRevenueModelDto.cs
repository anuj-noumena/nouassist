using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class CreateRevenueModelDto
    {
        [Required]
        [StringLength(BusinessModelManagementConsts.RevenueModel.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(BusinessModelManagementConsts.RevenueModel.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public RevenueModelType Type { get; set; }
    }
}

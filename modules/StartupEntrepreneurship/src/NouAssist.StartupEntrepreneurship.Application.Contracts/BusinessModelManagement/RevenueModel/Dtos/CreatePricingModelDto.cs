using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class CreatePricingModelDto
    {
        [Required]
        [StringLength(BusinessModelManagementConsts.RevenueModel.MaxPricingModelNameLength)]
        public string Name { get; set; }

        [Required]
        public PricingStrategy Strategy { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}

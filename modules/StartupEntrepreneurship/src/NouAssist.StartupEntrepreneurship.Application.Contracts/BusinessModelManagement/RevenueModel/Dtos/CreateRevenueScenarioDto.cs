using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class CreateRevenueScenarioDto
    {
        [Required]
        [StringLength(BusinessModelManagementConsts.RevenueModel.MaxScenarioNameLength)]
        public string Name { get; set; }

        [StringLength(BusinessModelManagementConsts.RevenueModel.MaxScenarioDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public ScenarioParametersDto Parameters { get; set; }
    }
}

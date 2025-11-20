using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class CreateHypothesisDto
    {
        [Required]
        public CanvasComponentType ComponentType { get; set; }

        [Required]
        [StringLength(BusinessModelManagementConsts.Hypothesis.MaxAssumptionLength)]
        public string Assumption { get; set; }
    }
}

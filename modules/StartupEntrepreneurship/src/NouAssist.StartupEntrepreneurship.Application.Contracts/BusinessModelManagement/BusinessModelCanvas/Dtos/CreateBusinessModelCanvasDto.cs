using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class CreateBusinessModelCanvasDto
    {
        [Required]
        [StringLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}

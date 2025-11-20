using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class UpdateCanvasComponentDto
    {
        [Required]
        public CanvasComponentType ComponentType { get; set; }

        [Required]
        public ComponentContentDto Content { get; set; }
    }
}

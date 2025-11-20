using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class CreateCanvasVersionDto
    {
        [Required]
        [StringLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxVersionNameLength)]
        public string VersionName { get; set; }

        [StringLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxVersionNotesLength)]
        public string Notes { get; set; }
    }
}

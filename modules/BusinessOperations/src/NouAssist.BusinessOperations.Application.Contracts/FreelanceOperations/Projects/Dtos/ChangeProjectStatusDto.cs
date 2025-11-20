using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos
{
    public class ChangeProjectStatusDto
    {
        [Required]
        public ProjectStatus NewStatus { get; set; }
    }
}

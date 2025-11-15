using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class CompleteTaskDto
    {
        [Required]
        [StringLength(200)]
        public string Resolution { get; set; }

        [StringLength(2000)]
        public string ResolutionNote { get; set; }
    }
}
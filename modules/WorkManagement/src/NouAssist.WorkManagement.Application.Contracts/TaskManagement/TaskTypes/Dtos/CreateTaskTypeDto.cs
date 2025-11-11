using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.TaskManagement.TaskTypes.Dtos
{
    public class CreateTaskTypeDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
        [StringLength(50)]
        public string Color { get; set; }
    }

    public class UpdateTaskTypeDto
    {
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
        [StringLength(50)]
        public string Color { get; set; }
    }
}

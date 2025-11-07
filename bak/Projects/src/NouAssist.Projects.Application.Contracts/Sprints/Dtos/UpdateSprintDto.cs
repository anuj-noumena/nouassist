using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Projects.Sprints.Dtos
{
    public class UpdateSprintDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Goal { get; set; }

        [StringLength(500)]
        public string ReleaseTheme { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}

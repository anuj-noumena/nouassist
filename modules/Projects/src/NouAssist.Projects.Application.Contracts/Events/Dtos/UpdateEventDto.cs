using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Projects.Events.Dtos
{
    public class UpdateEventDto
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsAllDay { get; set; }

        [StringLength(500)]
        public string Location { get; set; }
    }
}

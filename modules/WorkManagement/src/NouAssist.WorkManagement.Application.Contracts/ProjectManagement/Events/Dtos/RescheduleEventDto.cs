using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.ProjectManagement.Events.Dtos
{
    public class RescheduleEventDto
    {
        [Required]
        public DateTime NewEventDate { get; set; }

        public DateTime? NewEndDate { get; set; }
    }
}

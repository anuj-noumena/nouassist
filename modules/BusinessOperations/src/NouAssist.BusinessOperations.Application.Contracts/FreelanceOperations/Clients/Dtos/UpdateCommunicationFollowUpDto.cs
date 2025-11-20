using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos
{
    public class UpdateCommunicationFollowUpDto
    {
        [Required]
        public bool RequiresFollowUp { get; set; }

        public DateTime? FollowUpDate { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos
{
    public class CreateCommunicationRecordDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public Guid? ProjectId { get; set; }

        public bool RequiresFollowUp { get; set; }

        public DateTime? FollowUpDate { get; set; }
    }
}

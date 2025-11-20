using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.FreelanceOperations.ClientPortal.Dtos
{
    public class ProjectInteractionDto
    {
        [Required]
        public string InteractionType { get; set; }
    }
}

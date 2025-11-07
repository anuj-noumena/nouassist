using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.CollaborationHub.Resources.Enums;

namespace NouAssist.CollaborationHub.Resources.Resources.Dtos
{
    public class GrantAccessDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public ResourcePermission Permission { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}

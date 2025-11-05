using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.Resources.Enums;

namespace NouAssist.Resources.Resources.Dtos
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

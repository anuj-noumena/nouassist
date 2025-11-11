using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.AssetManagement.Resources.Enums;

namespace NouAssist.AssetManagement.Resources.Resources.Dtos
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

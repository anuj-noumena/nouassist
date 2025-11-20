using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO for granting access to an investor
    /// </summary>
    public class GrantAccessDto
    {
        [Required]
        public Guid InvestorId { get; set; }

        [Required]
        public AccessLevel AccessLevel { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO for revoking access from an investor
    /// </summary>
    public class RevokeAccessDto
    {
        [Required]
        public Guid InvestorId { get; set; }
    }
}

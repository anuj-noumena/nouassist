using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO for creating a new due diligence data room
    /// </summary>
    public class CreateDueDiligenceDataRoomDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool RequirePasswordAccess { get; set; } = false;

        [StringLength(200)]
        public string AccessPassword { get; set; }
    }
}

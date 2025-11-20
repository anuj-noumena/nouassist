using System;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO representing investor access to a data room
    /// </summary>
    public class DataRoomInvestorAccessDto
    {
        public Guid InvestorId { get; set; }
        public string InvestorName { get; set; }
        public string InvestorEmail { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public DateTime GrantedDate { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public int AccessCount { get; set; }
    }
}

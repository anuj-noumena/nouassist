using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.NetworkIntelligence.Dtos
{
    /// <summary>
    /// DTO representing investor influence scoring
    /// </summary>
    public class InvestorInfluenceDto
    {
        public Guid InvestorId { get; set; }
        public string Name { get; set; }
        public decimal InfluenceScore { get; set; }
        public int NetworkReach { get; set; }
        public List<string> InfluenceFactors { get; set; } = new();
    }
}

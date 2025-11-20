using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.NetworkIntelligence.Dtos
{
    /// <summary>
    /// DTO representing network insights
    /// </summary>
    public class NetworkInsightsDto
    {
        public int TotalNetworkSize { get; set; }
        public List<InvestorClusterDto> Clusters { get; set; } = new();
        public List<KeyConnectorDto> KeyConnectors { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
    }

    public class InvestorClusterDto
    {
        public string Name { get; set; }
        public List<Guid> InvestorIds { get; set; } = new();
        public string CommonCharacteristics { get; set; }
        public decimal AverageInvestment { get; set; }
    }

    public class KeyConnectorDto
    {
        public Guid InvestorId { get; set; }
        public string Name { get; set; }
        public int ConnectionCount { get; set; }
        public decimal InfluenceScore { get; set; }
        public string Role { get; set; }
    }
}

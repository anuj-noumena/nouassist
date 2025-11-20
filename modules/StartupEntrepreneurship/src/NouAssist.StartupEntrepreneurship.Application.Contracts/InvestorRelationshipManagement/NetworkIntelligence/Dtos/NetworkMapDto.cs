using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.NetworkIntelligence.Dtos
{
    /// <summary>
    /// DTO representing a network map
    /// </summary>
    public class NetworkMapDto
    {
        public Guid CenterInvestorId { get; set; }
        public List<NetworkNodeDto> Nodes { get; set; } = new();
        public List<NetworkEdgeDto> Edges { get; set; } = new();
        public NetworkStatisticsDto Statistics { get; set; }
    }

    public class NetworkNodeDto
    {
        public Guid InvestorId { get; set; }
        public string Name { get; set; }
        public InvestorType Type { get; set; }
        public decimal InvestmentAmount { get; set; }
        public int InfluenceScore { get; set; }
        public int ConnectionStrength { get; set; }
    }

    public class NetworkEdgeDto
    {
        public Guid FromInvestorId { get; set; }
        public Guid ToInvestorId { get; set; }
        public string ConnectionType { get; set; }
        public int Strength { get; set; }
        public List<string> CommonConnections { get; set; } = new();
    }

    public class NetworkStatisticsDto
    {
        public int TotalNodes { get; set; }
        public int TotalEdges { get; set; }
        public decimal AverageConnectionStrength { get; set; }
        public int ClusterCount { get; set; }
        public decimal NetworkDensity { get; set; }
    }
}

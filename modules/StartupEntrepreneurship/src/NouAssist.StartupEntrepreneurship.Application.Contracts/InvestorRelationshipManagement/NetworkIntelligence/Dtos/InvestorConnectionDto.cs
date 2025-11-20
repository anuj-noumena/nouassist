using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.NetworkIntelligence.Dtos
{
    /// <summary>
    /// DTO representing a potential investor connection
    /// </summary>
    public class InvestorConnectionDto
    {
        public Guid InvestorId { get; set; }
        public string Name { get; set; }
        public string ConnectionType { get; set; }
        public int ConnectionStrength { get; set; }
        public string ReasonForConnection { get; set; }
        public List<string> CommonFactors { get; set; } = new();
    }
}

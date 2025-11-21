using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Models
{
    /// <summary>
    /// Represents portfolio statistics for reporting
    /// </summary>
    public class PortfolioStatistics
    {
        public Guid PortfolioId { get; set; }
        public string PortfolioName { get; set; }
        public int TotalItems { get; set; }
        public Dictionary<PortfolioItemType, int> ItemTypeDistribution { get; set; }
        public decimal OverallProgress { get; set; }
        public decimal RiskScore { get; set; }
        public decimal CompliancePercentage { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

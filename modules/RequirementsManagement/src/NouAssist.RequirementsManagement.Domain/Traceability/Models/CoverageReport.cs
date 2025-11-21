using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.Traceability.Enums;

namespace NouAssist.RequirementsManagement.Traceability.Models
{
    /// <summary>
    /// Represents a report on traceability coverage between artifact types
    /// </summary>
    public class CoverageReport
    {
        /// <summary>
        /// Type of source artifacts
        /// </summary>
        public ArtifactType SourceType { get; set; }
        
        /// <summary>
        /// Type of target artifacts
        /// </summary>
        public ArtifactType TargetType { get; set; }
        
        /// <summary>
        /// Type of links included in the coverage analysis
        /// </summary>
        public LinkType? LinkType { get; set; }
        
        /// <summary>
        /// Total number of source artifacts
        /// </summary>
        public int TotalSourceArtifacts { get; set; }
        
        /// <summary>
        /// Total number of target artifacts
        /// </summary>
        public int TotalTargetArtifacts { get; set; }
        
        /// <summary>
        /// Number of source artifacts that have at least one link
        /// </summary>
        public int CoveredSourceArtifacts { get; set; }
        
        /// <summary>
        /// Number of target artifacts that have at least one link
        /// </summary>
        public int CoveredTargetArtifacts { get; set; }
        
        /// <summary>
        /// Source coverage percentage
        /// </summary>
        public decimal SourceCoveragePercentage { get; set; }
        
        /// <summary>
        /// Target coverage percentage
        /// </summary>
        public decimal TargetCoveragePercentage { get; set; }
        
        /// <summary>
        /// List of source artifacts without links
        /// </summary>
        public List<string> UncoveredSourceArtifacts { get; set; } = new List<string>();
        
        /// <summary>
        /// List of target artifacts without links
        /// </summary>
        public List<string> UncoveredTargetArtifacts { get; set; } = new List<string>();
        
        /// <summary>
        /// Calculates coverage percentages
        /// </summary>
        public void CalculateCoveragePercentages()
        {
            SourceCoveragePercentage = TotalSourceArtifacts > 0 
                ? Math.Round((decimal)CoveredSourceArtifacts / TotalSourceArtifacts * 100, 2) 
                : 0;
                
            TargetCoveragePercentage = TotalTargetArtifacts > 0 
                ? Math.Round((decimal)CoveredTargetArtifacts / TotalTargetArtifacts * 100, 2) 
                : 0;
        }
    }
}

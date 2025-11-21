using System;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// DTO for dependency analysis results
    /// </summary>
    public class DependencyAnalysisResultDto
    {
        public Guid DependentRequirementId { get; set; }
        public string DependencyType { get; set; }
        public string ImpactDescription { get; set; }
        public bool RequiresUpdate { get; set; }
    }
}

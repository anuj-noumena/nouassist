using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;

namespace NouAssist.RequirementsManagement.QualityAssurance.Services
{
    /// <summary>
    /// Domain service for managing defects
    /// </summary>
    public interface IDefectManager
    {
        /// <summary>
        /// Creates a new defect report
        /// </summary>
        Task<DefectReport> CreateAsync(
            string title,
            string description,
            string stepsToReproduce,
            string expectedOutcome,
            string actualOutcome,
            DefectSeverity severity,
            DefectPriority priority,
            string environment,
            string version,
            Guid? testRunId = null,
            Guid? testCycleId = null,
            Guid? testPlanId = null);
        
        /// <summary>
        /// Assigns a defect to a user
        /// </summary>
        Task<DefectReport> AssignAsync(Guid defectId, Guid assigneeId);
        
        /// <summary>
        /// Marks a defect as in progress
        /// </summary>
        Task<DefectReport> MarkAsInProgressAsync(Guid defectId);
        
        /// <summary>
        /// Marks a defect as fixed
        /// </summary>
        Task<DefectReport> MarkAsFixedAsync(Guid defectId, string fixCommitId);
        
        /// <summary>
        /// Verifies a defect fix
        /// </summary>
        Task<DefectReport> VerifyFixAsync(Guid defectId, string fixedInVersion);
        
        /// <summary>
        /// Rejects a defect fix
        /// </summary>
        Task<DefectReport> RejectFixAsync(Guid defectId, string reason);
        
        /// <summary>
        /// Closes a defect
        /// </summary>
        Task<DefectReport> CloseAsync(Guid defectId);
        
        /// <summary>
        /// Reopens a defect
        /// </summary>
        Task<DefectReport> ReopenAsync(Guid defectId, string reason);
        
        /// <summary>
        /// Adds a comment to a defect
        /// </summary>
        Task<DefectComment> AddCommentAsync(Guid defectId, string text);
        
        /// <summary>
        /// Links a requirement to a defect
        /// </summary>
        Task LinkRequirementAsync(Guid defectId, Guid requirementId);
        
        /// <summary>
        /// Gets high priority defects
        /// </summary>
        Task<List<DefectReport>> GetHighPriorityDefectsAsync();
    }
}

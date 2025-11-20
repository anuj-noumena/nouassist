using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos
{
    /// <summary>
    /// Data transfer object for a defect report
    /// </summary>
    public class DefectDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// The title of the defect
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The detailed description of the defect
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Steps to reproduce the defect
        /// </summary>
        public string StepsToReproduce { get; set; }
        
        /// <summary>
        /// The expected behavior or outcome
        /// </summary>
        public string ExpectedOutcome { get; set; }
        
        /// <summary>
        /// The actual behavior or outcome
        /// </summary>
        public string ActualOutcome { get; set; }
        
        /// <summary>
        /// The severity of the defect
        /// </summary>
        public DefectSeverity Severity { get; set; }
        
        /// <summary>
        /// The priority of the defect
        /// </summary>
        public DefectPriority Priority { get; set; }
        
        /// <summary>
        /// The current status of the defect
        /// </summary>
        public DefectStatus Status { get; set; }
        
        /// <summary>
        /// The user assigned to fix the defect
        /// </summary>
        public Guid? AssigneeId { get; set; }
        
        /// <summary>
        /// The test run that discovered this defect
        /// </summary>
        public Guid? TestRunId { get; set; }
        
        /// <summary>
        /// The test cycle associated with this defect
        /// </summary>
        public Guid? TestCycleId { get; set; }
        
        /// <summary>
        /// The test plan associated with this defect
        /// </summary>
        public Guid? TestPlanId { get; set; }
        
        /// <summary>
        /// Environment where the defect was found
        /// </summary>
        public string Environment { get; set; }
        
        /// <summary>
        /// Version or build where the defect was found
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// The commit ID that fixed the defect
        /// </summary>
        public string FixCommitId { get; set; }
        
        /// <summary>
        /// User who verified the fix
        /// </summary>
        public Guid? VerifierId { get; set; }
        
        /// <summary>
        /// Version or build where the fix was verified
        /// </summary>
        public string FixedInVersion { get; set; }
        
        /// <summary>
        /// Timestamp when the defect was assigned
        /// </summary>
        public DateTime? AssignedTime { get; set; }
        
        /// <summary>
        /// Timestamp when the defect was fixed
        /// </summary>
        public DateTime? FixedTime { get; set; }
        
        /// <summary>
        /// Timestamp when the fix was verified
        /// </summary>
        public DateTime? VerifiedTime { get; set; }
        
        /// <summary>
        /// Timestamp when the defect was closed
        /// </summary>
        public DateTime? ClosedTime { get; set; }
        
        /// <summary>
        /// List of comments on this defect
        /// </summary>
        public List<DefectCommentDto> Comments { get; set; }
        
        /// <summary>
        /// List of requirements affected by this defect
        /// </summary>
        public List<Guid> AffectedRequirementIds { get; set; }
    }
}

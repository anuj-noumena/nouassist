using System;
using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos
{
    /// <summary>
    /// DTO representing a merge operation
    /// </summary>
    public class MergeOperationDto : EntityDto<Guid>
    {
        /// <summary>
        /// ID of the source branch
        /// </summary>
        public Guid SourceBranchId { get; set; }
        
        /// <summary>
        /// ID of the target branch
        /// </summary>
        public Guid TargetBranchId { get; set; }
        
        /// <summary>
        /// Status of the merge operation
        /// </summary>
        public MergeStatus Status { get; set; }
        
        /// <summary>
        /// Result of the merge operation
        /// </summary>
        public MergeResultDto Result { get; set; }
        
        /// <summary>
        /// ID of the user who initiated the merge
        /// </summary>
        public Guid InitiatedById { get; set; }
        
        /// <summary>
        /// Name of the user who initiated the merge
        /// </summary>
        public string InitiatedByName { get; set; }
        
        /// <summary>
        /// Timestamp when the merge was initiated
        /// </summary>
        public DateTime InitiatedAt { get; set; }
        
        /// <summary>
        /// ID of the user who completed the merge (if applicable)
        /// </summary>
        public Guid? CompletedById { get; set; }
        
        /// <summary>
        /// Name of the user who completed the merge (if applicable)
        /// </summary>
        public string CompletedByName { get; set; }
        
        /// <summary>
        /// Timestamp when the merge was completed (if applicable)
        /// </summary>
        public DateTime? CompletedAt { get; set; }
    }
}

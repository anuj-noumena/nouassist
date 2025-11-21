using System;
using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Entities
{
    /// <summary>
    /// Entity representing a merge operation between branches
    /// </summary>
    public class MergeOperation : Entity<Guid>
    {
        /// <summary>
        /// ID of the source branch
        /// </summary>
        public Guid SourceBranchId { get; private set; }
        
        /// <summary>
        /// ID of the target branch
        /// </summary>
        public Guid TargetBranchId { get; private set; }
        
        /// <summary>
        /// Status of the merge operation
        /// </summary>
        public MergeStatus Status { get; private set; }
        
        /// <summary>
        /// Result of the merge operation
        /// </summary>
        public MergeResult Result { get; private set; }
        
        /// <summary>
        /// Reference to the user who initiated the merge
        /// </summary>
        public AuthorRef InitiatedBy { get; private set; }
        
        /// <summary>
        /// Timestamp when the merge was initiated
        /// </summary>
        public DateTime InitiatedAt { get; private set; }
        
        /// <summary>
        /// Reference to the user who completed the merge (if applicable)
        /// </summary>
        public AuthorRef CompletedBy { get; private set; }
        
        /// <summary>
        /// Timestamp when the merge was completed (if applicable)
        /// </summary>
        public DateTime? CompletedAt { get; private set; }

        protected MergeOperation() { }

        internal MergeOperation(
            Guid id,
            Guid sourceBranchId,
            Guid targetBranchId,
            AuthorRef initiatedBy,
            DateTime initiatedAt)
            : base(id)
        {
            SourceBranchId = sourceBranchId;
            TargetBranchId = targetBranchId;
            Status = MergeStatus.InProgress;
            InitiatedBy = initiatedBy;
            InitiatedAt = initiatedAt;
        }

        /// <summary>
        /// Updates the merge operation with the result and marks it as completed
        /// </summary>
        internal void Complete(MergeResult result, AuthorRef completedBy, DateTime completedAt)
        {
            Result = result;
            Status = result.Status;
            CompletedBy = completedBy;
            CompletedAt = completedAt;
        }

        /// <summary>
        /// Aborts the merge operation
        /// </summary>
        internal void Abort(string reason, AuthorRef abortedBy, DateTime abortedAt)
        {
            Result = MergeResult.Aborted(reason);
            Status = MergeStatus.Aborted;
            CompletedBy = abortedBy;
            CompletedAt = abortedAt;
        }
    }
}

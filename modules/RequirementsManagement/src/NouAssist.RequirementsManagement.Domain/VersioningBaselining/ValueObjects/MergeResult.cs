using System.Collections.Generic;
using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects
{
    /// <summary>
    /// Value object representing the result of a merge operation
    /// </summary>
    public class MergeResult : ValueObject
    {
        /// <summary>
        /// Status of the merge operation
        /// </summary>
        public MergeStatus Status { get; private set; }
        
        /// <summary>
        /// Number of requirements successfully merged
        /// </summary>
        public int MergedRequirementCount { get; private set; }
        
        /// <summary>
        /// Number of conflicts detected during the merge
        /// </summary>
        public int ConflictCount { get; private set; }
        
        /// <summary>
        /// Optional message or summary of the merge result
        /// </summary>
        public string Message { get; private set; }

        private MergeResult() { }

        /// <summary>
        /// Creates a new MergeResult with the specified properties
        /// </summary>
        public MergeResult(
            MergeStatus status, 
            int mergedRequirementCount, 
            int conflictCount, 
            string message = null)
        {
            Status = status;
            MergedRequirementCount = mergedRequirementCount;
            ConflictCount = conflictCount;
            Message = message;
        }

        /// <summary>
        /// Creates a successful merge result
        /// </summary>
        public static MergeResult Success(int mergedRequirementCount)
        {
            return new MergeResult(
                MergeStatus.Completed, 
                mergedRequirementCount, 
                0, 
                "Merge completed successfully"
            );
        }

        /// <summary>
        /// Creates a merge result with conflicts
        /// </summary>
        public static MergeResult Conflict(int mergedRequirementCount, int conflictCount)
        {
            return new MergeResult(
                MergeStatus.HasConflicts, 
                mergedRequirementCount, 
                conflictCount, 
                $"Merge has {conflictCount} conflicts that need resolution"
            );
        }

        /// <summary>
        /// Creates an aborted merge result
        /// </summary>
        public static MergeResult Aborted(string reason)
        {
            return new MergeResult(
                MergeStatus.Aborted, 
                0, 
                0, 
                $"Merge aborted: {reason}"
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Status;
            yield return MergedRequirementCount;
            yield return ConflictCount;
            yield return Message;
        }
    }
}

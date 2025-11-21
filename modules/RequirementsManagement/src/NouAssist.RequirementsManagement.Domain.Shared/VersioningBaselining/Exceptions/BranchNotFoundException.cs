using System;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Exceptions
{
    /// <summary>
    /// Exception thrown when a branch cannot be found
    /// </summary>
    public class BranchNotFoundException : VersionControlDomainException
    {
        /// <summary>
        /// ID of the branch
        /// </summary>
        public Guid BranchId { get; }

        /// <summary>
        /// Creates a new BranchNotFoundException
        /// </summary>
        public BranchNotFoundException(Guid branchId)
            : base("VersionControl:ErrorMessage:BranchNotFound")
        {
            BranchId = branchId;
            
            WithData("branchId", branchId);
        }
    }
}

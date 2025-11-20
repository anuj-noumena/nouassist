namespace NouAssist.RequirementsManagement.VersioningBaselining.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a branch with a name that already exists
    /// </summary>
    public class BranchNameAlreadyExistsException : VersionControlDomainException
    {
        /// <summary>
        /// Name of the branch that already exists
        /// </summary>
        public string BranchName { get; }

        /// <summary>
        /// Creates a new BranchNameAlreadyExistsException
        /// </summary>
        public BranchNameAlreadyExistsException(string branchName)
            : base("VersionControl:ErrorMessage:BranchNameAlreadyExists")
        {
            BranchName = branchName;
            
            WithData("branchName", branchName);
        }
    }
}

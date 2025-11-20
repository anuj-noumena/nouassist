namespace NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects
{
    /// <summary>
    /// Value object representing a branch name
    /// </summary>
    public class BranchName : ValueObject
    {
        /// <summary>
        /// The branch name
        /// </summary>
        public string Value { get; private set; }

        private BranchName() { }

        /// <summary>
        /// Creates a new BranchName with the specified name
        /// </summary>
        public BranchName(string name)
        {
            SetName(name);
        }

        private void SetName(string name)
        {
            Value = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: VersionControlConsts.MaxBranchNameLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
        
        public override string ToString()
        {
            return Value;
        }
    }
}

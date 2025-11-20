namespace NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects
{
    /// <summary>
    /// Value object representing an author reference
    /// </summary>
    public class AuthorRef : ValueObject
    {
        /// <summary>
        /// ID of the author
        /// </summary>
        public Guid Id { get; private set; }
        
        /// <summary>
        /// Name of the author
        /// </summary>
        public string Name { get; private set; }

        private AuthorRef() { }

        /// <summary>
        /// Creates a new AuthorRef with the specified ID and name
        /// </summary>
        public AuthorRef(Guid id, string name)
        {
            Id = id;
            SetName(name);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: VersionControlConsts.MaxAuthorNameLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Name;
        }
    }
}

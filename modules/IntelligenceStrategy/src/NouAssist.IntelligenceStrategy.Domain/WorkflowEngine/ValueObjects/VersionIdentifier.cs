namespace NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects
{
    /// <summary>
    /// Value object representing a version of a workflow definition
    /// </summary>
    public class VersionIdentifier : ValueObject
    {
        /// <summary>
        /// The major version number
        /// </summary>
        public int MajorVersion { get; private set; }
        
        /// <summary>
        /// The minor version number
        /// </summary>
        public int MinorVersion { get; private set; }
        
        /// <summary>
        /// Indicates if this version is published
        /// </summary>
        public bool IsPublished { get; private set; }
        
        /// <summary>
        /// The version creation timestamp
        /// </summary>
        public DateTime CreationTime { get; private set; }

        protected VersionIdentifier() { }

        public VersionIdentifier(int majorVersion, int minorVersion, bool isPublished = false)
        {
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            IsPublished = isPublished;
            CreationTime = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Marks this version as published
        /// </summary>
        public void Publish()
        {
            IsPublished = true;
        }
        
        /// <summary>
        /// Returns a string representation of the version
        /// </summary>
        /// <returns>A string in the format "Major.Minor"</returns>
        public override string ToString()
        {
            return $"{MajorVersion}.{MinorVersion}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MajorVersion;
            yield return MinorVersion;
            yield return IsPublished;
            yield return CreationTime;
        }
    }
}

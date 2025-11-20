namespace NouAssist.RequirementsManagement.VersioningBaselining.Exceptions
{
    /// <summary>
    /// Base exception for the VersionControl domain
    /// </summary>
    public class VersionControlDomainException : BusinessException
    {
        /// <summary>
        /// Creates a new VersionControlDomainException with the specified localization key
        /// </summary>
        public VersionControlDomainException(string code)
            : base(code)
        {
        }

        /// <summary>
        /// Creates a new VersionControlDomainException with the specified localization key and message
        /// </summary>
        public VersionControlDomainException(string code, string message)
            : base(code, message)
        {
        }

        /// <summary>
        /// Adds data to the exception for additional context
        /// </summary>
        public VersionControlDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}

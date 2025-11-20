namespace NouAssist.BusinessOperations.ProjectExecution.Exceptions
{
    /// <summary>
    /// Base exception for Project Execution domain
    /// </summary>
    public class ProjectExecutionDomainException : BusinessException
    {
        /// <summary>
        /// Creates a new ProjectExecutionDomainException with specified code
        /// </summary>
        /// <param name="code">Error code</param>
        public ProjectExecutionDomainException(string code)
            : base(code)
        {
        }

        /// <summary>
        /// Creates a new ProjectExecutionDomainException with specified code and message
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public ProjectExecutionDomainException(string code, string message)
            : base(code, message)
        {
        }

        /// <summary>
        /// Adds additional error data to the exception
        /// </summary>
        /// <param name="name">Data name</param>
        /// <param name="value">Data value</param>
        /// <returns>This exception instance for method chaining</returns>
        public ProjectExecutionDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}

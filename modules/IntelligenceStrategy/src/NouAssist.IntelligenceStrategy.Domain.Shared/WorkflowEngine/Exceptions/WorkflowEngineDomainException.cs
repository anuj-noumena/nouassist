using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Exceptions
{
    /// <summary>
    /// Base exception class for the Workflow Engine domain
    /// </summary>
    public class WorkflowEngineDomainException : BusinessException
    {
        /// <summary>
        /// Creates a new instance of <see cref="WorkflowEngineDomainException"/>
        /// </summary>
        /// <param name="code">Error code</param>
        public WorkflowEngineDomainException(string code)
            : base(code)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="WorkflowEngineDomainException"/>
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public WorkflowEngineDomainException(string code, string message)
            : base(code, message)
        {
        }
        
        /// <summary>
        /// Adds data to the exception for better diagnostics
        /// </summary>
        /// <param name="name">Name of the data</param>
        /// <param name="value">Value of the data</param>
        /// <returns>The current exception instance for chaining</returns>
        public WorkflowEngineDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}

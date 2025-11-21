using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions
{
    /// <summary>
    /// Base exception for Decision Support domain errors
    /// </summary>
    public class DecisionSupportDomainException : BusinessException
    {
        /// <summary>
        /// Creates a new instance of <see cref="DecisionSupportDomainException"/>
        /// </summary>
        /// <param name="code">Error code</param>
        public DecisionSupportDomainException(string code)
            : base(code)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="DecisionSupportDomainException"/>
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public DecisionSupportDomainException(string code, string message)
            : base(code, message)
        {
        }

        /// <summary>
        /// Adds contextual data to the exception
        /// </summary>
        /// <param name="name">Data key</param>
        /// <param name="value">Data value</param>
        /// <returns>This exception for method chaining</returns>
        public DecisionSupportDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}

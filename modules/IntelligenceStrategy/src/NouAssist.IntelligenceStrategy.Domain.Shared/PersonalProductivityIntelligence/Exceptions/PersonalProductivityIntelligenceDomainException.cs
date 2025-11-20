using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions
{
    /// <summary>
    /// Base exception for Personal Productivity Intelligence domain
    /// </summary>
    public class PersonalProductivityIntelligenceDomainException : BusinessException
    {
        /// <summary>
        /// Creates a new PersonalProductivityIntelligenceDomainException
        /// </summary>
        /// <param name="code">Error code</param>
        public PersonalProductivityIntelligenceDomainException(string code)
            : base(code)
        {
        }

        /// <summary>
        /// Creates a new PersonalProductivityIntelligenceDomainException with message
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public PersonalProductivityIntelligenceDomainException(string code, string message)
            : base(code, message)
        {
        }

        /// <summary>
        /// Adds data to the exception for better context
        /// </summary>
        /// <param name="name">Name of the data</param>
        /// <param name="value">Value of the data</param>
        /// <returns>This exception for chaining</returns>
        public PersonalProductivityIntelligenceDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}

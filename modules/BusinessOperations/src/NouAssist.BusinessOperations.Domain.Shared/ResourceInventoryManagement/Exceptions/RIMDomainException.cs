using Volo.Abp;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions
{
    /// <summary>
    /// Base exception for RIM domain
    /// </summary>
    public class RIMDomainException : BusinessException
    {
        /// <summary>
        /// Creates a new RIM domain exception with the specified code
        /// </summary>
        /// <param name="code">Error code</param>
        public RIMDomainException(string code)
            : base(code)
        {
        }

        /// <summary>
        /// Creates a new RIM domain exception with the specified code and message
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public RIMDomainException(string code, string message)
            : base(code, message)
        {
        }

        /// <summary>
        /// Adds data to the exception for additional context
        /// </summary>
        /// <param name="name">Data parameter name</param>
        /// <param name="value">Data parameter value</param>
        /// <returns>This exception for method chaining</returns>
        public RIMDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}

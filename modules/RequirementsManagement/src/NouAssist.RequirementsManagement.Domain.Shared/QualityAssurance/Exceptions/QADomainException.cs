namespace NouAssist.RequirementsManagement.QualityAssurance.Exceptions
{
    /// <summary>
    /// Base exception for QA domain
    /// </summary>
    public class QADomainException : BusinessException
    {
        /// <summary>
        /// Creates a new QA domain exception with the specified code
        /// </summary>
        public QADomainException(string code)
            : base(code)
        {
        }

        /// <summary>
        /// Creates a new QA domain exception with the specified code and message
        /// </summary>
        public QADomainException(string code, string message)
            : base(code, message)
        {
        }

        /// <summary>
        /// Adds data to the exception for additional context
        /// </summary>
        public QADomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Helpers
{
    /// <summary>
    /// Represents the result of a validation operation
    /// </summary>
    public class ValidationResult
    {
        private readonly List<string> _errors = new List<string>();
        
        /// <summary>
        /// Whether the validation passed
        /// </summary>
        public bool IsValid => !_errors.Any();
        
        /// <summary>
        /// List of validation errors
        /// </summary>
        public IReadOnlyList<string> Errors => _errors.AsReadOnly();
        
        /// <summary>
        /// Adds an error message
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        public void AddError(string errorMessage)
        {
            _errors.Add(errorMessage);
        }
        
        /// <summary>
        /// Adds multiple error messages
        /// </summary>
        /// <param name="errorMessages">The error messages</param>
        public void AddErrors(IEnumerable<string> errorMessages)
        {
            _errors.AddRange(errorMessages);
        }
        
        /// <summary>
        /// Merges another validation result into this one
        /// </summary>
        /// <param name="other">The other validation result</param>
        public void Merge(ValidationResult other)
        {
            if (other != null)
            {
                _errors.AddRange(other.Errors);
            }
        }
        
        /// <summary>
        /// Returns a string representation of validation errors
        /// </summary>
        public override string ToString()
        {
            return IsValid ? "Valid" : string.Join("; ", _errors);
        }
    }
}

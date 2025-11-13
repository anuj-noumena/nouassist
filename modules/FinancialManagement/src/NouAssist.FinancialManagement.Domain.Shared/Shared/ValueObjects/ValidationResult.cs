using System.Collections.Generic;
using System.Linq;

namespace NouAssist.FinancialManagement.Shared.ValueObjects
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public List<string> Errors { get; }

        public ValidationResult(bool isValid = true, List<string>? errors = null)
        {
            IsValid = isValid;
            Errors = errors ?? new List<string>();
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public static ValidationResult Success() => new(true);
        public static ValidationResult Failure(string error) => 
            new(false, new List<string> { error });
        public static ValidationResult Failure(List<string> errors) => 
            new(false, errors);

        public override string ToString() => IsValid 
            ? "Valid" 
            : $"Invalid: {string.Join("; ", Errors)}";
    }
}

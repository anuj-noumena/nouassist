using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.SharedKernel.ValueObjects
{
    public class ValidationResult : ValueObject
    {
        public bool IsValid { get; private set; }
        public List<ValidationError> Errors { get; private set; }

        public ValidationResult()
        {
            IsValid = true;
            Errors = new List<ValidationError>();
        }

        public void AddError(string message, string field = null)
        {
            IsValid = false;
            Errors.Add(new ValidationError(field, message));
        }

        public void AddErrors(IEnumerable<ValidationError> errors)
        {
            foreach (var error in errors)
            {
                AddError(error.Message, error.Field);
            }
        }

        public bool HasErrors => Errors.Any();

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return IsValid;
            foreach (var error in Errors)
            {
                yield return error;
            }
        }
    }

    public class ValidationError : ValueObject
    {
        public string Field { get; private set; }
        public string Message { get; private set; }

        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Field;
            yield return Message;
        }
    }
}

// Update OrderProcessingResult from the guide to use this

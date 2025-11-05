using System;
using Fpdart;
using NouAssist.Core.Mixins;

namespace NouAssist.Core.Validators
{
    public static class Validators
    {
        public static Option<ValidationError> Required(object value, string fieldName)
        {
            if (value == null)
            {
                return Option<ValidationError>.Some(new ValidationError(
                    fieldName,
                    $"{fieldName} is required",
                    value
                ));
            }

            if (value is string str && string.IsNullOrWhiteSpace(str))
            {
                return Option<ValidationError>.Some(new ValidationError(
                    fieldName,
                    $"{fieldName} cannot be empty",
                    value
                ));
            }

            return Option<ValidationError>.None;
        }

        public static Option<ValidationError> MaxLength(string value, int maxLength, string fieldName)
        {
            if (value != null && value.Length > maxLength)
            {
                return Option<ValidationError>.Some(new ValidationError(
                    fieldName,
                    $"{fieldName} cannot exceed {maxLength} characters",
                    value
                ));
            }

            return Option<ValidationError>.None;
        }

        public static Option<ValidationError> Range(int? value, int min, int max, string fieldName)
        {
            if (value.HasValue && (value.Value < min || value.Value > max))
            {
                return Option<ValidationError>.Some(new ValidationError(
                    fieldName,
                    $"{fieldName} must be between {min} and {max}",
                    value
                ));
            }

            return Option<ValidationError>.None;
        }

        public static Option<ValidationError> DateRange(DateTime? start, DateTime? end, string fieldName)
        {
            if (start.HasValue && end.HasValue && start.Value > end.Value)
            {
                return Option<ValidationError>.Some(new ValidationError(
                    fieldName,
                    $"{fieldName} start must be before end",
                    null
                ));
            }

            return Option<ValidationError>.None;
        }

        public static Option<ValidationError> ListSize<T>(ICollection<T> list, int maxSize, string fieldName)
        {
            if (list != null && list.Count > maxSize)
            {
                return Option<ValidationError>.Some(new ValidationError(
                    fieldName,
                    $"{fieldName} cannot have more than {maxSize} items",
                    list.Count
                ));
            }

            return Option<ValidationError>.None;
        }

        public static Either<ValidationErrors, Unit> Combine(params Option<ValidationError>[] validations)
        {
            var errors = validations
                .Where(v => v.IsSome)
                .Select(v => v.Value)
                .ToList();

            if (errors.Any())
            {
                return Either<ValidationErrors, Unit>.Left(new ValidationErrors(errors));
            }

            return Either<ValidationErrors, Unit>.Right(Unit.Default);
        }
    }

    public class ValidationError
    {
        public string Field { get; }
        public string Message { get; }
        public object Value { get; }

        public ValidationError(string field, string message, object value = null)
        {
            Field = field;
            Message = message;
            Value = value;
        }
    }

    public class ValidationErrors
    {
        public IReadOnlyList<ValidationError> Errors { get; }

        public ValidationErrors(IEnumerable<ValidationError> errors)
        {
            Errors = errors.ToList().AsReadOnly();
        }

        public override string ToString()
        {
            return string.Join("; ", Errors.Select(e => e.Message));
        }
    }
}

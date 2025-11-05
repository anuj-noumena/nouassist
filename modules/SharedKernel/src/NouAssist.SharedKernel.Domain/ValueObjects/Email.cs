using System.Collections.Generic;
using System.Text.RegularExpressions;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.SharedKernel.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; private set; }

        private Email() { }

        public Email(string value)
        {
            value = value?.Trim().ToLowerInvariant();
            
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessException("Core:EmailRequired", "Email is required.");

            if (!IsValidEmail(value))
                throw new BusinessException("Core:InvalidEmail", "Invalid email format.");

            Value = value;
        }

        public static Email Create(string value) => new(value);

        public string GetDomain() => Value.Split('@')[1];
        public string GetLocalPart() => Value.Split('@')[0];

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(Email email) => email.Value;
    }
}

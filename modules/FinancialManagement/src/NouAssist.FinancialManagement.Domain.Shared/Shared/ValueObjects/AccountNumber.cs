using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.Shared.ValueObjects
{
    public class AccountNumber : ValueObject
    {
        public string Number { get; private set; }

        protected AccountNumber() { }

        public AccountNumber(string number)
        {
            Number = Check.NotNullOrWhiteSpace(number, nameof(number));
            ValidateFormat(number);
        }

        private static void ValidateFormat(string number)
        {
            // Account numbers: digits with optional dots/dashes (e.g., "1000", "1000.01", "1-100")
            if (!Regex.IsMatch(number, @"^[\d]+([\.\-][\d]+)*$"))
                throw new BusinessException("Financial:InvalidAccountNumber", 
                    "Account number must contain only digits with optional dots or dashes");
            
            if (number.Length > 20)
                throw new BusinessException("Financial:AccountNumberTooLong", 
                    "Account number cannot exceed 20 characters");
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Number;
        }

        public override string ToString() => Number;
    }
}

using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AccountsReceivable.ValueObjects
{
    public class CustomerContact : ValueObject
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Department { get; private set; }

        private CustomerContact() { }

        public CustomerContact(string name, string email, string phone = null, string department = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 100);
            Email = Check.NotNullOrWhiteSpace(email, nameof(email), maxLength: 100);
            Phone = phone;
            Department = department;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Email;
            yield return Phone ?? string.Empty;
            yield return Department ?? string.Empty;
        }
    }
}

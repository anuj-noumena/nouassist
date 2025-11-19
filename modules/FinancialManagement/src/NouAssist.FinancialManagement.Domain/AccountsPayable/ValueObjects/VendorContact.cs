using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AccountsPayable.ValueObjects
{
    public class VendorContact : ValueObject
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Role { get; private set; }

        private VendorContact() { }

        public VendorContact(string name, string email, string phone = null, string role = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 100);
            Email = Check.NotNullOrWhiteSpace(email, nameof(email), maxLength: 100);
            Phone = phone;
            Role = role;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Email;
            yield return Phone ?? string.Empty;
            yield return Role ?? string.Empty;
        }
    }
}

using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AccountsPayable.ValueObjects
{
    public class ContactInfo : ValueObject
    {
        public string Email { get; }
        public string Phone { get; }
        public string Address { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public string Country { get; }

        protected ContactInfo() { }

        public ContactInfo(
            string email,
            string phone = null,
            string address = null,
            string city = null,
            string state = null,
            string zipCode = null,
            string country = null)
        {
            Email = Check.NotNullOrWhiteSpace(email, nameof(email), 200);
            Phone = phone;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipCode;
            Country = country ?? "USA";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Email;
            yield return Phone;
            yield return Address;
            yield return City;
            yield return State;
            yield return ZipCode;
            yield return Country;
        }
    }
}

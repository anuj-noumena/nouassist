using System.Collections.Generic;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for stakeholder contact information
    /// </summary>
    public class ContactInfo : ValueObject
    {
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }

        private ContactInfo() { }

        public ContactInfo(string email, string phone = null, string address = null)
        {
            SetEmail(email);
            SetPhone(phone);
            SetAddress(address);
        }

        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessException("Elicitation:Validation:Required")
                    .WithData("propertyName", "Email");
            }

            if (!email.Contains("@") || email.Length > ElicitationConsts.MaxEmailLength)
            {
                throw new BusinessException("Elicitation:Validation:InvalidEmail");
            }

            Email = email;
        }

        private void SetPhone(string phone)
        {
            if (!string.IsNullOrEmpty(phone) && phone.Length > ElicitationConsts.MaxPhoneLength)
            {
                throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "Phone")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxPhoneLength);
            }

            Phone = phone;
        }

        private void SetAddress(string address)
        {
            if (!string.IsNullOrEmpty(address) && address.Length > ElicitationConsts.MaxAddressLength)
            {
                throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "Address")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxAddressLength);
            }

            Address = address;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Email;
            yield return Phone ?? string.Empty;
            yield return Address ?? string.Empty;
        }
    }
}

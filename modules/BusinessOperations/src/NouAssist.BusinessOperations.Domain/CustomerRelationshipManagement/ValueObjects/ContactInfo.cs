using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Represents contact information for leads, contacts, and organizations
    /// </summary>
    public class ContactInfo : ValueObject
    {
        /// <summary>
        /// Primary email address
        /// </summary>
        public string Email { get; private set; }
        
        /// <summary>
        /// Primary phone number
        /// </summary>
        public string Phone { get; private set; }
        
        /// <summary>
        /// Alternative phone number
        /// </summary>
        public string AlternativePhone { get; private set; }
        
        /// <summary>
        /// Website URL
        /// </summary>
        public string Website { get; private set; }
        
        /// <summary>
        /// Street address
        /// </summary>
        public string Address { get; private set; }
        
        /// <summary>
        /// City
        /// </summary>
        public string City { get; private set; }
        
        /// <summary>
        /// State or province
        /// </summary>
        public string State { get; private set; }
        
        /// <summary>
        /// Postal code
        /// </summary>
        public string PostalCode { get; private set; }
        
        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; private set; }

        protected ContactInfo()
        {
        }

        public ContactInfo(
            string email,
            string phone,
            string alternativePhone = null,
            string website = null,
            string address = null,
            string city = null,
            string state = null,
            string postalCode = null,
            string country = null)
        {
            SetEmail(email);
            SetPhone(phone);
            AlternativePhone = alternativePhone;
            Website = website;
            Address = address;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }

        public ContactInfo Update(
            string email = null,
            string phone = null,
            string alternativePhone = null,
            string website = null,
            string address = null,
            string city = null,
            string state = null,
            string postalCode = null,
            string country = null)
        {
            if (email != null)
            {
                SetEmail(email);
            }

            if (phone != null)
            {
                SetPhone(phone);
            }

            if (alternativePhone != null)
            {
                AlternativePhone = alternativePhone;
            }

            if (website != null)
            {
                Website = website;
            }

            if (address != null)
            {
                Address = address;
            }

            if (city != null)
            {
                City = city;
            }

            if (state != null)
            {
                State = state;
            }

            if (postalCode != null)
            {
                PostalCode = postalCode;
            }

            if (country != null)
            {
                Country = country;
            }

            return this;
        }

        private void SetEmail(string email)
        {
            Email = Check.NotNullOrWhiteSpace(
                email,
                nameof(email),
                maxLength: CRMConsts.MaxLengths.Email
            );

            if (!IsValidEmail(Email))
            {
                throw new BusinessException("CRM:Validation:InvalidEmail");
            }
        }

        private void SetPhone(string phone)
        {
            Phone = Check.NotNullOrWhiteSpace(
                phone,
                nameof(phone),
                maxLength: CRMConsts.MaxLengths.Phone
            );

            if (!IsValidPhone(Phone))
            {
                throw new BusinessException("CRM:Validation:InvalidPhone");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            // Simple validation - could be enhanced based on requirements
            return phone.Length >= 7 && phone.All(c => char.IsDigit(c) || new[] { '+', '-', '(', ')', ' ' }.Contains(c));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Email;
            yield return Phone;
            yield return AlternativePhone ?? string.Empty;
            yield return Website ?? string.Empty;
            yield return Address ?? string.Empty;
            yield return City ?? string.Empty;
            yield return State ?? string.Empty;
            yield return PostalCode ?? string.Empty;
            yield return Country ?? string.Empty;
        }
    }
}

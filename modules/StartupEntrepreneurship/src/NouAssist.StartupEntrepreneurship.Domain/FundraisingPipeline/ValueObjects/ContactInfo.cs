using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects
{
    /// <summary>
    /// Value object representing contact information
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
            Phone = phone;
            Address = address;
        }
        
        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.");
            }
            
            if (!IsValidEmail(email))
            {
                throw new ArgumentException("Invalid email format.");
            }
            
            if (email.Length > FundraisingPipelineConsts.Contact.MaxEmailLength)
            {
                throw new ArgumentException($"Email cannot exceed {FundraisingPipelineConsts.Contact.MaxEmailLength} characters.");
            }
            
            Email = email;
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
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Email;
            yield return Phone ?? string.Empty;
            yield return Address ?? string.Empty;
        }
    }
}

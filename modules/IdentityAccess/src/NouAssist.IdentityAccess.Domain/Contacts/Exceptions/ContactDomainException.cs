using System;
using Volo.Abp;

namespace NouAssist.IdentityAccess.Contacts.Exceptions
{
    public class ContactDomainException : BusinessException
    {
        public ContactDomainException(string code, string message = null) 
            : base(code, message)
        {
        }
    }

    public class ContactNotFoundException : ContactDomainException
    {
        public ContactNotFoundException(Guid contactId) 
            : base("Contacts:NotFound", $"Contact with ID {contactId} was not found")
        {
            WithData("ContactId", contactId);
        }
    }

    public class DuplicateContactException : ContactDomainException
    {
        public DuplicateContactException(string email) 
            : base("Contacts:DuplicateEmail", $"A contact with email {email} already exists")
        {
            WithData("Email", email);
        }
    }
}

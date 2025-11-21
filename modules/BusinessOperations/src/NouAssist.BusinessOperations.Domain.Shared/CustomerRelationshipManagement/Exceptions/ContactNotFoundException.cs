using System;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a contact cannot be found
    /// </summary>
    public class ContactNotFoundException : CRMDomainException
    {
        public ContactNotFoundException(Guid id)
            : base("CRM:ErrorMessage:ContactNotFound")
        {
            WithData("contactId", id);
        }
    }
}

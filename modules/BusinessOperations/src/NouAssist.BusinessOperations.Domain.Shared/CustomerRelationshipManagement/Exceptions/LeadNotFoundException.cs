using System;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a lead cannot be found
    /// </summary>
    public class LeadNotFoundException : CRMDomainException
    {
        public LeadNotFoundException(Guid id)
            : base("CRM:ErrorMessage:LeadNotFound")
        {
            WithData("leadId", id);
        }
    }
}

using System;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to convert a lead that has already been converted
    /// </summary>
    public class LeadAlreadyConvertedException : CRMDomainException
    {
        public LeadAlreadyConvertedException(Guid id)
            : base("CRM:ErrorMessage:LeadAlreadyConverted")
        {
            WithData("leadId", id);
        }
    }
}

using System;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when an investor is not found
    /// </summary>
    public class InvestorNotFoundException : IRMDomainException
    {
        public InvestorNotFoundException(Guid investorId)
            : base("IRM:ErrorMessage:InvestorNotFound")
        {
            WithData("investorId", investorId);
        }
    }
}

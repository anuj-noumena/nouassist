using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to test an already tested hypothesis
    /// </summary>
    public class HypothesisAlreadyTestedException : BusinessModelManagementDomainException
    {
        public HypothesisAlreadyTestedException(Guid hypothesisId, ValidationStatus currentStatus)
            : base("BusinessModelManagement:ErrorMessage:HypothesisAlreadyTested")
        {
            WithData("hypothesisId", hypothesisId);
            WithData("currentStatus", currentStatus);
        }
    }
}

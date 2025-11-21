using System;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Exceptions
{
    /// <summary>
    /// Exception thrown when a problem hypothesis is not found
    /// </summary>
    public class ProblemHypothesisNotFoundException : ProductDiscoveryDomainException
    {
        public ProblemHypothesisNotFoundException(Guid hypothesisId)
            : base("ProductDiscovery:ErrorMessage:ProblemHypothesisNotFound")
        {
            WithData("hypothesisId", hypothesisId);
        }
    }
}

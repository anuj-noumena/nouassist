using System;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Exceptions
{
    /// <summary>
    /// Exception thrown when a feature definition is not found
    /// </summary>
    public class FeatureDefinitionNotFoundException : ProductDiscoveryDomainException
    {
        public FeatureDefinitionNotFoundException(Guid featureId)
            : base("ProductDiscovery:ErrorMessage:FeatureDefinitionNotFound")
        {
            WithData("featureId", featureId);
        }
    }
}

using System;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions
{
    /// <summary>
    /// Exception thrown when a conversion funnel configuration is invalid
    /// </summary>
    public class InvalidFunnelConfigurationException : GrowthExperimentationDomainException
    {
        public Guid FunnelId { get; }

        public InvalidFunnelConfigurationException(Guid funnelId, string details)
            : base("GrowthExperimentation:ErrorMessage:InvalidFunnelConfiguration")
        {
            FunnelId = funnelId;
            
            WithData("funnelId", funnelId);
            WithData("details", details);
        }
    }
}

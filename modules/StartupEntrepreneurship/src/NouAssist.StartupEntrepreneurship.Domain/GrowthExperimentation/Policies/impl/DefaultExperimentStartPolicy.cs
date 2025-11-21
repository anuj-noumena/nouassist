using System.Linq;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Policies.impl
{
    /// <summary>
    /// Default implementation of experiment start policy
    /// </summary>
    public class DefaultExperimentStartPolicy : IExperimentStartPolicy
    {
        public PolicyResult CanStart(GrowthExperiment experiment)
        {
            // Can only start experiments in Draft status
            if (experiment.Status != ExperimentStatus.Draft)
            {
                return PolicyResult.Deny($"Cannot start experiment in {experiment.Status} status");
            }

            // Must have at least 2 variants
            if (experiment.Variants.Count < 2)
            {
                return PolicyResult.Deny("Experiment must have at least 2 variants to start");
            }

            // Must have at least one target metric
            if (experiment.TargetMetrics.Count == 0)
            {
                return PolicyResult.Deny("Experiment must have at least one target metric");
            }

            // Must have at least one hypothesis
            if (experiment.Hypotheses.Count == 0)
            {
                return PolicyResult.Deny("Experiment must have at least one hypothesis");
            }

            return PolicyResult.Allow();
        }
    }
}

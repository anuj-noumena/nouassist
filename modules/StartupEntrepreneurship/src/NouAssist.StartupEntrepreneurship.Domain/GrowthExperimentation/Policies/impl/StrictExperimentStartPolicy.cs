using System.Linq;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Policies.impl
{
    /// <summary>
    /// Strict implementation of experiment start policy with enhanced validation
    /// </summary>
    public class StrictExperimentStartPolicy : IExperimentStartPolicy
    {
        private readonly int _minimumSampleSizePerVariant;
        private readonly int _minimumDurationDays;

        public StrictExperimentStartPolicy(
            int minimumSampleSizePerVariant = 1000,
            int minimumDurationDays = 7)
        {
            _minimumSampleSizePerVariant = minimumSampleSizePerVariant;
            _minimumDurationDays = minimumDurationDays;
        }

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

            // Each variant must have minimum sample size configured
            var insufficientVariants = experiment.Variants
                .Where(v => v.TargetSampleSize < _minimumSampleSizePerVariant)
                .ToList();

            if (insufficientVariants.Any())
            {
                return PolicyResult.Deny(
                    $"All variants must have a minimum sample size of {_minimumSampleSizePerVariant}. " +
                    $"{insufficientVariants.Count} variants don't meet this requirement");
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

            // Experiment duration must meet minimum
            if (experiment.EndDate.HasValue && experiment.StartDate.HasValue)
            {
                var duration = (experiment.EndDate.Value - experiment.StartDate.Value).TotalDays;
                if (duration < _minimumDurationDays)
                {
                    return PolicyResult.Deny(
                        $"Experiment duration must be at least {_minimumDurationDays} days. " +
                        $"Current duration: {duration:F0} days");
                }
            }

            // Control variant must be present
            var controlVariant = experiment.Variants.FirstOrDefault(v => v.IsControl);
            if (controlVariant == null)
            {
                return PolicyResult.Deny("Experiment must have exactly one control variant");
            }

            return PolicyResult.Allow();
        }
    }
}

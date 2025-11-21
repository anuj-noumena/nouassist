using System.Linq;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Policies.impl
{
    /// <summary>
    /// Strict implementation of canvas validation policy with enhanced requirements
    /// </summary>
    public class StrictCanvasValidationPolicy : ICanvasValidationPolicy
    {
        private readonly int _minimumHypotheses;
        private readonly int _minimumComponentBulletPoints;

        public StrictCanvasValidationPolicy(
            int minimumHypotheses = 3,
            int minimumComponentBulletPoints = 2)
        {
            _minimumHypotheses = minimumHypotheses;
            _minimumComponentBulletPoints = minimumComponentBulletPoints;
        }

        public PolicyResult CanValidate(BusinessModelCanvas canvas)
        {
            // Must have at least one version
            if (canvas.Versions.Count == 0)
            {
                return PolicyResult.Deny("Canvas must have at least one version to be validated");
            }

            // Must have a current version
            if (!canvas.CurrentVersionId.HasValue)
            {
                return PolicyResult.Deny("Canvas must have a current version set");
            }

            // All components must have content
            var emptyComponents = canvas.Components
                .Where(c => c.Content == null || string.IsNullOrWhiteSpace(c.Content.TextContent))
                .ToList();

            if (emptyComponents.Any())
            {
                return PolicyResult.Deny($"Canvas has {emptyComponents.Count} empty components");
            }

            // Each component must have minimum bullet points
            var insufficientComponents = canvas.Components
                .Where(c => c.Content?.BulletPoints?.Count < _minimumComponentBulletPoints)
                .ToList();

            if (insufficientComponents.Any())
            {
                return PolicyResult.Deny(
                    $"Canvas has {insufficientComponents.Count} components with fewer than {_minimumComponentBulletPoints} bullet points");
            }

            // Must have minimum hypotheses tested
            var testedHypotheses = canvas.AssociatedHypotheses
                .Count(h => h.TestStatus == BusinessModelManagement.Enums.TestStatus.Tested ||
                           h.TestStatus == BusinessModelManagement.Enums.TestStatus.Validated);

            if (testedHypotheses < _minimumHypotheses)
            {
                return PolicyResult.Deny(
                    $"Canvas must have at least {_minimumHypotheses} tested hypotheses. Currently has {testedHypotheses}");
            }

            return PolicyResult.Allow();
        }
    }
}

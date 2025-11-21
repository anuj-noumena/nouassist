using System.Linq;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Policies.impl
{
    /// <summary>
    /// Default implementation of canvas validation policy
    /// </summary>
    public class DefaultCanvasValidationPolicy : ICanvasValidationPolicy
    {
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

            // All required components must have content
            var emptyComponents = canvas.Components
                .Where(c => c.Content == null || string.IsNullOrWhiteSpace(c.Content.TextContent))
                .ToList();

            if (emptyComponents.Any())
            {
                return PolicyResult.Deny($"Canvas has {emptyComponents.Count} empty components");
            }

            return PolicyResult.Allow();
        }
    }
}

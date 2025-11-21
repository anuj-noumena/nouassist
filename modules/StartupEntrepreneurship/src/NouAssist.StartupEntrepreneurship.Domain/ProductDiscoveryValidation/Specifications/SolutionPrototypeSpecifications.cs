using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Specifications
{
    /// <summary>
    /// Base specification class for SolutionPrototype aggregate
    /// </summary>
    public abstract class SolutionPrototypeSpecification : Specification<SolutionPrototype>
    {
    }

    /// <summary>
    /// Specification for prototypes ready for testing
    /// </summary>
    public class TestReadyPrototypeSpecification : SolutionPrototypeSpecification
    {
        public override Expression<Func<SolutionPrototype, bool>> ToExpression()
        {
            return prototype => 
                prototype.Versions.Any(v => v.IsActive) &&
                prototype.Type == PrototypeType.InteractivePrototype;
        }
    }

    /// <summary>
    /// Specification for prototypes with multiple iterations
    /// </summary>
    public class IteratedPrototypeSpecification : SolutionPrototypeSpecification
    {
        private readonly int _minimumVersions;

        public IteratedPrototypeSpecification(int minimumVersions = 2)
        {
            _minimumVersions = minimumVersions;
        }

        public override Expression<Func<SolutionPrototype, bool>> ToExpression()
        {
            return prototype => prototype.Versions.Count >= _minimumVersions;
        }
    }

    /// <summary>
    /// Specification for prototypes related to validated hypotheses
    /// </summary>
    public class ValidatedHypothesisPrototypeSpecification : SolutionPrototypeSpecification
    {
        public override Expression<Func<SolutionPrototype, bool>> ToExpression()
        {
            return prototype => prototype.RelatedHypothesisId.HasValue;
        }
    }

    /// <summary>
    /// Specification for stale prototypes that need updating
    /// </summary>
    public class StalePrototypeSpecification : SolutionPrototypeSpecification
    {
        private readonly int _daysSinceLastUpdate;

        public StalePrototypeSpecification(int daysSinceLastUpdate = 60)
        {
            _daysSinceLastUpdate = daysSinceLastUpdate;
        }

        public override Expression<Func<SolutionPrototype, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_daysSinceLastUpdate);
            return prototype => 
                prototype.Versions.Any() &&
                prototype.Versions.All(v => v.CreatedDate <= cutoffDate);
        }
    }
}

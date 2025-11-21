using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Specifications
{
    /// <summary>
    /// Base specification class for ProblemHypothesis aggregate
    /// </summary>
    public abstract class ProblemHypothesisSpecification : Specification<ProblemHypothesis>
    {
    }

    /// <summary>
    /// Specification for validated hypotheses with strong evidence
    /// </summary>
    public class StronglyValidatedHypothesisSpecification : ProblemHypothesisSpecification
    {
        private readonly int _minimumValidationAttempts;
        private readonly decimal _minimumFitScore;

        public StronglyValidatedHypothesisSpecification(int minimumValidationAttempts = 2, decimal minimumFitScore = 70m)
        {
            _minimumValidationAttempts = minimumValidationAttempts;
            _minimumFitScore = minimumFitScore;
        }

        public override Expression<Func<ProblemHypothesis, bool>> ToExpression()
        {
            return hypothesis => 
                hypothesis.Status == ValidationStatus.Validated &&
                hypothesis.ValidationAttempts.Count >= _minimumValidationAttempts &&
                hypothesis.FitMetric != null &&
                hypothesis.FitMetric.CalculateOverallFit() >= _minimumFitScore;
        }
    }

    /// <summary>
    /// Specification for hypotheses ready for validation
    /// </summary>
    public class ReadyForValidationSpecification : ProblemHypothesisSpecification
    {
        public override Expression<Func<ProblemHypothesis, bool>> ToExpression()
        {
            return hypothesis => 
                hypothesis.Status == ValidationStatus.NotStarted &&
                hypothesis.ValidationAttempts.Any();
        }
    }

    /// <summary>
    /// Specification for hypotheses related to a specific customer segment
    /// </summary>
    public class HypothesisForSegmentSpecification : ProblemHypothesisSpecification
    {
        private readonly Guid _customerSegmentId;

        public HypothesisForSegmentSpecification(Guid customerSegmentId)
        {
            _customerSegmentId = customerSegmentId;
        }

        public override Expression<Func<ProblemHypothesis, bool>> ToExpression()
        {
            return hypothesis => hypothesis.RelatedCustomerSegmentId == _customerSegmentId;
        }
    }

    /// <summary>
    /// Specification for stalled validation efforts
    /// </summary>
    public class StalledValidationSpecification : ProblemHypothesisSpecification
    {
        private readonly int _daysWithoutProgress;

        public StalledValidationSpecification(int daysWithoutProgress = 14)
        {
            _daysWithoutProgress = daysWithoutProgress;
        }

        public override Expression<Func<ProblemHypothesis, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_daysWithoutProgress);
            return hypothesis => 
                hypothesis.Status == ValidationStatus.InProgress &&
                hypothesis.ValidationAttempts.All(v => 
                    v.Status == ValidationStatus.NotStarted ||
                    (v.ActualStartDate.HasValue && v.ActualStartDate.Value <= cutoffDate && !v.CompletedDate.HasValue));
        }
    }
}

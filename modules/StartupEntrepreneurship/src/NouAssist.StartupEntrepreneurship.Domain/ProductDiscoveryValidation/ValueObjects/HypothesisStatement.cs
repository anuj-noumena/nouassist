using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects
{
    /// <summary>
    /// Value object representing a structured hypothesis statement
    /// </summary>
    public class HypothesisStatement : ValueObject
    {
        public string Problem { get; private set; }
        public string Target { get; private set; }
        public string Solution { get; private set; }
        public string ExpectedOutcome { get; private set; }

        private HypothesisStatement() { }

        public HypothesisStatement(string problem, string target, string solution, string expectedOutcome)
        {
            SetProblem(problem);
            SetTarget(target);
            SetSolution(solution);
            SetExpectedOutcome(expectedOutcome);
        }

        private void SetProblem(string problem)
        {
            Problem = Check.NotNullOrWhiteSpace(
                problem,
                nameof(problem),
                maxLength: ProductDiscoveryConsts.MaxHypothesisStatementLength
            );
        }

        private void SetTarget(string target)
        {
            Target = Check.NotNullOrWhiteSpace(
                target,
                nameof(target),
                maxLength: ProductDiscoveryConsts.MaxHypothesisStatementLength
            );
        }

        private void SetSolution(string solution)
        {
            Solution = Check.NotNullOrWhiteSpace(
                solution,
                nameof(solution),
                maxLength: ProductDiscoveryConsts.MaxHypothesisStatementLength
            );
        }

        private void SetExpectedOutcome(string expectedOutcome)
        {
            ExpectedOutcome = Check.NotNullOrWhiteSpace(
                expectedOutcome,
                nameof(expectedOutcome),
                maxLength: ProductDiscoveryConsts.MaxHypothesisStatementLength
            );
        }

        public string ToFullStatement()
        {
            return $"We believe that [{Target}] experience [{Problem}]. " +
                   $"If we [{Solution}], then [{ExpectedOutcome}].";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Problem;
            yield return Target;
            yield return Solution;
            yield return ExpectedOutcome;
        }
    }
}

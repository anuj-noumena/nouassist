using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ValidationVerification.ValueObjects
{
    /// <summary>
    /// Represents the actual result of a test step execution
    /// </summary>
    public class ActualResult : ValueObject
    {
        public string Description { get; private set; }
        public string Evidence { get; private set; }
        public DateTime ExecutedAt { get; private set; }
        public Guid ExecutedBy { get; private set; }

        private ActualResult() { }

        public ActualResult(
            string description,
            string evidence,
            DateTime executedAt,
            Guid executedBy)
        {
            SetDescription(description);
            Evidence = evidence;
            ExecutedAt = executedAt;
            ExecutedBy = executedBy;
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ValidationVerificationConsts.TestStep.MaxActualResultLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return Evidence;
            yield return ExecutedAt;
            yield return ExecutedBy;
        }
    }
}

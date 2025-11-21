using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ValidationVerification.ValueObjects
{
    /// <summary>
    /// Represents a review decision made by a reviewer
    /// </summary>
    public class ReviewDecision : ValueObject
    {
        public ReviewDecisionType DecisionType { get; private set; }
        public string Reason { get; private set; }
        public DateTime DecisionDate { get; private set; }
        public Guid ReviewerId { get; private set; }

        private ReviewDecision() { }

        public ReviewDecision(
            ReviewDecisionType decisionType,
            string reason,
            DateTime decisionDate,
            Guid reviewerId)
        {
            DecisionType = decisionType;
            SetReason(reason);
            DecisionDate = decisionDate;
            ReviewerId = reviewerId;
        }

        private void SetReason(string reason)
        {
            if (!string.IsNullOrWhiteSpace(reason))
            {
                Reason = Check.Length(
                    reason,
                    nameof(reason),
                    maxLength: ValidationVerificationConsts.ReviewFeedback.MaxReasonLength
                );
            }
            else
            {
                Reason = reason;
            }
        }

        public bool IsPositive()
        {
            return DecisionType == ReviewDecisionType.Approve;
        }

        public bool IsNegative()
        {
            return DecisionType == ReviewDecisionType.Reject;
        }

        public bool RequiresChanges()
        {
            return DecisionType == ReviewDecisionType.RequestChanges;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DecisionType;
            yield return Reason;
            yield return DecisionDate;
            yield return ReviewerId;
        }
    }
}

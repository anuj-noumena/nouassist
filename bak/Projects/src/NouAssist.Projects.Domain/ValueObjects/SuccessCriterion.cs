using System;
using System.Collections.Generic;
using NouAssist.Projects.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Projects.ValueObjects
{
    public class SuccessCriterion : ValueObject
    {
        public string Description { get; private set; }
        public string MeasurementMethod { get; private set; }
        public string TargetValue { get; private set; }
        public SuccessCriterionType CriterionType { get; private set; }
        public double Weight { get; private set; }
        public bool IsMet { get; private set; }
        public DateTime? MetAt { get; private set; }
        public string MetBy { get; private set; }

        private SuccessCriterion() { }

        public SuccessCriterion(
            string description,
            SuccessCriterionType criterionType,
            string measurementMethod = null,
            string targetValue = null,
            double weight = 1.0)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            CriterionType = criterionType;
            MeasurementMethod = measurementMethod;
            TargetValue = targetValue;
            Weight = weight;
            IsMet = false;

            if (weight < 0 || weight > 10)
            {
                throw new BusinessException("Projects:InvalidWeight", "Weight must be between 0 and 10.");
            }
        }

        public SuccessCriterion MarkAsMet(string metBy)
        {
            if (IsMet)
            {
                return this;
            }

            return new SuccessCriterion(
                Description,
                CriterionType,
                MeasurementMethod,
                TargetValue,
                Weight)
            {
                IsMet = true,
                MetAt = DateTime.UtcNow,
                MetBy = Check.NotNullOrWhiteSpace(metBy, nameof(metBy))
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return CriterionType;
            yield return MeasurementMethod;
            yield return TargetValue;
            yield return Weight;
        }
    }
}

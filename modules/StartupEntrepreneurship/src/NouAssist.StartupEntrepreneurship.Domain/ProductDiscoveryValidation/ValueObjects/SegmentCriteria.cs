using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects
{
    /// <summary>
    /// Value object representing criteria for customer segmentation
    /// </summary>
    public class SegmentCriteria : ValueObject
    {
        public Dictionary<string, object> Criteria { get; private set; }
        public int MinimumSampleSize { get; private set; }

        private SegmentCriteria() 
        { 
            Criteria = new Dictionary<string, object>();
        }

        public SegmentCriteria(Dictionary<string, object> criteria, int minimumSampleSize = 5)
        {
            SetCriteria(criteria);
            SetMinimumSampleSize(minimumSampleSize);
        }

        private void SetCriteria(Dictionary<string, object> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
            
            if (Criteria.Count == 0)
            {
                throw new BusinessException("ProductDiscovery:Validation:Required", 
                    "At least one criterion is required");
            }
        }

        private void SetMinimumSampleSize(int minimumSampleSize)
        {
            if (minimumSampleSize < 1)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidRange", 
                    "Minimum sample size must be at least 1");
            }

            MinimumSampleSize = minimumSampleSize;
        }

        public bool Matches(Dictionary<string, object> customerAttributes)
        {
            if (customerAttributes == null)
                return false;

            foreach (var criterion in Criteria)
            {
                if (!customerAttributes.ContainsKey(criterion.Key))
                    return false;

                if (!criterion.Value.Equals(customerAttributes[criterion.Key]))
                    return false;
            }

            return true;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MinimumSampleSize;
            foreach (var kvp in Criteria.OrderBy(x => x.Key))
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}

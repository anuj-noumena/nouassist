using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Defines the logic for alert conditions
    /// </summary>
    public class AlertConditionLogic : ValueObject
    {
        /// <summary>
        /// The expression that defines when an alert should trigger
        /// </summary>
        public string Expression { get; private set; }
        
        /// <summary>
        /// Description of what the alert means
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Whether consecutive occurrences are required to trigger
        /// </summary>
        public bool RequireConsecutiveOccurrences { get; private set; }
        
        /// <summary>
        /// Number of consecutive occurrences required
        /// </summary>
        public int ConsecutiveOccurrencesRequired { get; private set; }
        
        private AlertConditionLogic()
        {
        }
        
        public AlertConditionLogic(
            string expression,
            string description,
            bool requireConsecutiveOccurrences = false,
            int consecutiveOccurrencesRequired = 1)
        {
            Expression = Check.NotNullOrWhiteSpace(
                expression,
                nameof(expression),
                AnalyticsReportingConsts.MaxAlertConditionLogicLength
            );
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                AnalyticsReportingConsts.MaxDescriptionLength
            );
            
            if (consecutiveOccurrencesRequired < 1)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "consecutiveOccurrencesRequired");
            }
            
            RequireConsecutiveOccurrences = requireConsecutiveOccurrences;
            ConsecutiveOccurrencesRequired = consecutiveOccurrencesRequired;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Expression;
            yield return Description;
            yield return RequireConsecutiveOccurrences;
            yield return ConsecutiveOccurrencesRequired;
        }
    }
}

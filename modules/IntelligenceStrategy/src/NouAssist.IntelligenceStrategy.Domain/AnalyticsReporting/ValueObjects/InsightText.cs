using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Represents an insight text with recommended actions
    /// </summary>
    public class InsightText : ValueObject
    {
        /// <summary>
        /// The main insight text
        /// </summary>
        public string Text { get; private set; }
        
        /// <summary>
        /// When the insight was generated
        /// </summary>
        public DateTime GeneratedAt { get; private set; }
        
        /// <summary>
        /// Recommended actions based on the insight
        /// </summary>
        public string RecommendedAction { get; private set; }
        
        /// <summary>
        /// Importance level of the insight (1-10)
        /// </summary>
        public int ImportanceLevel { get; private set; }
        
        private InsightText()
        {
        }
        
        public InsightText(
            string text,
            DateTime generatedAt,
            string recommendedAction = null,
            int importanceLevel = 5)
        {
            Text = Check.NotNullOrWhiteSpace(
                text,
                nameof(text),
                AnalyticsReportingConsts.MaxInsightTextLength
            );
            
            if (recommendedAction != null && recommendedAction.Length > AnalyticsReportingConsts.MaxRecommendedActionLength)
            {
                throw new BusinessException("AnalyticsReporting:Validation:StringLength")
                    .WithData("propertyName", "recommendedAction")
                    .WithData("min", 1)
                    .WithData("max", AnalyticsReportingConsts.MaxRecommendedActionLength);
            }
            
            if (importanceLevel < 1 || importanceLevel > 10)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "importanceLevel");
            }
            
            Text = text;
            GeneratedAt = generatedAt;
            RecommendedAction = recommendedAction;
            ImportanceLevel = importanceLevel;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
            yield return GeneratedAt;
            yield return RecommendedAction ?? string.Empty;
            yield return ImportanceLevel;
        }
    }
}

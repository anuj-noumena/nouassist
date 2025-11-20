using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents an insight text derived from performance data
    /// </summary>
    public class InsightText : Entity
    {
        /// <summary>
        /// The insight text content
        /// </summary>
        public string Text { get; protected set; }
        
        /// <summary>
        /// Category of the insight
        /// </summary>
        public string Category { get; protected set; }
        
        /// <summary>
        /// Relevance score (0-1) indicating importance
        /// </summary>
        public double? RelevanceScore { get; protected set; }

        protected InsightText()
        {
        }

        /// <summary>
        /// Creates a new insight text
        /// </summary>
        public InsightText(
            string text,
            string category = null,
            double? relevanceScore = null)
        {
            Text = text;
            Category = category;
            RelevanceScore = relevanceScore.HasValue ? Math.Clamp(relevanceScore.Value, 0, 1) : null;
        }

        /// <summary>
        /// Gets the entity keys
        /// </summary>
        public override object[] GetKeys()
        {
            return new object[] { Text };
        }
    }
}

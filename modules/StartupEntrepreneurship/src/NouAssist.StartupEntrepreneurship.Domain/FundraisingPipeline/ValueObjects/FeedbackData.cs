using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects
{
    /// <summary>
    /// Value object for structured feedback data
    /// </summary>
    public class FeedbackData : ValueObject
    {
        public FeedbackSentiment Sentiment { get; private set; }
        public int Rating { get; private set; } // 1-10
        public Dictionary<string, string> CategoryFeedback { get; private set; } // e.g., "Problem Statement": "Clear and compelling"
        public List<string> StrengthPoints { get; private set; }
        public List<string> ImprovementAreas { get; private set; }
        
        private FeedbackData() 
        {
            CategoryFeedback = new Dictionary<string, string>();
            StrengthPoints = new List<string>();
            ImprovementAreas = new List<string>();
        }
        
        public FeedbackData(
            FeedbackSentiment sentiment, 
            int rating, 
            Dictionary<string, string> categoryFeedback, 
            List<string> strengthPoints, 
            List<string> improvementAreas)
        {
            SetRating(rating);
            Sentiment = sentiment;
            CategoryFeedback = categoryFeedback ?? new Dictionary<string, string>();
            StrengthPoints = strengthPoints ?? new List<string>();
            ImprovementAreas = improvementAreas ?? new List<string>();
        }
        
        private void SetRating(int rating)
        {
            if (rating < 1 || rating > 10)
            {
                throw new ArgumentException("Rating must be between 1 and 10.");
            }
            
            Rating = rating;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Sentiment;
            yield return Rating;
            
            // Sort dictionary keys for deterministic comparison
            foreach (var key in CategoryFeedback.Keys.OrderBy(k => k))
            {
                yield return key;
                yield return CategoryFeedback[key];
            }
            
            foreach (var point in StrengthPoints)
            {
                yield return point;
            }
            
            foreach (var area in ImprovementAreas)
            {
                yield return area;
            }
        }
    }
}

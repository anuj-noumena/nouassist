namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects
{
    /// <summary>
    /// Value object representing a prospect score
    /// </summary>
    public class ProspectScore : ValueObject
    {
        public int EngagementScore { get; private set; } // 0-100
        public int FitScore { get; private set; } // 0-100
        public int TimelinessScore { get; private set; } // 0-100
        public int OverallScore { get; private set; } // Calculated
        
        private ProspectScore() { }
        
        public ProspectScore(int engagementScore, int fitScore, int timelinessScore)
        {
            SetEngagementScore(engagementScore);
            SetFitScore(fitScore);
            SetTimelinessScore(timelinessScore);
            CalculateOverallScore();
        }
        
        private void SetEngagementScore(int score)
        {
            if (score < 0 || score > 100)
            {
                throw new ArgumentException("Engagement score must be between 0 and 100.");
            }
            
            EngagementScore = score;
        }
        
        private void SetFitScore(int score)
        {
            if (score < 0 || score > 100)
            {
                throw new ArgumentException("Fit score must be between 0 and 100.");
            }
            
            FitScore = score;
        }
        
        private void SetTimelinessScore(int score)
        {
            if (score < 0 || score > 100)
            {
                throw new ArgumentException("Timeliness score must be between 0 and 100.");
            }
            
            TimelinessScore = score;
        }
        
        private void CalculateOverallScore()
        {
            // Weighted average: 40% fit, 40% engagement, 20% timeliness
            OverallScore = (int)(FitScore * 0.4 + EngagementScore * 0.4 + TimelinessScore * 0.2);
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EngagementScore;
            yield return FitScore;
            yield return TimelinessScore;
            yield return OverallScore;
        }
    }
}

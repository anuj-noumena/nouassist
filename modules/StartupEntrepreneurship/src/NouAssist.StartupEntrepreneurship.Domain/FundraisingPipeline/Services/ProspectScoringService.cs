namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services
{
    /// <summary>
    /// Domain service for calculating and updating prospect scores
    /// </summary>
    public class ProspectScoringService : DomainService, IProspectScoringService
    {
        private readonly IProspectRepository _prospectRepository;
        private readonly ILogger<ProspectScoringService> _logger;
        
        public ProspectScoringService(
            IProspectRepository prospectRepository,
            ILogger<ProspectScoringService> logger)
        {
            _prospectRepository = prospectRepository;
            _logger = logger;
        }
        
        /// <summary>
        /// Calculates engagement score based on interaction history
        /// </summary>
        public async Task<int> CalculateEngagementScoreAsync(Guid prospectId)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);
            if (prospect == null)
            {
                throw new ProspectNotFoundException(prospectId);
            }
            
            int score = 0;
            var now = DateTime.Now;
            
            // No interactions means zero score
            if (!prospect.Interactions.Any())
            {
                return 0;
            }
            
            // Calculate recency score (40%)
            var mostRecentInteraction = prospect.Interactions.Max(i => i.InteractionDate);
            var daysSinceLastInteraction = (now - mostRecentInteraction).TotalDays;
            
            int recencyScore;
            if (daysSinceLastInteraction <= 7)
                recencyScore = 40;
            else if (daysSinceLastInteraction <= 14)
                recencyScore = 30;
            else if (daysSinceLastInteraction <= 30)
                recencyScore = 20;
            else if (daysSinceLastInteraction <= 60)
                recencyScore = 10;
            else
                recencyScore = 0;
            
            // Calculate frequency score (30%)
            var interactionCount = prospect.Interactions.Count;
            int frequencyScore = Math.Min(interactionCount * 5, 30);
            
            // Calculate type score (30%)
            int typeScore = 0;
            if (prospect.Interactions.Any(i => i.Type == InteractionType.Pitch))
                typeScore += 15;
            if (prospect.Interactions.Any(i => i.Type == InteractionType.Meeting))
                typeScore += 10;
            if (prospect.Interactions.Any(i => i.Type == InteractionType.Call))
                typeScore += 5;
            
            // Sum the scores
            score = recencyScore + frequencyScore + typeScore;
            
            return score;
        }
        
        /// <summary>
        /// Calculates fit score based on prospect attributes and company fit
        /// </summary>
        public async Task<int> CalculateFitScoreAsync(Guid prospectId)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);
            if (prospect == null)
            {
                throw new ProspectNotFoundException(prospectId);
            }
            
            // This would typically involve more complex business logic based on:
            // - Investment focus alignment
            // - Stage fit
            // - Industry expertise
            // - Previous investments
            // - Network connections
            
            // For demo purposes, we'll return a simplified score
            // In a real implementation, this would use more detailed criteria
            
            var random = new Random(prospectId.GetHashCode());
            return random.Next(40, 101); // Score between 40-100
        }
        
        /// <summary>
        /// Calculates timeliness score based on prospect pipeline status and timing
        /// </summary>
        public async Task<int> CalculateTimelinessScoreAsync(Guid prospectId)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);
            if (prospect == null)
            {
                throw new ProspectNotFoundException(prospectId);
            }
            
            int score = 0;
            
            // Higher score for prospects in advanced stages
            switch (prospect.PipelineStatus.Status)
            {
                case ProspectStatus.Negotiating:
                case ProspectStatus.TermSheetReceived:
                case ProspectStatus.Committed:
                    score += 50;
                    break;
                case ProspectStatus.InDueDiligence:
                case ProspectStatus.Pitched:
                    score += 40;
                    break;
                case ProspectStatus.Engaged:
                    score += 30;
                    break;
                case ProspectStatus.Contacted:
                    score += 20;
                    break;
                case ProspectStatus.Researching:
                case ProspectStatus.New:
                    score += 10;
                    break;
            }
            
            // Add score for prospects that have moved recently
            if (prospect.PipelineStatus.DaysInCurrentStatus <= 7)
            {
                score += 30;
            }
            else if (prospect.PipelineStatus.DaysInCurrentStatus <= 14)
            {
                score += 20;
            }
            else if (prospect.PipelineStatus.DaysInCurrentStatus <= 30)
            {
                score += 10;
            }
            
            // Cap at 100
            return Math.Min(score, 100);
        }
        
        /// <summary>
        /// Updates all scores for a prospect
        /// </summary>
        public async Task UpdateAllScoresAsync(Guid prospectId)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);
            if (prospect == null)
            {
                throw new ProspectNotFoundException(prospectId);
            }
            
            try
            {
                int engagementScore = await CalculateEngagementScoreAsync(prospectId);
                int fitScore = await CalculateFitScoreAsync(prospectId);
                int timelinessScore = await CalculateTimelinessScoreAsync(prospectId);
                
                prospect.UpdateScore(engagementScore, fitScore, timelinessScore);
                await _prospectRepository.UpdateAsync(prospect);
                
                _logger.LogInformation("Updated scores for prospect {ProspectId}: Engagement={Engagement}, Fit={Fit}, Timeliness={Timeliness}",
                    prospectId, engagementScore, fitScore, timelinessScore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update scores for prospect {ProspectId}", prospectId);
                throw;
            }
        }
    }
}

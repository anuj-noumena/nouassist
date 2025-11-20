namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services
{
    /// <summary>
    /// Domain service for analyzing pitch effectiveness
    /// </summary>
    public class PitchEffectivenessAnalyzer : DomainService, IPitchEffectivenessAnalyzer
    {
        private readonly IPitchDeckRepository _pitchDeckRepository;
        private readonly ILogger<PitchEffectivenessAnalyzer> _logger;
        
        public PitchEffectivenessAnalyzer(
            IPitchDeckRepository pitchDeckRepository,
            ILogger<PitchEffectivenessAnalyzer> logger)
        {
            _pitchDeckRepository = pitchDeckRepository;
            _logger = logger;
        }
        
        /// <summary>
        /// Analyzes the effectiveness of a pitch deck based on feedback and outcomes
        /// </summary>
        public async Task<PitchEffectivenessResult> AnalyzePitchEffectivenessAsync(Guid pitchDeckId)
        {
            var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(pitchDeckId);
            if (pitchDeck == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:PitchDeckNotFound")
                    .WithData("id", pitchDeckId);
            }
            
            try
            {
                var result = new PitchEffectivenessResult
                {
                    PitchDeckId = pitchDeckId,
                    PitchDeckTitle = pitchDeck.Title,
                    TotalPresentations = pitchDeck.Presentations.Count,
                    TotalFeedbacks = pitchDeck.Feedbacks.Count,
                    AverageRating = 0,
                    SentimentBreakdown = new Dictionary<FeedbackSentiment, int>(),
                    VersionEffectiveness = new Dictionary<string, double>(),
                    TopStrengths = new List<string>(),
                    TopImprovementAreas = new List<string>()
                };
                
                if (pitchDeck.Feedbacks.Any())
                {
                    // Calculate average rating
                    result.AverageRating = pitchDeck.Feedbacks.Average(f => f.Feedback.Rating);
                    
                    // Calculate sentiment breakdown
                    result.SentimentBreakdown = pitchDeck.Feedbacks
                        .GroupBy(f => f.Feedback.Sentiment)
                        .ToDictionary(g => g.Key, g => g.Count());
                    
                    // Calculate version effectiveness
                    var versionPresentationCounts = pitchDeck.Presentations
                        .GroupBy(p => p.VersionId)
                        .ToDictionary(g => g.Key, g => g.Count());
                    
                    var versionPositiveFeedbackCounts = new Dictionary<Guid, int>();
                    
                    foreach (var feedback in pitchDeck.Feedbacks)
                    {
                        var presentation = pitchDeck.Presentations.FirstOrDefault(p => p.Id == feedback.PresentationId);
                        if (presentation != null)
                        {
                            if (feedback.Feedback.Sentiment >= FeedbackSentiment.Positive)
                            {
                                if (!versionPositiveFeedbackCounts.ContainsKey(presentation.VersionId))
                                {
                                    versionPositiveFeedbackCounts[presentation.VersionId] = 0;
                                }
                                
                                versionPositiveFeedbackCounts[presentation.VersionId]++;
                            }
                        }
                    }
                    
                    foreach (var version in pitchDeck.Versions)
                    {
                        var presentationCount = versionPresentationCounts.GetValueOrDefault(version.Id);
                        if (presentationCount > 0)
                        {
                            var positiveFeedbackCount = versionPositiveFeedbackCounts.GetValueOrDefault(version.Id);
                            var effectiveness = (double)positiveFeedbackCount / presentationCount;
                            result.VersionEffectiveness[version.VersionName] = effectiveness;
                        }
                    }
                    
                    // Identify top strengths and improvement areas
                    var allStrengths = new Dictionary<string, int>();
                    var allImprovements = new Dictionary<string, int>();
                    
                    foreach (var feedback in pitchDeck.Feedbacks)
                    {
                        foreach (var strength in feedback.Feedback.StrengthPoints)
                        {
                            if (!allStrengths.ContainsKey(strength))
                            {
                                allStrengths[strength] = 0;
                            }
                            allStrengths[strength]++;
                        }
                        
                        foreach (var improvement in feedback.Feedback.ImprovementAreas)
                        {
                            if (!allImprovements.ContainsKey(improvement))
                            {
                                allImprovements[improvement] = 0;
                            }
                            allImprovements[improvement]++;
                        }
                    }
                    
                    result.TopStrengths = allStrengths
                        .OrderByDescending(s => s.Value)
                        .Take(5)
                        .Select(s => s.Key)
                        .ToList();
                    
                    result.TopImprovementAreas = allImprovements
                        .OrderByDescending(i => i.Value)
                        .Take(5)
                        .Select(i => i.Key)
                        .ToList();
                }
                
                _logger.LogInformation(
                    "Analyzed pitch effectiveness for pitch deck {PitchDeckId} with {PresentationCount} presentations and {FeedbackCount} feedbacks",
                    pitchDeckId, result.TotalPresentations, result.TotalFeedbacks);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to analyze pitch effectiveness for pitch deck {PitchDeckId}", pitchDeckId);
                throw;
            }
        }
    }
    
    /// <summary>
    /// Container for pitch effectiveness analysis results
    /// </summary>
    public class PitchEffectivenessResult
    {
        public Guid PitchDeckId { get; set; }
        public string PitchDeckTitle { get; set; }
        public int TotalPresentations { get; set; }
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<FeedbackSentiment, int> SentimentBreakdown { get; set; }
        public Dictionary<string, double> VersionEffectiveness { get; set; }
        public List<string> TopStrengths { get; set; }
        public List<string> TopImprovementAreas { get; set; }
    }
}

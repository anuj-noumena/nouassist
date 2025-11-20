namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services
{
    /// <summary>
    /// Interface for SWOT analysis domain service
    /// </summary>
    public interface ISwotAnalysisService
    {
        /// <summary>
        /// Creates a new SWOT analysis
        /// </summary>
        Task<SwotAnalysis> CreateSwotAnalysisAsync(
            string title,
            string description);

        /// <summary>
        /// Updates a SWOT analysis
        /// </summary>
        Task<SwotAnalysis> UpdateSwotAnalysisAsync(
            Guid analysisId,
            string title,
            string description,
            string analysisNotes = null);

        /// <summary>
        /// Adds a SWOT item
        /// </summary>
        Task AddSwotItemAsync(
            Guid analysisId,
            SwotCategory category,
            string title,
            string description,
            decimal impact = 0.5m,
            int priority = 1,
            string actionItems = null,
            string tags = null);

        /// <summary>
        /// Completes a SWOT analysis
        /// </summary>
        Task CompleteSwotAnalysisAsync(
            Guid analysisId,
            string strategicRecommendation);

        /// <summary>
        /// Gets recent SWOT analyses
        /// </summary>
        Task<List<SwotAnalysis>> GetRecentSwotAnalysesAsync(int maxResultCount = 10);

        /// <summary>
        /// Gets completed SWOT analyses
        /// </summary>
        Task<List<SwotAnalysis>> GetCompletedSwotAnalysesAsync();

        /// <summary>
        /// Evaluates analysis balance and quality
        /// </summary>
        Task<SwotAnalysisQualityResult> EvaluateSwotAnalysisQualityAsync(Guid analysisId);

        /// <summary>
        /// Gets SWOT items by category
        /// </summary>
        Task<Dictionary<SwotCategory, List<SwotItem>>> GetItemsByCategoryAsync(Guid analysisId);
    }
}

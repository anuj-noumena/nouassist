namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services
{
    /// <summary>
    /// Domain service for calculating investor relationship scores
    /// </summary>
    public class InvestorScoringService : DomainService, IInvestorScoringService
    {
        private readonly IInvestorProfileRepository _investorRepository;
        private readonly ILogger<InvestorScoringService> _logger;

        public InvestorScoringService(
            IInvestorProfileRepository investorRepository,
            ILogger<InvestorScoringService> logger)
        {
            _investorRepository = investorRepository;
            _logger = logger;
        }

        public async Task<RelationshipScore> CalculateScoreAsync(InvestorProfile investor, DateTime asOfDate)
        {
            try
            {
                var score = CalculateBaseScore(investor, asOfDate);
                var notes = $"Score calculated on {asOfDate:yyyy-MM-dd}";
                
                return new RelationshipScore(score, asOfDate, notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate relationship score for investor {InvestorId}", investor.Id);
                throw;
            }
        }

        public async Task<Dictionary<Guid, RelationshipScore>> CalculateScoresAsync(List<InvestorProfile> investors, DateTime asOfDate)
        {
            var result = new Dictionary<Guid, RelationshipScore>();

            foreach (var investor in investors)
            {
                try
                {
                    var score = await CalculateScoreAsync(investor, asOfDate);
                    result[investor.Id] = score;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to calculate score for investor {InvestorId}", investor.Id);
                    // Continue with other investors
                }
            }

            return result;
        }

        public async Task<RelationshipScore> RecalculateScoreAsync(Guid investorId, DateTime asOfDate)
        {
            var investor = await _investorRepository.GetAsync(investorId);
            return await CalculateScoreAsync(investor, asOfDate);
        }

        private int CalculateBaseScore(InvestorProfile investor, DateTime asOfDate)
        {
            var score = 0;

            // Base score based on status (0-20 points)
            score += investor.Status switch
            {
                InvestorStatus.Active => 20,
                InvestorStatus.Prospect => 10,
                InvestorStatus.Inactive => 5,
                _ => 0
            };

            // Investment amount factor (0-30 points)
            var totalInvestment = investor.GetTotalInvestedAmount();
            if (totalInvestment > 0)
            {
                score += totalInvestment switch
                {
                    >= 1000000 => 30,
                    >= 500000 => 25,
                    >= 100000 => 20,
                    >= 50000 => 15,
                    >= 10000 => 10,
                    _ => 5
                };
            }

            // Interaction frequency (0-25 points)
            var recentInteractions = investor.GetInteractionCount(TimeSpan.FromDays(90));
            score += recentInteractions switch
            {
                >= 10 => 25,
                >= 7 => 20,
                >= 5 => 15,
                >= 3 => 10,
                >= 1 => 5,
                _ => 0
            };

            // Last interaction recency (0-15 points)
            if (investor.LastInteractionDate.HasValue)
            {
                var daysSinceLastInteraction = (asOfDate - investor.LastInteractionDate.Value).Days;
                score += daysSinceLastInteraction switch
                {
                    <= 7 => 15,
                    <= 30 => 12,
                    <= 60 => 8,
                    <= 90 => 5,
                    _ => 0
                };
            }

            // Communication preferences alignment (0-10 points)
            if (investor.CommunicationPreferences != null)
            {
                score += investor.CommunicationPreferences.AllowMarketing ? 5 : 0;
                score += investor.CommunicationPreferences.UpdateFrequency != UpdateFrequency.Annually ? 5 : 0;
            }

            // Ensure score is within valid range
            return Math.Max(0, Math.Min(100, score));
        }
    }
}

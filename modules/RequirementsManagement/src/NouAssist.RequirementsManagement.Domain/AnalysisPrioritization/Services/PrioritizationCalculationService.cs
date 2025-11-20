namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service for calculating prioritization scores and rankings
    /// </summary>
    public class PrioritizationCalculationService : DomainService, IPrioritizationCalculationService
    {
        private readonly IPrioritizationActivityRepository _activityRepository;
        private readonly ILogger<PrioritizationCalculationService> _logger;

        public PrioritizationCalculationService(
            IPrioritizationActivityRepository activityRepository,
            ILogger<PrioritizationCalculationService> logger)
        {
            _activityRepository = activityRepository;
            _logger = logger;
        }

        public async Task<Dictionary<Guid, RankScore>> CalculatePrioritizationResultsAsync(
            Guid activityId,
            CancellationToken cancellationToken = default)
        {
            var activity = await _activityRepository.GetAsync(activityId, cancellationToken: cancellationToken);

            if (activity.Status != ActivityStatus.Completed)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:ActivityNotCompleted");
            }

            _logger.LogInformation(
                "Calculating prioritization results for activity {ActivityId} using method {Method}",
                activityId,
                activity.Method);

            // Delegate to the aggregate's calculation method
            var results = activity.CalculateResults();

            // Apply stakeholder influence if needed
            var stakeholderInfluence = await CalculateStakeholderInfluenceAsync(activityId, cancellationToken);
            if (stakeholderInfluence.Any())
            {
                results = ApplyStakeholderInfluence(results, activity, stakeholderInfluence);
            }

            _logger.LogInformation(
                "Prioritization results calculated for {RequirementCount} requirements",
                results.Count);

            return results;
        }

        public async Task<RankScore> CalculateIndividualScoreAsync(
            Guid activityId,
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var activity = await _activityRepository.GetAsync(activityId, cancellationToken: cancellationToken);
            var votes = activity.GetVotesForRequirement(requirementId);

            if (!votes.Any())
            {
                return new RankScore(0, int.MaxValue, 0, 0);
            }

            var scores = votes.Select(v => v.Vote.NumericValue).ToList();
            var average = scores.Average();
            var variance = CalculateVariance(scores);
            var confidence = CalculateConfidence(variance, scores.Count);

            return new RankScore(average, 0, confidence, scores.Count); // Rank will be calculated later
        }

        public async Task<decimal> CalculateConsensusAsync(
            Guid activityId,
            CancellationToken cancellationToken = default)
        {
            var activity = await _activityRepository.GetAsync(activityId, cancellationToken: cancellationToken);

            if (!activity.Votes.Any())
            {
                return 0;
            }

            // Calculate consensus as the inverse of average variance across all requirements
            var requirementVariances = new List<decimal>();

            foreach (var requirementId in activity.RequirementIds)
            {
                var votes = activity.GetVotesForRequirement(requirementId);
                if (votes.Count > 1)
                {
                    var scores = votes.Select(v => v.Vote.NumericValue).ToList();
                    var variance = CalculateVariance(scores);
                    requirementVariances.Add(variance);
                }
            }

            if (!requirementVariances.Any())
            {
                return 1; // Perfect consensus with single votes
            }

            var averageVariance = requirementVariances.Average();
            var consensus = Math.Max(0, 1 - (averageVariance / 4)); // Normalize to 0-1

            _logger.LogInformation(
                "Consensus calculated for activity {ActivityId}: {Consensus:F2}",
                activityId,
                consensus);

            return consensus;
        }

        public async Task<Dictionary<Guid, decimal>> CalculateStakeholderInfluenceAsync(
            Guid activityId,
            CancellationToken cancellationToken = default)
        {
            var activity = await _activityRepository.GetAsync(activityId, cancellationToken: cancellationToken);
            var stakeholderInfluence = new Dictionary<Guid, decimal>();

            // For now, give equal weight to all stakeholders
            // This could be enhanced to consider stakeholder roles, expertise, etc.
            foreach (var participantId in activity.ParticipantIds)
            {
                stakeholderInfluence[participantId] = 1.0m / activity.ParticipantIds.Count;
            }

            // Could enhance this with:
            // - Role-based weighting (Product Owner gets higher weight)
            // - Expertise-based weighting (Domain experts get higher weight)
            // - Historical accuracy weighting (Track stakeholder prediction accuracy)

            return stakeholderInfluence;
        }

        private static Dictionary<Guid, RankScore> ApplyStakeholderInfluence(
            Dictionary<Guid, RankScore> baseResults,
            PrioritizationActivity activity,
            Dictionary<Guid, decimal> stakeholderInfluence)
        {
            var adjustedResults = new Dictionary<Guid, decimal>();

            // Recalculate scores with stakeholder influence
            foreach (var requirementId in activity.RequirementIds)
            {
                var votes = activity.GetVotesForRequirement(requirementId);
                var weightedSum = 0m;
                var totalWeight = 0m;

                foreach (var vote in votes)
                {
                    var influence = stakeholderInfluence.GetValueOrDefault(vote.StakeholderId, 1.0m);
                    weightedSum += vote.Vote.NumericValue * influence;
                    totalWeight += influence;
                }

                var adjustedScore = totalWeight > 0 ? weightedSum / totalWeight : 0;
                adjustedResults[requirementId] = adjustedScore;
            }

            // Re-rank based on adjusted scores
            var sortedResults = adjustedResults
                .OrderByDescending(kvp => kvp.Value)
                .ToList();

            var finalResults = new Dictionary<Guid, RankScore>();
            for (int i = 0; i < sortedResults.Count; i++)
            {
                var requirementId = sortedResults[i].Key;
                var baseResult = baseResults[requirementId];
                
                finalResults[requirementId] = new RankScore(
                    sortedResults[i].Value,
                    i + 1,
                    baseResult.Confidence, // Keep original confidence
                    baseResult.VoteCount   // Keep original vote count
                );
            }

            return finalResults;
        }

        private static decimal CalculateVariance(List<decimal> scores)
        {
            if (scores.Count <= 1) return 0;
            
            var mean = scores.Average();
            var variance = scores.Select(x => Math.Pow((double)(x - mean), 2)).Average();
            return (decimal)variance;
        }

        private static decimal CalculateConfidence(decimal variance, int voteCount)
        {
            if (voteCount == 0) return 0;
            
            var normalizedVoteCount = Math.Min(1.0m, voteCount / 10.0m);
            var normalizedVariance = Math.Min(1.0m, variance / 4.0m);
            
            return normalizedVoteCount * (1 - normalizedVariance);
        }
    }
}

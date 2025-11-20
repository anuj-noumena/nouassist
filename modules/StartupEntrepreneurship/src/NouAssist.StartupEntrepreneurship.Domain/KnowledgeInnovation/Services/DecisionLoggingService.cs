namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Services
{
    /// <summary>
    /// Service interface for decision logging operations
    /// </summary>
    public interface IDecisionLoggingService
    {
        /// <summary>
        /// Create a new decision record
        /// </summary>
        Task<DecisionRecord> CreateDecisionRecordAsync(
            Guid id,
            string title,
            string background,
            string problemStatement,
            List<string> stakeholders,
            List<string> constraints,
            List<string> tags,
            Guid creatorId,
            Guid? tenantId = null);

        /// <summary>
        /// Add alternative to a decision record
        /// </summary>
        Task<DecisionAlternative> AddAlternativeAsync(
            Guid decisionId,
            string title,
            string description,
            string prosCons);

        /// <summary>
        /// Make a decision
        /// </summary>
        Task MakeDecisionAsync(
            Guid decisionId,
            string chosenAlternativeTitle,
            string reasoning,
            List<string> keyFactors,
            List<string> risksIdentified,
            List<string> assumptionsMade,
            DateTime decisionDate);

        /// <summary>
        /// Record implementation of a decision
        /// </summary>
        Task ImplementDecisionAsync(
            Guid decisionId,
            string description,
            DateTime implementationDate);

        /// <summary>
        /// Evaluate outcome of a decision
        /// </summary>
        Task EvaluateOutcomeAsync(
            Guid decisionId,
            string description,
            string lessonsLearned,
            List<OutcomeImpact> impacts);

        /// <summary>
        /// Reverse a decision
        /// </summary>
        Task ReverseDecisionAsync(
            Guid decisionId,
            string reason);

        /// <summary>
        /// Get decisions by status
        /// </summary>
        Task<List<DecisionRecord>> GetDecisionsByStatusAsync(DecisionStatus status);

        /// <summary>
        /// Get recent decisions
        /// </summary>
        Task<List<DecisionRecord>> GetRecentDecisionsAsync(DateTime sinceDate);

        /// <summary>
        /// Get decisions by tag
        /// </summary>
        Task<List<DecisionRecord>> GetDecisionsByTagAsync(string tag);

        /// <summary>
        /// Get decisions that can be implemented
        /// </summary>
        Task<List<DecisionRecord>> GetImplementableDecisionsAsync();
    }

    /// <summary>
    /// Implementation of decision logging service
    /// </summary>
    public class DecisionLoggingService : DomainService, IDecisionLoggingService
    {
        private readonly IDecisionRecordRepository _decisionRepository;
        private readonly ILogger<DecisionLoggingService> _logger;

        public DecisionLoggingService(
            IDecisionRecordRepository decisionRepository,
            ILogger<DecisionLoggingService> logger)
        {
            _decisionRepository = decisionRepository;
            _logger = logger;
        }

        public async Task<DecisionRecord> CreateDecisionRecordAsync(
            Guid id,
            string title,
            string background,
            string problemStatement,
            List<string> stakeholders,
            List<string> constraints,
            List<string> tags,
            Guid creatorId,
            Guid? tenantId = null)
        {
            try
            {
                var context = new DecisionContext(
                    id,
                    background,
                    problemStatement,
                    stakeholders,
                    constraints);
                
                var decision = DecisionRecord.Create(
                    id,
                    title,
                    context,
                    tags,
                    tenantId);
                
                decision.CreatorId = creatorId;
                
                await _decisionRepository.InsertAsync(decision);
                return decision;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating decision record with title: {Title}", title);
                throw;
            }
        }

        public async Task<DecisionAlternative> AddAlternativeAsync(
            Guid decisionId,
            string title,
            string description,
            string prosCons)
        {
            try
            {
                var decision = await _decisionRepository.GetAsync(decisionId);
                if (decision == null)
                {
                    throw new EntityNotFoundException("DecisionRecord", decisionId);
                }
                
                var alternative = decision.AddAlternative(title, description, prosCons);
                await _decisionRepository.UpdateAsync(decision);
                return alternative;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding alternative to decision {DecisionId}", decisionId);
                throw;
            }
        }

        public async Task MakeDecisionAsync(
            Guid decisionId,
            string chosenAlternativeTitle,
            string reasoning,
            List<string> keyFactors,
            List<string> risksIdentified,
            List<string> assumptionsMade,
            DateTime decisionDate)
        {
            try
            {
                var decision = await _decisionRepository.GetAsync(decisionId);
                if (decision == null)
                {
                    throw new EntityNotFoundException("DecisionRecord", decisionId);
                }
                
                var rationale = new DecisionRationale(
                    reasoning,
                    keyFactors,
                    risksIdentified,
                    assumptionsMade);
                
                decision.MakeDecision(chosenAlternativeTitle, rationale, decisionDate);
                await _decisionRepository.UpdateAsync(decision);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error making decision {DecisionId}", decisionId);
                throw;
            }
        }

        public async Task ImplementDecisionAsync(
            Guid decisionId,
            string description,
            DateTime implementationDate)
        {
            try
            {
                var decision = await _decisionRepository.GetAsync(decisionId);
                if (decision == null)
                {
                    throw new EntityNotFoundException("DecisionRecord", decisionId);
                }
                
                decision.ImplementDecision(description, implementationDate);
                await _decisionRepository.UpdateAsync(decision);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error implementing decision {DecisionId}", decisionId);
                throw;
            }
        }

        public async Task EvaluateOutcomeAsync(
            Guid decisionId,
            string description,
            string lessonsLearned,
            List<OutcomeImpact> impacts)
        {
            try
            {
                var decision = await _decisionRepository.GetAsync(decisionId);
                if (decision == null)
                {
                    throw new EntityNotFoundException("DecisionRecord", decisionId);
                }
                
                decision.EvaluateOutcome(description, lessonsLearned, impacts);
                await _decisionRepository.UpdateAsync(decision);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error evaluating outcome for decision {DecisionId}", decisionId);
                throw;
            }
        }

        public async Task ReverseDecisionAsync(
            Guid decisionId,
            string reason)
        {
            try
            {
                var decision = await _decisionRepository.GetAsync(decisionId);
                if (decision == null)
                {
                    throw new EntityNotFoundException("DecisionRecord", decisionId);
                }
                
                decision.ReverseDecision(reason);
                await _decisionRepository.UpdateAsync(decision);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reversing decision {DecisionId}", decisionId);
                throw;
            }
        }

        public async Task<List<DecisionRecord>> GetDecisionsByStatusAsync(DecisionStatus status)
        {
            try
            {
                return await _decisionRepository.GetByStatusAsync(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting decisions with status {Status}", status);
                throw;
            }
        }

        public async Task<List<DecisionRecord>> GetRecentDecisionsAsync(DateTime sinceDate)
        {
            try
            {
                return await _decisionRepository.GetRecentDecisionsAsync(sinceDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent decisions since {SinceDate}", sinceDate);
                throw;
            }
        }

        public async Task<List<DecisionRecord>> GetDecisionsByTagAsync(string tag)
        {
            try
            {
                return await _decisionRepository.GetByTagAsync(tag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting decisions with tag {Tag}", tag);
                throw;
            }
        }

        public async Task<List<DecisionRecord>> GetImplementableDecisionsAsync()
        {
            try
            {
                return await _decisionRepository.GetImplementableDecisionsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting implementable decisions");
                throw;
            }
        }
    }
}

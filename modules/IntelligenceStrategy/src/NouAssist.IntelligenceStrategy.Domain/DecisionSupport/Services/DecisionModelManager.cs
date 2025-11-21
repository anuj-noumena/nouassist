using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Entities;
using NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Services
{
    /// <summary>
    /// Domain service for managing decision models
    /// </summary>
    public class DecisionModelManager : DomainService, IDecisionModelManager
    {
        private readonly IRepository<DecisionModelInstance, Guid> _decisionModelRepository;
        private readonly ILogger<DecisionModelManager> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="DecisionModelManager"/>
        /// </summary>
        public DecisionModelManager(
            IRepository<DecisionModelInstance, Guid> decisionModelRepository,
            ILogger<DecisionModelManager> logger)
        {
            _decisionModelRepository = decisionModelRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<DecisionModelInstance> CreateModelAsync(
            string title,
            string description,
            string modelType,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if a model with the same title already exists
                var existingModel = await _decisionModelRepository.FirstOrDefaultAsync(
                    dm => dm.Title == title && dm.TenantId == CurrentTenant.Id,
                    cancellationToken);
                    
                if (existingModel != null)
                {
                    throw new DecisionSupportDomainException(
                        "DecisionSupport:ErrorMessage:DuplicateName",
                        $"A decision model with the name '{title}' already exists")
                        .WithData("name", title);
                }
                
                // Create the new decision model
                var decisionModel = DecisionModelInstance.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    modelType,
                    CurrentTenant.Id);
                    
                // Save the decision model
                await _decisionModelRepository.InsertAsync(decisionModel, true, cancellationToken);
                
                _logger.LogInformation(
                    "Created decision model {DecisionModelId} with title {Title}",
                    decisionModel.Id,
                    decisionModel.Title);
                    
                return decisionModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create decision model with title {Title}",
                    title);
                    
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Criterion> AddCriterionAsync(
            Guid decisionModelId,
            string name,
            string description,
            decimal weight,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var decisionModel = await _decisionModelRepository.GetAsync(decisionModelId, cancellationToken: cancellationToken)
                    ?? throw new EntityNotFoundException(nameof(DecisionModelInstance), decisionModelId);
                    
                var criterion = decisionModel.AddCriterion(name, description, weight);
                
                await _decisionModelRepository.UpdateAsync(decisionModel, true, cancellationToken);
                
                _logger.LogInformation(
                    "Added criterion {CriterionId} to decision model {DecisionModelId}",
                    criterion.Id,
                    decisionModelId);
                    
                return criterion;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to add criterion to decision model {DecisionModelId}",
                    decisionModelId);
                    
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Alternative> AddAlternativeAsync(
            Guid decisionModelId,
            string name,
            string description,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var decisionModel = await _decisionModelRepository.GetAsync(decisionModelId, cancellationToken: cancellationToken)
                    ?? throw new EntityNotFoundException(nameof(DecisionModelInstance), decisionModelId);
                    
                var alternative = decisionModel.AddAlternative(name, description);
                
                await _decisionModelRepository.UpdateAsync(decisionModel, true, cancellationToken);
                
                _logger.LogInformation(
                    "Added alternative {AlternativeId} to decision model {DecisionModelId}",
                    alternative.Id,
                    decisionModelId);
                    
                return alternative;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to add alternative to decision model {DecisionModelId}",
                    decisionModelId);
                    
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task SetScoreAsync(
            Guid decisionModelId,
            Guid alternativeId,
            Guid criterionId,
            decimal score,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var decisionModel = await _decisionModelRepository.GetAsync(decisionModelId, cancellationToken: cancellationToken)
                    ?? throw new EntityNotFoundException(nameof(DecisionModelInstance), decisionModelId);
                    
                decisionModel.SetScore(alternativeId, criterionId, score);
                
                await _decisionModelRepository.UpdateAsync(decisionModel, true, cancellationToken);
                
                _logger.LogInformation(
                    "Set score for alternative {AlternativeId} on criterion {CriterionId} in decision model {DecisionModelId}",
                    alternativeId,
                    criterionId,
                    decisionModelId);
                    
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to set score in decision model {DecisionModelId}",
                    decisionModelId);
                    
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DecisionModelInstance> FinalizeDecisionAsync(
            Guid decisionModelId,
            Guid selectedAlternativeId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var decisionModel = await _decisionModelRepository.GetAsync(decisionModelId, cancellationToken: cancellationToken)
                    ?? throw new EntityNotFoundException(nameof(DecisionModelInstance), decisionModelId);
                    
                decisionModel.FinalizeDecision(selectedAlternativeId);
                
                await _decisionModelRepository.UpdateAsync(decisionModel, true, cancellationToken);
                
                _logger.LogInformation(
                    "Finalized decision model {DecisionModelId} with selected alternative {AlternativeId}",
                    decisionModelId,
                    selectedAlternativeId);
                    
                return decisionModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to finalize decision model {DecisionModelId}",
                    decisionModelId);
                    
                throw;
            }
        }
    }
}

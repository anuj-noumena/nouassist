using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services;
using NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Services
{
    /// <summary>
    /// Domain service for managing strategic initiatives
    /// </summary>
    public class StrategicInitiativeManager : DomainService, IStrategicInitiativeManager
    {
        private readonly IRepository<StrategicInitiativeProposal, Guid> _initiativeRepository;
        private readonly IRepository<DecisionModelInstance, Guid> _decisionModelRepository;
        private readonly ILogger<StrategicInitiativeManager> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="StrategicInitiativeManager"/>
        /// </summary>
        public StrategicInitiativeManager(
            IRepository<StrategicInitiativeProposal, Guid> initiativeRepository,
            IRepository<DecisionModelInstance, Guid> decisionModelRepository,
            ILogger<StrategicInitiativeManager> logger)
        {
            _initiativeRepository = initiativeRepository;
            _decisionModelRepository = decisionModelRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<StrategicInitiativeProposal> CreateProposalAsync(
            string title,
            string description,
            DecisionRationaleText rationale,
            ExpectedOutcomeText expectedOutcomes,
            Guid? relatedDecisionModelId = null,
            AlignmentStatus strategicAlignment = AlignmentStatus.NotEvaluated,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if a decision model reference is provided and valid
                if (relatedDecisionModelId.HasValue)
                {
                    var decisionModelExists = await _decisionModelRepository.AnyAsync(
                        dm => dm.Id == relatedDecisionModelId.Value,
                        cancellationToken);
                        
                    if (!decisionModelExists)
                    {
                        throw new EntityNotFoundException(
                            nameof(DecisionModelInstance),
                            relatedDecisionModelId.Value);
                    }
                }
                
                // Check if a proposal with the same title already exists
                var existingProposal = await _initiativeRepository.FirstOrDefaultAsync(
                    p => p.Title == title && p.TenantId == CurrentTenant.Id,
                    cancellationToken);
                    
                if (existingProposal != null)
                {
                    throw new DecisionSupportDomainException(
                        "DecisionSupport:ErrorMessage:DuplicateName",
                        $"A strategic initiative with the name '{title}' already exists")
                        .WithData("name", title);
                }
                
                // Create the new proposal
                var initiative = StrategicInitiativeProposal.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    rationale,
                    expectedOutcomes,
                    relatedDecisionModelId,
                    strategicAlignment,
                    CurrentTenant.Id);
                    
                // Save the proposal
                await _initiativeRepository.InsertAsync(initiative, true, cancellationToken);
                
                _logger.LogInformation(
                    "Created strategic initiative proposal {ProposalId} with title {Title}",
                    initiative.Id,
                    initiative.Title);
                    
                return initiative;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create strategic initiative proposal with title {Title}",
                    title);
                    
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<StrategicInitiativeProposal> ApproveProposalAsync(
            Guid proposalId,
            Guid? projectId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var initiative = await _initiativeRepository.GetAsync(proposalId, cancellationToken)
                    ?? throw new InitiativeNotFoundException(proposalId);
                    
                initiative.Approve(CurrentUser.Id.Value, projectId);
                
                await _initiativeRepository.UpdateAsync(initiative, true, cancellationToken);
                
                _logger.LogInformation(
                    "Approved strategic initiative {ProposalId} with title {Title}",
                    initiative.Id,
                    initiative.Title);
                    
                return initiative;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to approve strategic initiative {ProposalId}",
                    proposalId);
                    
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<StrategicInitiativeProposal> RejectProposalAsync(
            Guid proposalId,
            string rejectionReason,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var initiative = await _initiativeRepository.GetAsync(proposalId, cancellationToken)
                    ?? throw new InitiativeNotFoundException(proposalId);
                    
                initiative.Reject(rejectionReason, CurrentUser.Id.Value);
                
                await _initiativeRepository.UpdateAsync(initiative, true, cancellationToken);
                
                _logger.LogInformation(
                    "Rejected strategic initiative {ProposalId} with title {Title}",
                    initiative.Id,
                    initiative.Title);
                    
                return initiative;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to reject strategic initiative {ProposalId}",
                    proposalId);
                    
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<StrategicInitiativeProposal> CreateFromDecisionModelAsync(
            Guid decisionModelId,
            string title,
            string description,
            DecisionRationaleText rationale,
            ExpectedOutcomeText expectedOutcomes,
            AlignmentStatus alignment = AlignmentStatus.NotEvaluated,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var decisionModel = await _decisionModelRepository.GetAsync(decisionModelId, cancellationToken)
                    ?? throw new EntityNotFoundException(nameof(DecisionModelInstance), decisionModelId);
                    
                // Check if the decision has been finalized
                if (!decisionModel.IsFinalized)
                {
                    throw new DecisionSupportDomainException(
                        "DecisionSupport:ErrorMessage:DecisionNotFinalized",
                        "Cannot create an initiative from a decision model that hasn't been finalized");
                }
                
                // Create the initiative through the decision model
                var initiative = decisionModel.ProposeInitiative(
                    title,
                    description,
                    rationale,
                    expectedOutcomes,
                    alignment);
                    
                // Save the initiative
                await _initiativeRepository.InsertAsync(initiative, true, cancellationToken);
                
                _logger.LogInformation(
                    "Created strategic initiative {ProposalId} from decision model {DecisionModelId}",
                    initiative.Id,
                    decisionModelId);
                    
                return initiative;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create strategic initiative from decision model {DecisionModelId}",
                    decisionModelId);
                    
                throw;
            }
        }
    }
}

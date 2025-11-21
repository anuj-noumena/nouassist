using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Managers
{
    /// <summary>
    /// Manager for FundraisingRound aggregate - handles complex business logic
    /// </summary>
    public class FundraisingRoundManager : DomainService
    {
        private readonly IFundraisingRoundRepository _roundRepository;
        private readonly ILogger<FundraisingRoundManager> _logger;

        public FundraisingRoundManager(
            IFundraisingRoundRepository roundRepository,
            ILogger<FundraisingRoundManager> logger)
        {
            _roundRepository = roundRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new fundraising round with validation
        /// </summary>
        public async Task<FundraisingRound> CreateAsync(
            string name,
            RoundType type,
            decimal targetAmount,
            DateTime startDate,
            DateTime? targetCloseDate = null,
            string description = null)
        {
            // Check for duplicate name within same tenant
            var existingRound = await _roundRepository.FindByNameAsync(
                name,
                CurrentTenant.Id);

            if (existingRound != null)
            {
                throw new FundraisingPipelineDomainException(
                    "FundraisingPipeline:DuplicateName",
                    $"A fundraising round with name '{name}' already exists.");
            }

            // Validate target amount
            if (targetAmount <= 0)
            {
                throw new FundraisingPipelineDomainException(
                    "FundraisingPipeline:InvalidTargetAmount",
                    "Target amount must be greater than zero.");
            }

            // Validate dates
            if (targetCloseDate.HasValue && targetCloseDate.Value <= startDate)
            {
                throw new FundraisingPipelineDomainException(
                    "FundraisingPipeline:InvalidDateRange",
                    "Target close date must be after start date.");
            }

            var round = FundraisingRound.Create(
                GuidGenerator.Create(),
                name,
                type,
                targetAmount,
                startDate,
                targetCloseDate,
                description,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new fundraising round {RoundId} with name '{Name}' and target {TargetAmount:C}",
                round.Id, round.Name, round.TargetAmount);

            return round;
        }

        /// <summary>
        /// Updates fundraising round details
        /// </summary>
        public async Task<FundraisingRound> UpdateAsync(
            Guid roundId,
            string? newName = null,
            string? newDescription = null,
            decimal? newTargetAmount = null,
            DateTime? newTargetCloseDate = null)
        {
            var round = await _roundRepository.GetAsync(roundId);

            // Check for duplicate name if name is being changed
            if (!string.IsNullOrWhiteSpace(newName) && round.Name != newName)
            {
                var existingRound = await _roundRepository.FindByNameAsync(
                    newName,
                    CurrentTenant.Id);

                if (existingRound != null && existingRound.Id != roundId)
                {
                    throw new FundraisingPipelineDomainException(
                        "FundraisingPipeline:DuplicateName",
                        $"A fundraising round with name '{newName}' already exists.");
                }
            }

            round.UpdateBasicInfo(
                newName ?? round.Name,
                newDescription ?? round.Description,
                newTargetAmount ?? round.TargetAmount,
                newTargetCloseDate ?? round.TargetCloseDate);

            await _roundRepository.UpdateAsync(round);

            _logger.LogInformation(
                "Updated fundraising round {RoundId}",
                roundId);

            return round;
        }

        /// <summary>
        /// Advances the round to the next stage
        /// </summary>
        public async Task<FundraisingRound> AdvanceStageAsync(
            Guid roundId,
            RoundStage newStage)
        {
            var round = await _roundRepository.GetAsync(roundId);

            // Validate stage transition
            if (!CanTransitionToStage(round.Stage, newStage))
            {
                throw new InvalidRoundStageTransitionException(
                    $"Cannot transition from {round.Stage} to {newStage}");
            }

            round.AdvanceStage(newStage);
            await _roundRepository.UpdateAsync(round);

            _logger.LogInformation(
                "Advanced fundraising round {RoundId} to stage {NewStage}",
                roundId, newStage);

            return round;
        }

        /// <summary>
        /// Closes the fundraising round
        /// </summary>
        public async Task<FundraisingRound> CloseRoundAsync(
            Guid roundId,
            DateTime? actualCloseDate = null)
        {
            var round = await _roundRepository.GetAsync(roundId);

            // Business rule: Can only close if in Active or Closing stage
            if (round.Stage != RoundStage.Active && round.Stage != RoundStage.Closing)
            {
                throw new FundraisingPipelineDomainException(
                    "FundraisingPipeline:InvalidStageForClosing",
                    $"Cannot close round in {round.Stage} stage.");
            }

            round.CloseRound(actualCloseDate ?? Clock.Now);
            await _roundRepository.UpdateAsync(round);

            _logger.LogInformation(
                "Closed fundraising round {RoundId} with committed amount {CommittedAmount:C}",
                roundId, round.CommittedAmount);

            return round;
        }

        /// <summary>
        /// Validates if a stage transition is allowed
        /// </summary>
        private bool CanTransitionToStage(RoundStage currentStage, RoundStage newStage)
        {
            // Define valid stage transitions
            return (currentStage, newStage) switch
            {
                (RoundStage.Planning, RoundStage.Active) => true,
                (RoundStage.Active, RoundStage.Closing) => true,
                (RoundStage.Active, RoundStage.Closed) => true,
                (RoundStage.Closing, RoundStage.Closed) => true,
                _ => false
            };
        }

        /// <summary>
        /// Deletes a fundraising round (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid roundId, bool hardDelete = false)
        {
            var round = await _roundRepository.GetAsync(roundId);

            // Business rule: Cannot delete closed rounds
            if (round.Stage == RoundStage.Closed && !hardDelete)
            {
                throw new FundraisingPipelineDomainException(
                    "FundraisingPipeline:CannotDeleteClosedRound",
                    "Cannot delete a closed round. Use hard delete if necessary.");
            }

            if (hardDelete)
            {
                await _roundRepository.DeleteAsync(round);
                _logger.LogWarning("Hard deleted fundraising round {RoundId}", roundId);
            }
            else
            {
                await _roundRepository.DeleteAsync(round, autoSave: true);
                _logger.LogInformation("Soft deleted fundraising round {RoundId}", roundId);
            }
        }
    }
}

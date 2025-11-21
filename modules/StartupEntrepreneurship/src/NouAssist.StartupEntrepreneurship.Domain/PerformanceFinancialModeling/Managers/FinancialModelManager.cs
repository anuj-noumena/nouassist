using System;
using System.Threading.Task;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Exceptions;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Managers
{
    /// <summary>
    /// Manager for FinancialModel aggregate - handles complex business logic
    /// </summary>
    public class FinancialModelManager : DomainService
    {
        private readonly IFinancialModelRepository _financialModelRepository;
        private readonly ILogger<FinancialModelManager> _logger;

        public FinancialModelManager(
            IFinancialModelRepository financialModelRepository,
            ILogger<FinancialModelManager> logger)
        {
            _financialModelRepository = financialModelRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new financial model with validation
        /// </summary>
        public async Task<FinancialModel> CreateAsync(
            string name,
            string description,
            DateTime startDate,
            DateTime endDate,
            Guid ownerId)
        {
            // Validate date range
            if (endDate <= startDate)
            {
                throw new PerformanceFinancialModelingDomainException(
                    "PerformanceFinancialModeling:InvalidDateRange",
                    "End date must be after start date.");
            }

            // Check for duplicate name
            var existing = await _financialModelRepository.FindByNameAsync(
                name,
                CurrentTenant.Id);

            if (existing != null)
            {
                throw new DuplicateMetricDefinitionException(
                    $"A financial model with name '{name}' already exists.");
            }

            var model = FinancialModel.Create(
                GuidGenerator.Create(),
                name,
                description,
                startDate,
                endDate,
                ownerId,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new financial model {ModelId} with name '{Name}'",
                model.Id, model.Name);

            return model;
        }

        /// <summary>
        /// Updates financial model with validation
        /// </summary>
        public async Task<FinancialModel> UpdateAsync(
            Guid modelId,
            string newName = null,
            string newDescription = null)
        {
            var model = await _financialModelRepository.GetAsync(modelId);

            // Check for duplicate name if being changed
            if (!string.IsNullOrWhiteSpace(newName) && model.Name != newName)
            {
                var existing = await _financialModelRepository.FindByNameAsync(
                    newName,
                    CurrentTenant.Id);

                if (existing != null && existing.Id != modelId)
                {
                    throw new DuplicateMetricDefinitionException(
                        $"A financial model with name '{newName}' already exists.");
                }
            }

            model.UpdateBasicInfo(
                newName ?? model.Name,
                newDescription ?? model.Description);

            await _financialModelRepository.UpdateAsync(model);

            _logger.LogInformation(
                "Updated financial model {ModelId}",
                modelId);

            return model;
        }

        /// <summary>
        /// Finalizes a financial model
        /// </summary>
        public async Task<FinancialModel> FinalizeAsync(
            Guid modelId,
            Guid finalizedBy)
        {
            var model = await _financialModelRepository.GetAsync(modelId);

            // Business rule: Cannot finalize an already finalized model
            if (model.IsFinalized)
            {
                throw new PerformanceFinancialModelingDomainException(
                    "PerformanceFinancialModeling:AlreadyFinalized",
                    "Financial model is already finalized.");
            }

            // Business rule: Must have at least one scenario
            if (model.Scenarios.Count == 0)
            {
                throw new InvalidScenarioException(
                    "Cannot finalize a model without any scenarios.");
            }

            model.Finalize(finalizedBy, Clock.Now);
            await _financialModelRepository.UpdateAsync(model);

            _logger.LogInformation(
                "Finalized financial model {ModelId} by {FinalizedBy}",
                modelId, finalizedBy);

            return model;
        }

        /// <summary>
        /// Deletes a financial model (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid modelId, bool hardDelete = false)
        {
            var model = await _financialModelRepository.GetAsync(modelId);

            // Business rule: Cannot delete finalized models
            if (model.IsFinalized && !hardDelete)
            {
                throw new PerformanceFinancialModelingDomainException(
                    "PerformanceFinancialModeling:CannotDeleteFinalizedModel",
                    "Cannot delete a finalized model. Use hard delete if necessary.");
            }

            if (hardDelete)
            {
                await _financialModelRepository.DeleteAsync(model);
                _logger.LogWarning("Hard deleted financial model {ModelId}", modelId);
            }
            else
            {
                await _financialModelRepository.DeleteAsync(model, autoSave: true);
                _logger.LogInformation("Soft deleted financial model {ModelId}", modelId);
            }
        }
    }
}

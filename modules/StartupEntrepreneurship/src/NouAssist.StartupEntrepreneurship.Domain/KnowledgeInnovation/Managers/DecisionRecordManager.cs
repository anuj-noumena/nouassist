using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Managers
{
    /// <summary>
    /// Manager for DecisionRecord aggregate - handles complex business logic
    /// </summary>
    public class DecisionRecordManager : DomainService
    {
        private readonly IDecisionRecordRepository _decisionRecordRepository;
        private readonly ILogger<DecisionRecordManager> _logger;

        public DecisionRecordManager(
            IDecisionRecordRepository decisionRecordRepository,
            ILogger<DecisionRecordManager> logger)
        {
            _decisionRecordRepository = decisionRecordRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new decision record with validation
        /// </summary>
        public async Task<DecisionRecord> CreateAsync(
            string title,
            string context,
            string decision,
            Guid decisionMakerId,
            DateTime decisionDate,
            string category = null)
        {
            // Check for duplicate title
            var existing = await _decisionRecordRepository.FindByTitleAsync(
                title,
                CurrentTenant.Id);

            if (existing != null)
            {
                throw new DuplicateTitleException(
                    $"A decision record with title '{title}' already exists.");
            }

            // Validate decision date is not in the future
            if (decisionDate > Clock.Now)
            {
                throw new KnowledgeInnovationDomainException(
                    "KnowledgeInnovation:InvalidDecisionDate",
                    "Decision date cannot be in the future.");
            }

            var record = DecisionRecord.Create(
                GuidGenerator.Create(),
                title,
                context,
                decision,
                decisionMakerId,
                decisionDate,
                category,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new decision record {RecordId} with title '{Title}'",
                record.Id, record.Title);

            return record;
        }

        /// <summary>
        /// Updates decision record with validation
        /// </summary>
        public async Task<DecisionRecord> UpdateAsync(
            Guid recordId,
            string newTitle = null,
            string newContext = null,
            string newDecision = null)
        {
            var record = await _decisionRecordRepository.GetAsync(recordId);

            // Business rule: Cannot update superseded or deprecated records
            if (record.Status == DecisionStatus.Superseded || record.Status == DecisionStatus.Deprecated)
            {
                throw new InvalidStatusTransitionException(
                    $"Cannot update a {record.Status.ToString().ToLower()} decision record.");
            }

            // Check for duplicate title if being changed
            if (!string.IsNullOrWhiteSpace(newTitle) && record.Title != newTitle)
            {
                var existing = await _decisionRecordRepository.FindByTitleAsync(
                    newTitle,
                    CurrentTenant.Id);

                if (existing != null && existing.Id != recordId)
                {
                    throw new DuplicateTitleException(
                        $"A decision record with title '{newTitle}' already exists.");
                }
            }

            record.UpdateBasicInfo(
                newTitle ?? record.Title,
                newContext ?? record.Context,
                newDecision ?? record.Decision);

            await _decisionRecordRepository.UpdateAsync(record);

            _logger.LogInformation(
                "Updated decision record {RecordId}",
                recordId);

            return record;
        }

        /// <summary>
        /// Supersedes a decision record with a new one
        /// </summary>
        public async Task<DecisionRecord> SupersedeAsync(
            Guid recordId,
            Guid supersededBy,
            string reason)
        {
            var record = await _decisionRecordRepository.GetAsync(recordId);

            // Business rule: Can only supersede active or accepted records
            if (record.Status != DecisionStatus.Active && record.Status != DecisionStatus.Accepted)
            {
                throw new InvalidStatusTransitionException(
                    $"Can only supersede active or accepted records. Current status: {record.Status}");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new KnowledgeInnovationDomainException(
                    "KnowledgeInnovation:ReasonRequired",
                    "A reason must be provided for superseding a decision record.");
            }

            record.Supersede(supersededBy, reason, Clock.Now);
            await _decisionRecordRepository.UpdateAsync(record);

            _logger.LogInformation(
                "Superseded decision record {RecordId} by {SupersededBy}. Reason: {Reason}",
                recordId, supersededBy, reason);

            return record;
        }

        /// <summary>
        /// Deprecates a decision record
        /// </summary>
        public async Task<DecisionRecord> DeprecateAsync(
            Guid recordId,
            string reason)
        {
            var record = await _decisionRecordRepository.GetAsync(recordId);

            // Business rule: Cannot deprecate already superseded or deprecated records
            if (record.Status == DecisionStatus.Superseded || record.Status == DecisionStatus.Deprecated)
            {
                throw new InvalidStatusTransitionException(
                    $"Cannot deprecate a {record.Status.ToString().ToLower()} record.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new KnowledgeInnovationDomainException(
                    "KnowledgeInnovation:ReasonRequired",
                    "A reason must be provided for deprecating a decision record.");
            }

            record.Deprecate(reason, Clock.Now);
            await _decisionRecordRepository.UpdateAsync(record);

            _logger.LogInformation(
                "Deprecated decision record {RecordId}. Reason: {Reason}",
                recordId, reason);

            return record;
        }

        /// <summary>
        /// Deletes a decision record (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid recordId, bool hardDelete = false)
        {
            var record = await _decisionRecordRepository.GetAsync(recordId);

            // Business rule: Cannot delete active or accepted records
            if ((record.Status == DecisionStatus.Active || record.Status == DecisionStatus.Accepted) && !hardDelete)
            {
                throw new KnowledgeInnovationDomainException(
                    "KnowledgeInnovation:CannotDeleteActiveDecision",
                    "Cannot delete an active or accepted decision record. Deprecate it first or use hard delete.");
            }

            if (hardDelete)
            {
                await _decisionRecordRepository.DeleteAsync(record);
                _logger.LogWarning("Hard deleted decision record {RecordId}", recordId);
            }
            else
            {
                await _decisionRecordRepository.DeleteAsync(record, autoSave: true);
                _logger.LogInformation("Soft deleted decision record {RecordId}", recordId);
            }
        }
    }
}

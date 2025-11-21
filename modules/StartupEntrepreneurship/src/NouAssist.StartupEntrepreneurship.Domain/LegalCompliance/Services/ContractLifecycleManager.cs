using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Services
{
    /// <summary>
    /// Implementation of the contract lifecycle management service
    /// </summary>
    public class ContractLifecycleManager : DomainService, IContractLifecycleManager
    {
        private readonly ILegalAgreementRepository _agreementRepository;
        private readonly ILogger<ContractLifecycleManager> _logger;

        public ContractLifecycleManager(
            ILegalAgreementRepository agreementRepository,
            ILogger<ContractLifecycleManager> logger)
        {
            _agreementRepository = agreementRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<LegalAgreement> CreateAgreementAsync(
            string title,
            string description,
            LegalAgreementType type,
            string counterpartyName,
            DateTime effectiveDate,
            DateTime? expirationDate,
            string referenceNumber,
            string country,
            string state = null,
            decimal? value = null,
            string currency = null)
        {
            try
            {
                var jurisdiction = new Jurisdiction(country, state);

                var agreement = LegalAgreement.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    type,
                    counterpartyName,
                    effectiveDate,
                    expirationDate,
                    referenceNumber,
                    jurisdiction,
                    value,
                    currency,
                    CurrentTenant.Id);

                await _agreementRepository.InsertAsync(agreement);

                return agreement;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create legal agreement {Title} of type {Type}", title, type);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<LegalAgreement> UpdateAgreementAsync(
            Guid id,
            string title,
            string description,
            string counterpartyName,
            string referenceNumber,
            string country,
            string state = null,
            decimal? value = null,
            string currency = null)
        {
            try
            {
                var agreement = await _agreementRepository.GetAsync(id);
                var jurisdiction = new Jurisdiction(country, state);

                agreement.Update(
                    title,
                    description,
                    counterpartyName,
                    referenceNumber,
                    jurisdiction,
                    value,
                    currency);

                await _agreementRepository.UpdateAsync(agreement);

                return agreement;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update legal agreement {Id}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateAgreementStatusAsync(
            Guid id,
            FilingStatusType newStatus,
            string notes = null)
        {
            try
            {
                var agreement = await _agreementRepository.GetAsync(id);
                agreement.UpdateStatus(newStatus, Clock.Now, notes);

                await _agreementRepository.UpdateAsync(agreement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update status for legal agreement {Id} to {Status}", id, newStatus);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AddAgreementTermAsync(
            Guid agreementId,
            string title,
            string description,
            bool isCritical = false,
            DateTime? deadlineDate = null)
        {
            try
            {
                var agreement = await _agreementRepository.GetAsync(agreementId);
                agreement.AddTerm(title, description, isCritical, deadlineDate);

                await _agreementRepository.UpdateAsync(agreement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add term to legal agreement {Id}", agreementId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AddAgreementReminderAsync(
            Guid agreementId,
            string title,
            DateTime reminderDate,
            string description = null,
            int? daysBeforeEvent = null,
            bool isRecurring = false,
            string recurrencePattern = null)
        {
            try
            {
                var agreement = await _agreementRepository.GetAsync(agreementId);
                agreement.AddReminder(
                    title,
                    reminderDate,
                    description,
                    daysBeforeEvent,
                    isRecurring,
                    recurrencePattern);

                await _agreementRepository.UpdateAsync(agreement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add reminder to legal agreement {Id}", agreementId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<LegalAgreement> RenewAgreementAsync(
            Guid id,
            DateTime newEffectiveDate,
            DateTime? newExpirationDate,
            string notes = null)
        {
            try
            {
                var agreement = await _agreementRepository.GetAsync(id);
                agreement.Renew(newEffectiveDate, newExpirationDate, notes);

                await _agreementRepository.UpdateAsync(agreement);

                return agreement;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to renew legal agreement {Id}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task TerminateAgreementAsync(
            Guid id,
            DateTime terminationDate,
            string reason)
        {
            try
            {
                var agreement = await _agreementRepository.GetAsync(id);
                agreement.Terminate(terminationDate, reason);

                await _agreementRepository.UpdateAsync(agreement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to terminate legal agreement {Id}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<LegalAgreement>> GetExpiringAgreementsAsync(int daysThreshold)
        {
            try
            {
                return await _agreementRepository.GetExpiringAgreementsAsync(daysThreshold);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get expiring agreements");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<LegalAgreement>> GetActiveAgreementsAsync(DateTime referenceDate)
        {
            try
            {
                return await _agreementRepository.GetActiveAgreementsAsync(referenceDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get active agreements");
                throw;
            }
        }
    }
}

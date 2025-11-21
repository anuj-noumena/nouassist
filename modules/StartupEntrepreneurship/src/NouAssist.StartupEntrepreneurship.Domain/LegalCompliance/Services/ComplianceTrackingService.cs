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
    /// Implementation of the compliance tracking service
    /// </summary>
    public class ComplianceTrackingService : DomainService, IComplianceTrackingService
    {
        private readonly IComplianceObligationRepository _obligationRepository;
        private readonly ILogger<ComplianceTrackingService> _logger;

        public ComplianceTrackingService(
            IComplianceObligationRepository obligationRepository,
            ILogger<ComplianceTrackingService> logger)
        {
            _obligationRepository = obligationRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ComplianceObligation> CreateObligationAsync(
            string title,
            string overview,
            string requirements,
            string notes,
            string country,
            string state,
            string authorityName,
            DateTime? deadline = null,
            string referenceNumber = null,
            bool isRecurring = false,
            string recurrencePattern = null)
        {
            try
            {
                var description = new ObligationDescription(overview, requirements, notes);
                var jurisdiction = new Jurisdiction(country, state);

                var obligation = ComplianceObligation.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    jurisdiction,
                    authorityName,
                    deadline,
                    referenceNumber,
                    isRecurring,
                    recurrencePattern,
                    CurrentTenant.Id);

                await _obligationRepository.InsertAsync(obligation);

                return obligation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create compliance obligation {Title}", title);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ComplianceObligation> UpdateObligationAsync(
            Guid id,
            string title,
            string overview,
            string requirements,
            string notes,
            string country,
            string state,
            string authorityName,
            DateTime? deadline = null,
            string referenceNumber = null,
            bool isRecurring = false,
            string recurrencePattern = null)
        {
            try
            {
                var obligation = await _obligationRepository.GetAsync(id);
                
                var description = new ObligationDescription(overview, requirements, notes);
                var jurisdiction = new Jurisdiction(country, state);

                obligation.Update(
                    title,
                    description,
                    jurisdiction,
                    authorityName,
                    deadline,
                    referenceNumber,
                    isRecurring,
                    recurrencePattern);

                await _obligationRepository.UpdateAsync(obligation);

                return obligation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update compliance obligation {Id}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateObligationStatusAsync(
            Guid id,
            ComplianceStatus newStatus,
            string notes = null)
        {
            try
            {
                var obligation = await _obligationRepository.GetAsync(id);
                obligation.UpdateStatus(newStatus, Clock.Now, notes);

                await _obligationRepository.UpdateAsync(obligation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update status for compliance obligation {Id} to {Status}", id, newStatus);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<ComplianceObligation>> GetUpcomingObligationsAsync(int daysThreshold)
        {
            try
            {
                return await _obligationRepository.GetWithApproachingDeadlinesAsync(daysThreshold);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get upcoming obligations");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<ComplianceObligation>> GetOverdueObligationsAsync()
        {
            try
            {
                return await _obligationRepository.GetOverdueObligationsAsync(Clock.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get overdue obligations");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<int> GenerateRecurringObligationsAsync()
        {
            try
            {
                int generatedCount = 0;
                var completedObligations = await _obligationRepository.GetByStatusAsync(ComplianceStatus.Completed);
                
                foreach (var obligation in completedObligations)
                {
                    if (obligation.IsRecurring)
                    {
                        var nextObligation = obligation.GenerateNextRecurringObligation();
                        if (nextObligation != null)
                        {
                            await _obligationRepository.InsertAsync(nextObligation);
                            generatedCount++;
                        }
                    }
                }
                
                return generatedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate recurring obligations");
                throw;
            }
        }
    }
}

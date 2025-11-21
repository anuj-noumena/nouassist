using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Specifications;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Services
{
    /// <summary>
    /// Service for monitoring contract status and alerts
    /// </summary>
    public class ContractStatusMonitor : DomainService, IContractStatusMonitor
    {
        private readonly IContractRepository _contractRepository;
        private readonly IContractManager _contractManager;
        private readonly ILogger<ContractStatusMonitor> _logger;

        public ContractStatusMonitor(
            IContractRepository contractRepository,
            IContractManager contractManager,
            ILogger<ContractStatusMonitor> logger)
        {
            _contractRepository = contractRepository;
            _contractManager = contractManager;
            _logger = logger;
        }

        public async Task ProcessAlertsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var currentDate = Clock.Now;
                var contractsWithAlerts = await _contractRepository.GetWithDueAlertsAsync(currentDate);

                foreach (var contract in contractsWithAlerts)
                {
                    foreach (var alert in contract.Alerts)
                    {
                        if (alert.ShouldTrigger(currentDate))
                        {
                            // In a real implementation, this would publish alerts through a notification system
                            _logger.LogInformation("Alert triggered: {AlertTitle} for contract {ContractId}", 
                                alert.Title, contract.Id);
                            
                            alert.MarkAsTriggered(currentDate);
                        }
                    }

                    await _contractRepository.UpdateAsync(contract, cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing contract alerts");
                throw;
            }
        }

        public async Task UpdateContractStatusesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var currentDate = Clock.Now;
                var expiredContracts = await GetExpiredContractsAsync();

                foreach (var contract in expiredContracts)
                {
                    await _contractManager.ChangeContractStatusAsync(contract.Id, ContractStatus.Expired);
                    _logger.LogInformation("Contract {ContractId} status changed to Expired", contract.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating contract statuses");
                throw;
            }
        }

        public async Task<List<ContractAlert>> GetDueAlertsAsync(Guid? tenantId = null)
        {
            try
            {
                var currentDate = Clock.Now;
                var contractsWithAlerts = await _contractRepository.GetWithDueAlertsAsync(currentDate, tenantId);
                var dueAlerts = new List<ContractAlert>();

                foreach (var contract in contractsWithAlerts)
                {
                    foreach (var alert in contract.Alerts)
                    {
                        if (alert.ShouldTrigger(currentDate))
                        {
                            dueAlerts.Add(alert);
                        }
                    }
                }

                return dueAlerts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting due alerts");
                throw;
            }
        }

        public async Task<List<ContractAggregate>> GetExpiredContractsAsync(Guid? tenantId = null)
        {
            try
            {
                var currentDate = Clock.Now;
                var activeContracts = await _contractRepository.GetListAsync(
                    new ActiveContractSpecification(currentDate));

                return activeContracts
                    .Where(c => c.IsExpired(currentDate))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expired contracts");
                throw;
            }
        }
    }
}

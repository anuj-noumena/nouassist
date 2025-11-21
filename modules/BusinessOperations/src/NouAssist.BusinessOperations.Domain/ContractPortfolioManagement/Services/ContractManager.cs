using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Specifications;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Services
{
    /// <summary>
    /// Implementation of contract management service
    /// </summary>
    public class ContractManager : DomainService, IContractManager
    {
        private readonly IContractRepository _contractRepository;
        private readonly ILogger<ContractManager> _logger;

        public ContractManager(
            IContractRepository contractRepository,
            ILogger<ContractManager> logger)
        {
            _contractRepository = contractRepository;
            _logger = logger;
        }

        public async Task<ContractAggregate> CreateContractAsync(
            string title,
            string referenceNumber,
            string description,
            DateTime effectiveDate,
            DateTime? expirationDate,
            decimal totalValue,
            string currency,
            ContractTemplateReference templateReference,
            DocumentId documentId,
            Guid? tenantId = null)
        {
            try
            {
                // Check if a contract with the same reference number already exists
                var existingContract = await _contractRepository.GetByReferenceNumberAsync(
                    referenceNumber, 
                    tenantId);
                
                if (existingContract != null)
                {
                    throw new BusinessException("ContractPortfolioManagement:ErrorMessage:DuplicateReferenceNumber")
                        .WithData("referenceNumber", referenceNumber);
                }

                var contract = ContractAggregate.Create(
                    GuidGenerator.Create(),
                    tenantId,
                    title,
                    referenceNumber,
                    description,
                    effectiveDate,
                    expirationDate,
                    totalValue,
                    currency,
                    templateReference,
                    documentId);

                await _contractRepository.InsertAsync(contract);
                return contract;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contract with reference number {ReferenceNumber}", referenceNumber);
                throw;
            }
        }

        public async Task<ContractVersion> CreateContractVersionAsync(
            Guid contractId,
            string versionLabel,
            string changeReason,
            DocumentId documentId)
        {
            try
            {
                var contract = await _contractRepository.GetAsync(contractId);
                if (contract == null)
                {
                    throw new ContractNotFoundException(contractId);
                }

                var version = contract.CreateNewVersion(
                    versionLabel,
                    changeReason,
                    documentId,
                    Clock.Now);

                await _contractRepository.UpdateAsync(contract);
                return version;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new version for contract {ContractId}", contractId);
                throw;
            }
        }

        public async Task<ContractAggregate> ChangeContractStatusAsync(
            Guid contractId,
            ContractStatus newStatus)
        {
            try
            {
                var contract = await _contractRepository.GetAsync(contractId);
                if (contract == null)
                {
                    throw new ContractNotFoundException(contractId);
                }

                contract.ChangeStatus(newStatus, Clock.Now);
                await _contractRepository.UpdateAsync(contract);
                return contract;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing status to {NewStatus} for contract {ContractId}", 
                    newStatus, contractId);
                throw;
            }
        }

        public async Task<ContractAmendment> AddContractAmendmentAsync(
            Guid contractId,
            string title,
            string description,
            DateTime effectiveDate,
            List<string> affectedSections,
            DocumentId documentId)
        {
            try
            {
                var contract = await _contractRepository.GetAsync(contractId);
                if (contract == null)
                {
                    throw new ContractNotFoundException(contractId);
                }

                var amendmentDetails = new AmendmentDetails(
                    title,
                    description,
                    effectiveDate,
                    affectedSections);

                var amendment = contract.AddAmendment(amendmentDetails, documentId);
                await _contractRepository.UpdateAsync(contract);
                return amendment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding amendment to contract {ContractId}", contractId);
                throw;
            }
        }

        public async Task<ContractAlert> AddContractAlertAsync(
            Guid contractId,
            string title,
            string description,
            ContractAlertType alertType,
            DateTime triggerDate,
            int daysInAdvance,
            bool isRecurring = false,
            string frequency = null)
        {
            try
            {
                var contract = await _contractRepository.GetAsync(contractId);
                if (contract == null)
                {
                    throw new ContractNotFoundException(contractId);
                }

                var alertCondition = new AlertCondition(
                    triggerDate,
                    daysInAdvance,
                    isRecurring,
                    frequency);

                var alert = contract.AddAlert(title, description, alertType, alertCondition);
                await _contractRepository.UpdateAsync(contract);
                return alert;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding alert to contract {ContractId}", contractId);
                throw;
            }
        }

        public async Task<List<ContractAggregate>> GetExpiringContractsAsync(
            int daysThreshold,
            Guid? tenantId = null)
        {
            try
            {
                var specification = new ExpiringContractSpecification(Clock.Now, daysThreshold);
                return await _contractRepository.GetListAsync(specification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expiring contracts with threshold {DaysThreshold}", daysThreshold);
                throw;
            }
        }

        public async Task<List<ContractAggregate>> GetContractsNeedingAttentionAsync(
            int expirationDaysThreshold,
            Guid? tenantId = null)
        {
            try
            {
                var specification = new ContractsNeedingAttentionSpecification(
                    Clock.Now,
                    expirationDaysThreshold);
                    
                return await _contractRepository.GetListAsync(specification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting contracts needing attention");
                throw;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using NouAssist.BusinessOperations.FreelanceOperations.Specifications;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.FreelanceOperations.Services
{
    /// <summary>
    /// Domain service for Upwork integration
    /// </summary>
    public class UpworkConnector : DomainService, IUpworkConnector
    {
        private readonly IUpworkCredentialsRepository _credentialsRepository;
        private readonly IUpworkEngagementRepository _engagementRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<UpworkConnector> _logger;

        public UpworkConnector(
            IUpworkCredentialsRepository credentialsRepository,
            IUpworkEngagementRepository engagementRepository,
            IGuidGenerator guidGenerator,
            ILogger<UpworkConnector> logger)
        {
            _credentialsRepository = credentialsRepository;
            _engagementRepository = engagementRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task SetCredentialsAsync(UpworkCredentials credentials)
        {
            try
            {
                await _credentialsRepository.SetCredentialsAsync(credentials, CurrentTenant.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set Upwork credentials");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<UpworkCredentials> GetCredentialsAsync()
        {
            try
            {
                return await _credentialsRepository.GetCredentialsAsync(CurrentTenant.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get Upwork credentials");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<UpworkEngagement> ImportJobAsync(
            string upworkJobId,
            string jobTitle,
            string clientName,
            string jobLink,
            string contractType)
        {
            try
            {
                var engagementId = _guidGenerator.Create();
                
                var engagement = UpworkEngagement.Create(
                    engagementId,
                    upworkJobId,
                    jobTitle,
                    clientName,
                    jobLink,
                    contractType,
                    CurrentTenant.Id
                );

                await _engagementRepository.InsertAsync(engagement);
                return engagement;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to import Upwork job {JobId}", upworkJobId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateEngagementWithContractAsync(
            Guid engagementId,
            string upworkContractId,
            DateTime startDate)
        {
            try
            {
                var engagement = await _engagementRepository.GetAsync(engagementId);
                
                engagement.SetContract(upworkContractId, startDate);

                await _engagementRepository.UpdateAsync(engagement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update Upwork engagement {EngagementId} with contract", engagementId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task LinkEngagementToSystemAsync(
            Guid engagementId,
            Guid? projectId,
            Guid? clientId)
        {
            try
            {
                var engagement = await _engagementRepository.GetAsync(engagementId);
                
                engagement.LinkToSystem(projectId, clientId);

                await _engagementRepository.UpdateAsync(engagement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to link Upwork engagement {EngagementId} to system", engagementId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<UpworkEngagement>> GetActiveEngagementsAsync()
        {
            try
            {
                var specification = new ActiveUpworkEngagementsSpecification();
                return await _engagementRepository.GetListAsync(specification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get active Upwork engagements");
                throw;
            }
        }
    }
}

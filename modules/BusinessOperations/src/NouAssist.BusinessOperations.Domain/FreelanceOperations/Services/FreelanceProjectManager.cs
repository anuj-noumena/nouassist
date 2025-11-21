using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Exceptions;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.FreelanceOperations.Services
{
    /// <summary>
    /// Domain service for managing freelance projects
    /// </summary>
    public class FreelanceProjectManager : DomainService, IFreelanceProjectManager
    {
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<FreelanceProjectManager> _logger;

        public FreelanceProjectManager(
            IFreelanceProjectRepository projectRepository,
            IClientRepository clientRepository,
            IGuidGenerator guidGenerator,
            ILogger<FreelanceProjectManager> logger)
        {
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<FreelanceProject> CreateHourlyProjectAsync(
            string name,
            Guid clientId,
            HourlyRate hourlyRate,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                // Verify client exists
                if (!await _clientRepository.AnyAsync(clientId))
                {
                    throw new ClientNotFoundException(clientId);
                }

                var projectId = _guidGenerator.Create();
                
                var project = FreelanceProject.CreateHourlyProject(
                    projectId,
                    name,
                    clientId,
                    hourlyRate,
                    scope,
                    startDate,
                    endDate,
                    CurrentTenant.Id
                );

                await _projectRepository.InsertAsync(project);
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create hourly project with name {Name}", name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<FreelanceProject> CreateFixedPriceProjectAsync(
            string name,
            Guid clientId,
            decimal fixedPrice,
            string currency,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                // Verify client exists
                if (!await _clientRepository.AnyAsync(clientId))
                {
                    throw new ClientNotFoundException(clientId);
                }

                var projectId = _guidGenerator.Create();
                
                var project = FreelanceProject.CreateFixedPriceProject(
                    projectId,
                    name,
                    clientId,
                    fixedPrice,
                    currency,
                    scope,
                    startDate,
                    endDate,
                    CurrentTenant.Id
                );

                await _projectRepository.InsertAsync(project);
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create fixed price project with name {Name}", name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<FreelanceProject> CreateMilestoneProjectAsync(
            string name,
            Guid clientId,
            ProjectScopeDescription scope,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                // Verify client exists
                if (!await _clientRepository.AnyAsync(clientId))
                {
                    throw new ClientNotFoundException(clientId);
                }

                var projectId = _guidGenerator.Create();
                
                var project = FreelanceProject.CreateMilestoneProject(
                    projectId,
                    name,
                    clientId,
                    scope,
                    startDate,
                    endDate,
                    CurrentTenant.Id
                );

                await _projectRepository.InsertAsync(project);
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create milestone project with name {Name}", name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TimeEntry> AddTimeEntryAsync(
            Guid projectId,
            DateTime date,
            TimeSpan startTime,
            int durationMinutes,
            string description,
            bool isBillable = true)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                
                var timeEntry = project.AddTimeEntry(
                    date,
                    startTime,
                    durationMinutes,
                    description,
                    isBillable
                );

                await _projectRepository.UpdateAsync(project);
                return timeEntry;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add time entry to project {ProjectId}", projectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ProjectMilestone> AddMilestoneAsync(
            Guid projectId,
            MilestoneDetails details,
            DateTime? deadline = null)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                
                var milestoneId = _guidGenerator.Create();
                var milestone = project.AddMilestone(
                    milestoneId,
                    details,
                    deadline
                );

                await _projectRepository.UpdateAsync(project);
                return milestone;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add milestone to project {ProjectId}", projectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ChangeProjectStatusAsync(
            Guid projectId,
            ProjectStatus newStatus)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                
                project.ChangeStatus(newStatus, DateTime.UtcNow);

                await _projectRepository.UpdateAsync(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to change status of project {ProjectId} to {Status}", 
                    projectId, newStatus);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task LinkProjectToProposalAsync(
            Guid projectId,
            Guid proposalId)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                
                project.LinkToProposal(proposalId);

                await _projectRepository.UpdateAsync(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to link project {ProjectId} to proposal {ProposalId}", 
                    projectId, proposalId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<decimal> CalculateTotalAmountAsync(Guid projectId)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                return project.CalculateTotalAmount();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate total amount for project {ProjectId}", projectId);
                throw;
            }
        }
    }
}

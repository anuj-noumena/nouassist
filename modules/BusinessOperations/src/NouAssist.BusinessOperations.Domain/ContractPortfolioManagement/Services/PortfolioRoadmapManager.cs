using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Services
{
    /// <summary>
    /// Implementation of portfolio roadmap management service
    /// </summary>
    public class PortfolioRoadmapManager : DomainService, IPortfolioRoadmapManager
    {
        private readonly IPortfolioRoadmapRepository _roadmapRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ILogger<PortfolioRoadmapManager> _logger;

        public PortfolioRoadmapManager(
            IPortfolioRoadmapRepository roadmapRepository,
            IPortfolioRepository portfolioRepository,
            ILogger<PortfolioRoadmapManager> logger)
        {
            _roadmapRepository = roadmapRepository;
            _portfolioRepository = portfolioRepository;
            _logger = logger;
        }

        public async Task<PortfolioRoadmapAggregate> CreateRoadmapAsync(
            string title,
            string description,
            Guid portfolioId,
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId = null)
        {
            try
            {
                // Verify portfolio exists
                var portfolio = await _portfolioRepository.GetAsync(portfolioId);
                if (portfolio == null)
                {
                    throw new PortfolioNotFoundException(portfolioId);
                }

                var roadmap = PortfolioRoadmapAggregate.Create(
                    GuidGenerator.Create(),
                    tenantId,
                    title,
                    description,
                    portfolioId,
                    startDate,
                    endDate);

                await _roadmapRepository.InsertAsync(roadmap);
                return roadmap;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating roadmap {Title} for portfolio {PortfolioId}", 
                    title, portfolioId);
                throw;
            }
        }

        public async Task<RoadmapMilestone> AddRoadmapMilestoneAsync(
            Guid roadmapId,
            string title,
            string description,
            DateTime plannedDate,
            Guid? portfolioItemId = null)
        {
            try
            {
                var roadmap = await _roadmapRepository.GetWithMilestonesAsync(roadmapId);
                if (roadmap == null)
                {
                    throw new BusinessException("ContractPortfolioManagement:ErrorMessage:RoadmapNotFound")
                        .WithData("roadmapId", roadmapId);
                }

                var milestoneDate = new MilestoneDate(plannedDate);
                var milestone = roadmap.AddMilestone(
                    title,
                    description,
                    milestoneDate,
                    portfolioItemId);

                await _roadmapRepository.UpdateAsync(roadmap);
                return milestone;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding milestone {Title} to roadmap {RoadmapId}", 
                    title, roadmapId);
                throw;
            }
        }

        public async Task<CrossItemDependency> CreateMilestoneDependencyAsync(
            Guid roadmapId,
            Guid sourceMilestoneId,
            Guid targetMilestoneId,
            DependencyType dependencyType)
        {
            try
            {
                var roadmap = await _roadmapRepository.GetWithAllDetailsAsync(roadmapId);
                if (roadmap == null)
                {
                    throw new BusinessException("ContractPortfolioManagement:ErrorMessage:RoadmapNotFound")
                        .WithData("roadmapId", roadmapId);
                }

                var dependency = roadmap.AddDependency(
                    sourceMilestoneId,
                    targetMilestoneId,
                    dependencyType);

                await _roadmapRepository.UpdateAsync(roadmap);
                return dependency;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating dependency between milestones {SourceId} and {TargetId}", 
                    sourceMilestoneId, targetMilestoneId);
                throw;
            }
        }

        public async Task UpdateMilestoneCompletionAsync(
            Guid roadmapId,
            Guid milestoneId,
            bool isCompleted,
            DateTime? completionDate = null)
        {
            try
            {
                var roadmap = await _roadmapRepository.GetWithMilestonesAsync(roadmapId);
                if (roadmap == null)
                {
                    throw new BusinessException("ContractPortfolioManagement:ErrorMessage:RoadmapNotFound")
                        .WithData("roadmapId", roadmapId);
                }

                var milestone = roadmap.Milestones.FirstOrDefault(m => m.Id == milestoneId);
                if (milestone == null)
                {
                    throw new BusinessException("ContractPortfolioManagement:ErrorMessage:MilestoneNotFound")
                        .WithData("milestoneId", milestoneId)
                        .WithData("roadmapId", roadmapId);
                }

                if (isCompleted)
                {
                    milestone.MarkAsCompleted(completionDate ?? Clock.Now);
                }
                else
                {
                    milestone.ResetCompletionStatus();
                }

                await _roadmapRepository.UpdateAsync(roadmap);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating completion status for milestone {MilestoneId}", milestoneId);
                throw;
            }
        }

        public async Task<List<RoadmapMilestone>> GetMilestonesInDateRangeAsync(
            Guid roadmapId,
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                return await _roadmapRepository.GetMilestonesInDateRangeAsync(
                    roadmapId,
                    startDate,
                    endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting milestones in date range for roadmap {RoadmapId}", roadmapId);
                throw;
            }
        }

        public async Task<List<CrossItemDependency>> GetDependenciesForMilestoneAsync(
            Guid roadmapId,
            Guid milestoneId)
        {
            try
            {
                var roadmap = await _roadmapRepository.GetWithDependenciesAsync(roadmapId);
                if (roadmap == null)
                {
                    throw new BusinessException("ContractPortfolioManagement:ErrorMessage:RoadmapNotFound")
                        .WithData("roadmapId", roadmapId);
                }

                return roadmap.GetDependenciesForMilestone(milestoneId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dependencies for milestone {MilestoneId}", milestoneId);
                throw;
            }
        }
    }
}

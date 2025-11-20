using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.ContractPortfolioManagement.PortfolioRoadmaps.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.ContractPortfolioManagement.PortfolioRoadmaps;

/// <summary>
/// Application service interface for Portfolio Roadmap management
/// </summary>
public interface IPortfolioRoadmapAppService : IApplicationService
{
    /// <summary>
    /// Gets roadmap details by ID
    /// </summary>
    Task<PortfolioRoadmapDto> GetAsync(Guid id);

    /// <summary>
    /// Gets a paged list of roadmaps
    /// </summary>
    Task<PagedResultDto<PortfolioRoadmapListDto>> GetListAsync(GetPortfolioRoadmapListDto input);

    /// <summary>
    /// Creates a new roadmap
    /// </summary>
    Task<PortfolioRoadmapDto> CreateAsync(CreatePortfolioRoadmapDto input);

    /// <summary>
    /// Updates an existing roadmap
    /// </summary>
    Task<PortfolioRoadmapDto> UpdateAsync(Guid id, UpdatePortfolioRoadmapDto input);

    /// <summary>
    /// Deletes a roadmap
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets roadmaps for a portfolio
    /// </summary>
    Task<List<PortfolioRoadmapListDto>> GetPortfolioRoadmapsAsync(Guid portfolioId);

    /// <summary>
    /// Adds a milestone to a roadmap
    /// </summary>
    Task<RoadmapMilestoneDto> AddMilestoneAsync(Guid id, CreateRoadmapMilestoneDto input);

    /// <summary>
    /// Updates a roadmap milestone
    /// </summary>
    Task<RoadmapMilestoneDto> UpdateMilestoneAsync(Guid roadmapId, Guid milestoneId, UpdateRoadmapMilestoneDto input);

    /// <summary>
    /// Updates milestone completion status
    /// </summary>
    Task<RoadmapMilestoneDto> UpdateMilestoneCompletionAsync(Guid roadmapId, Guid milestoneId, UpdateMilestoneCompletionDto input);

    /// <summary>
    /// Deletes a milestone from a roadmap
    /// </summary>
    Task DeleteMilestoneAsync(Guid roadmapId, Guid milestoneId);

    /// <summary>
    /// Gets milestones in a specific date range
    /// </summary>
    Task<List<RoadmapMilestoneDto>> GetMilestonesInDateRangeAsync(Guid roadmapId, GetMilestonesInRangeDto input);

    /// <summary>
    /// Creates a dependency between milestones
    /// </summary>
    Task<CrossItemDependencyDto> CreateDependencyAsync(Guid roadmapId, CreateCrossItemDependencyDto input);

    /// <summary>
    /// Deletes a dependency
    /// </summary>
    Task DeleteDependencyAsync(Guid roadmapId, Guid dependencyId);

    /// <summary>
    /// Gets dependencies for a milestone
    /// </summary>
    Task<List<CrossItemDependencyDto>> GetDependenciesForMilestoneAsync(Guid roadmapId, Guid milestoneId);

    /// <summary>
    /// Adds a skill requirement to a milestone
    /// </summary>
    Task AddSkillRequirementAsync(Guid roadmapId, CreateResourceSkillRequirementDto input);

    /// <summary>
    /// Removes a skill requirement from a milestone
    /// </summary>
    Task RemoveSkillRequirementAsync(Guid roadmapId, Guid milestoneId, string skillName);
}

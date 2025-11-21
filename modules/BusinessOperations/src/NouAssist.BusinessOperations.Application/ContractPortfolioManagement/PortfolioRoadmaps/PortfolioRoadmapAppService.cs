using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Localization;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.ContractPortfolioManagement.Exceptions;
using NouAssist.ContractPortfolioManagement.Interfaces.Services;
using NouAssist.ContractPortfolioManagement.Permissions;
using NouAssist.ContractPortfolioManagement.PortfolioRoadmaps.Dtos;
using NouAssist.ContractPortfolioManagement.Repositories;
using NouAssist.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;

namespace NouAssist.ContractPortfolioManagement.PortfolioRoadmaps;

[Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.Default)]
public class PortfolioRoadmapAppService : ApplicationService, IPortfolioRoadmapAppService
{
    private readonly IPortfolioRoadmapRepository _roadmapRepository;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly IPortfolioRoadmapManager _roadmapManager;
    private readonly IClock _clock;
    private readonly ILogger<PortfolioRoadmapAppService> _logger;

    public PortfolioRoadmapAppService(
        IPortfolioRoadmapRepository roadmapRepository,
        IPortfolioRepository portfolioRepository,
        IPortfolioRoadmapManager roadmapManager,
        IClock clock,
        ILogger<PortfolioRoadmapAppService> logger)
    {
        _roadmapRepository = roadmapRepository;
        _portfolioRepository = portfolioRepository;
        _roadmapManager = roadmapManager;
        _clock = clock;
        _logger = logger;
        
        LocalizationResource = typeof(ContractPortfolioManagementResource);
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.View)]
    public async Task<PortfolioRoadmapDto> GetAsync(Guid id)
    {
        try
        {
            var roadmap = await _roadmapRepository.GetWithAllDetailsAsync(id);
            
            var roadmapDto = ObjectMapper.Map<PortfolioRoadmapAggregate, PortfolioRoadmapDto>(roadmap);
            
            // Enhance milestones with dependency information
            foreach (var milestoneDto in roadmapDto.Milestones)
            {
                milestoneDto.OutgoingDependencies = roadmapDto.Dependencies
                    .Where(d => d.SourceMilestoneId == milestoneDto.Id)
                    .ToList();
                    
                milestoneDto.IncomingDependencies = roadmapDto.Dependencies
                    .Where(d => d.TargetMilestoneId == milestoneDto.Id)
                    .ToList();
                    
                // Add milestone titles to dependencies
                foreach (var dependency in milestoneDto.OutgoingDependencies)
                {
                    dependency.SourceMilestoneTitle = milestoneDto.Title;
                    dependency.TargetMilestoneTitle = roadmapDto.Milestones
                        .FirstOrDefault(m => m.Id == dependency.TargetMilestoneId)?.Title;
                }
                
                foreach (var dependency in milestoneDto.IncomingDependencies)
                {
                    dependency.TargetMilestoneTitle = milestoneDto.Title;
                    dependency.SourceMilestoneTitle = roadmapDto.Milestones
                        .FirstOrDefault(m => m.Id == dependency.SourceMilestoneId)?.Title;
                }
            }
            
            return roadmapDto;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.View)]
    public async Task<PagedResultDto<PortfolioRoadmapListDto>> GetListAsync(GetPortfolioRoadmapListDto input)
    {
        try
        {
            var queryable = await _roadmapRepository.GetQueryableAsync();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(r => 
                    r.Title.Contains(input.Filter) || 
                    (r.Description != null && r.Description.Contains(input.Filter)));
            }

            if (input.PortfolioId.HasValue)
            {
                queryable = queryable.Where(r => r.PortfolioId == input.PortfolioId.Value);
            }

            if (input.StartDateMin.HasValue)
            {
                queryable = queryable.Where(r => r.StartDate >= input.StartDateMin.Value);
            }

            if (input.EndDateMax.HasValue)
            {
                queryable = queryable.Where(r => r.EndDate <= input.EndDateMax.Value);
            }

            // Get total count
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            // Apply sorting and paging
            queryable = queryable
                .OrderBy(input.Sorting ?? "Title")
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            // Get entities
            var roadmaps = await AsyncExecuter.ToListAsync(queryable);

            // Map to DTOs
            var roadmapDtos = ObjectMapper.Map<List<PortfolioRoadmapAggregate>, List<PortfolioRoadmapListDto>>(roadmaps);

            // Add portfolio names to DTOs
            var portfolioIds = roadmapDtos.Select(r => r.PortfolioId).Distinct().ToList();
            var portfolios = await _portfolioRepository.GetListAsync(portfolioIds);
            
            foreach (var roadmapDto in roadmapDtos)
            {
                var portfolio = portfolios.FirstOrDefault(p => p.Id == roadmapDto.PortfolioId);
                if (portfolio != null)
                {
                    roadmapDto.PortfolioName = portfolio.Name;
                }
            }

            return new PagedResultDto<PortfolioRoadmapListDto>(totalCount, roadmapDtos);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.Create)]
    public async Task<PortfolioRoadmapDto> CreateAsync(CreatePortfolioRoadmapDto input)
    {
        try
        {
            // Verify portfolio exists
            var portfolio = await _portfolioRepository.GetAsync(input.PortfolioId);
            
            var roadmap = await _roadmapManager.CreateRoadmapAsync(
                input.Title,
                input.Description,
                input.PortfolioId,
                input.StartDate,
                input.EndDate,
                CurrentTenant.Id);

            return ObjectMapper.Map<PortfolioRoadmapAggregate, PortfolioRoadmapDto>(roadmap);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", input.PortfolioId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.Update)]
    public async Task<PortfolioRoadmapDto> UpdateAsync(Guid id, UpdatePortfolioRoadmapDto input)
    {
        try
        {
            // Verify portfolio exists
            var portfolio = await _portfolioRepository.GetAsync(input.PortfolioId);
            
            var roadmap = await _roadmapRepository.GetAsync(id);

            roadmap.UpdateBasicDetails(
                input.Title,
                input.Description,
                input.StartDate,
                input.EndDate);

            await _roadmapRepository.UpdateAsync(roadmap);

            return ObjectMapper.Map<PortfolioRoadmapAggregate, PortfolioRoadmapDto>(
                await _roadmapRepository.GetWithAllDetailsAsync(id));
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
            {
                if (ex.Message.Contains(input.PortfolioId.ToString()))
                    throw new UserFriendlyException(L["Portfolio:ErrorNotFound", input.PortfolioId]);
                else
                    throw new UserFriendlyException(L["Roadmap:ErrorNotFound", id]);
            }
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _roadmapRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.View)]
    public async Task<List<PortfolioRoadmapListDto>> GetPortfolioRoadmapsAsync(Guid portfolioId)
    {
        try
        {
            var roadmaps = await _roadmapRepository.GetByPortfolioIdAsync(portfolioId);
            var roadmapDtos = ObjectMapper.Map<List<PortfolioRoadmapAggregate>, List<PortfolioRoadmapListDto>>(roadmaps);
            
            // Get portfolio name
            var portfolio = await _portfolioRepository.GetAsync(portfolioId);
            var portfolioName = portfolio.Name;
            
            foreach (var roadmapDto in roadmapDtos)
            {
                roadmapDto.PortfolioName = portfolioName;
            }
            
            return roadmapDtos;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", portfolioId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.AddMilestone)]
    public async Task<RoadmapMilestoneDto> AddMilestoneAsync(Guid id, CreateRoadmapMilestoneDto input)
    {
        try
        {
            var milestone = await _roadmapManager.AddRoadmapMilestoneAsync(
                id,
                input.Title,
                input.Description,
                input.PlannedDate,
                input.PortfolioItemId);

            return ObjectMapper.Map<RoadmapMilestone, RoadmapMilestoneDto>(milestone);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.Update)]
    public async Task<RoadmapMilestoneDto> UpdateMilestoneAsync(Guid roadmapId, Guid milestoneId, UpdateRoadmapMilestoneDto input)
    {
        try
        {
            var roadmap = await _roadmapRepository.GetWithMilestonesAsync(roadmapId);
            
            var milestone = roadmap.Milestones.FirstOrDefault(m => m.Id == milestoneId);
            if (milestone == null)
            {
                throw new BusinessException("ContractPortfolioManagement:ErrorMessage:MilestoneNotFound")
                    .WithData("milestoneId", milestoneId)
                    .WithData("roadmapId", roadmapId);
            }

            var milestoneDate = new MilestoneDate(input.PlannedDate, milestone.Date.ActualDate);
            
            milestone.UpdateDetails(
                input.Title,
                input.Description,
                milestoneDate);
                
            await _roadmapRepository.UpdateAsync(roadmap);

            return ObjectMapper.Map<RoadmapMilestone, RoadmapMilestoneDto>(milestone);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", roadmapId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.Update)]
    public async Task<RoadmapMilestoneDto> UpdateMilestoneCompletionAsync(Guid roadmapId, Guid milestoneId, UpdateMilestoneCompletionDto input)
    {
        try
        {
            await _roadmapManager.UpdateMilestoneCompletionAsync(
                roadmapId,
                milestoneId,
                input.IsCompleted,
                input.CompletionDate ?? _clock.Now);

            var roadmap = await _roadmapRepository.GetWithMilestonesAsync(roadmapId);
            var milestone = roadmap.Milestones.First(m => m.Id == milestoneId);
            
            return ObjectMapper.Map<RoadmapMilestone, RoadmapMilestoneDto>(milestone);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", roadmapId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.Update)]
    public async Task DeleteMilestoneAsync(Guid roadmapId, Guid milestoneId)
    {
        try
        {
            var roadmap = await _roadmapRepository.GetWithMilestonesAsync(roadmapId);
            
            roadmap.RemoveMilestone(milestoneId);

            await _roadmapRepository.UpdateAsync(roadmap);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", roadmapId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.View)]
    public async Task<List<RoadmapMilestoneDto>> GetMilestonesInDateRangeAsync(Guid roadmapId, GetMilestonesInRangeDto input)
    {
        try
        {
            var milestones = await _roadmapManager.GetMilestonesInDateRangeAsync(
                roadmapId,
                input.StartDate,
                input.EndDate);

            return ObjectMapper.Map<List<RoadmapMilestone>, List<RoadmapMilestoneDto>>(milestones);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", roadmapId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.ManageDependencies)]
    public async Task<CrossItemDependencyDto> CreateDependencyAsync(Guid roadmapId, CreateCrossItemDependencyDto input)
    {
        try
        {
            var dependency = await _roadmapManager.CreateMilestoneDependencyAsync(
                roadmapId,
                input.SourceMilestoneId,
                input.TargetMilestoneId,
                input.Type);

            var dependencyDto = ObjectMapper.Map<CrossItemDependency, CrossItemDependencyDto>(dependency);
            
            // Add milestone titles for better context
            var roadmap = await _roadmapRepository.GetWithMilestonesAsync(roadmapId);
            var sourceMilestone = roadmap.Milestones.FirstOrDefault(m => m.Id == input.SourceMilestoneId);
            var targetMilestone = roadmap.Milestones.FirstOrDefault(m => m.Id == input.TargetMilestoneId);
            
            if (sourceMilestone != null)
                dependencyDto.SourceMilestoneTitle = sourceMilestone.Title;
                
            if (targetMilestone != null)
                dependencyDto.TargetMilestoneTitle = targetMilestone.Title;

            return dependencyDto;
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", roadmapId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.ManageDependencies)]
    public async Task DeleteDependencyAsync(Guid roadmapId, Guid dependencyId)
    {
        try
        {
            var roadmap = await _roadmapRepository.GetWithDependenciesAsync(roadmapId);
            
            var dependency = roadmap.Dependencies.FirstOrDefault(d => d.Id == dependencyId);
            if (dependency == null)
            {
                throw new BusinessException("ContractPortfolioManagement:ErrorMessage:DependencyNotFound")
                    .WithData("dependencyId", dependencyId)
                    .WithData("roadmapId", roadmapId);
            }
            
            roadmap.RemoveDependency(dependencyId);

            await _roadmapRepository.UpdateAsync(roadmap);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", roadmapId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.View)]
    public async Task<List<CrossItemDependencyDto>> GetDependenciesForMilestoneAsync(Guid roadmapId, Guid milestoneId)
    {
        try
        {
            var dependencies = await _roadmapManager.GetDependenciesForMilestoneAsync(roadmapId, milestoneId);
            
            var dependencyDtos = ObjectMapper.Map<List<CrossItemDependency>, List<CrossItemDependencyDto>>(dependencies);
            
            // Add milestone titles for better context
            var roadmap = await _roadmapRepository.GetWithMilestonesAsync(roadmapId);
            
            foreach (var dependencyDto in dependencyDtos)
            {
                var sourceMilestone = roadmap.Milestones.FirstOrDefault(m => m.Id == dependencyDto.SourceMilestoneId);
                var targetMilestone = roadmap.Milestones.FirstOrDefault(m => m.Id == dependencyDto.TargetMilestoneId);
                
                if (sourceMilestone != null)
                    dependencyDto.SourceMilestoneTitle = sourceMilestone.Title;
                    
                if (targetMilestone != null)
                    dependencyDto.TargetMilestoneTitle = targetMilestone.Title;
            }
            
            return dependencyDtos;
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", roadmapId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.Update)]
    public async Task AddSkillRequirementAsync(Guid roadmapId, CreateResourceSkillRequirementDto input)
    {
        try
        {
            var roadmap = await _roadmapRepository.GetWithMilestonesAsync(roadmapId);
            
            var milestone = roadmap.Milestones.FirstOrDefault(m => m.Id == input.MilestoneId);
            if (milestone == null)
            {
                throw new BusinessException("ContractPortfolioManagement:ErrorMessage:MilestoneNotFound")
                    .WithData("milestoneId", input.MilestoneId)
                    .WithData("roadmapId", roadmapId);
            }

            milestone.AddSkillRequirement(
                input.SkillName,
                input.RequiredLevel,
                input.IsMandatory,
                input.Description);
                
            await _roadmapRepository.UpdateAsync(roadmap);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", roadmapId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.PortfolioRoadmap.Update)]
    public async Task RemoveSkillRequirementAsync(Guid roadmapId, Guid milestoneId, string skillName)
    {
        try
        {
            var roadmap = await _roadmapRepository.GetWithMilestonesAsync(roadmapId);
            
            var milestone = roadmap.Milestones.FirstOrDefault(m => m.Id == milestoneId);
            if (milestone == null)
            {
                throw new BusinessException("ContractPortfolioManagement:ErrorMessage:MilestoneNotFound")
                    .WithData("milestoneId", milestoneId)
                    .WithData("roadmapId", roadmapId);
            }

            milestone.RemoveSkillRequirement(skillName);
                
            await _roadmapRepository.UpdateAsync(roadmap);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Roadmap:ErrorNotFound", roadmapId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    protected async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
    {
        _logger.Log(logLevel, exception, 
            "Error in Portfolio Roadmap Application Service: {ErrorMessage}", exception.Message);
        
        await Task.CompletedTask;
    }
}

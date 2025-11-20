using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.ContractPortfolioManagement.Enums;
using NouAssist.ContractPortfolioManagement.Exceptions;
using NouAssist.ContractPortfolioManagement.Interfaces.Services;
using NouAssist.ContractPortfolioManagement.Permissions;
using NouAssist.ContractPortfolioManagement.Portfolios.Dtos;
using NouAssist.ContractPortfolioManagement.Repositories;
using NouAssist.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;

namespace NouAssist.ContractPortfolioManagement.Portfolios;

[Authorize(ContractPortfolioManagementPermissions.Portfolio.Default)]
public class PortfolioAppService : ApplicationService, IPortfolioAppService
{
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly IPortfolioManager _portfolioManager;
    private readonly IClock _clock;
    private readonly ILogger<PortfolioAppService> _logger;

    public PortfolioAppService(
        IPortfolioRepository portfolioRepository,
        IPortfolioManager portfolioManager,
        IClock clock,
        ILogger<PortfolioAppService> logger)
    {
        _portfolioRepository = portfolioRepository;
        _portfolioManager = portfolioManager;
        _clock = clock;
        _logger = logger;
        
        LocalizationResource = typeof(ContractPortfolioManagementResource);
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.View)]
    public async Task<PortfolioDto> GetAsync(Guid id)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetWithAllDetailsAsync(id);
            return ObjectMapper.Map<PortfolioAggregate, PortfolioDto>(portfolio);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.View)]
    public async Task<PagedResultDto<PortfolioListDto>> GetListAsync(GetPortfolioListDto input)
    {
        try
        {
            var queryable = await _portfolioRepository.GetQueryableAsync();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(p => 
                    p.Name.Contains(input.Filter) || 
                    (p.Description != null && p.Description.Contains(input.Filter)));
            }

            if (input.IsActive.HasValue)
            {
                queryable = queryable.Where(p => p.IsActive == input.IsActive.Value);
            }

            // Get total count
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            // Apply sorting and paging
            queryable = queryable
                .OrderBy(input.Sorting ?? "Name")
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            // Get entities
            var portfolios = await AsyncExecuter.ToListAsync(queryable);

            // Map to DTOs
            var portfolioDtos = ObjectMapper.Map<List<PortfolioAggregate>, List<PortfolioListDto>>(portfolios);

            return new PagedResultDto<PortfolioListDto>(totalCount, portfolioDtos);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Create)]
    public async Task<PortfolioDto> CreateAsync(CreatePortfolioDto input)
    {
        try
        {
            var portfolio = await _portfolioManager.CreatePortfolioAsync(
                input.Name,
                input.Description,
                CurrentTenant.Id);

            return ObjectMapper.Map<PortfolioAggregate, PortfolioDto>(portfolio);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Update)]
    public async Task<PortfolioDto> UpdateAsync(Guid id, UpdatePortfolioDto input)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            portfolio.UpdateBasicDetails(input.Name, input.Description);
            portfolio.SetActiveStatus(input.IsActive);

            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<PortfolioAggregate, PortfolioDto>(
                await _portfolioRepository.GetWithAllDetailsAsync(id));
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
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _portfolioRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Update)]
    public async Task AddGoalAsync(Guid id, CreatePortfolioGoalDto input)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            portfolio.AddGoal(input.Title, input.Description, input.TargetDate);

            await _portfolioRepository.UpdateAsync(portfolio);
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
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Update)]
    public async Task AddMetricAsync(Guid id, CreatePerformanceMetricDto input)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            portfolio.AddMetric(input.Name, input.Description, input.Target, input.Actual, input.Unit);

            await _portfolioRepository.UpdateAsync(portfolio);
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
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Update)]
    public async Task UpdateMetricAsync(Guid id, UpdatePerformanceMetricDto input)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            portfolio.UpdateMetric(input.Name, input.Actual, _clock.Now);

            await _portfolioRepository.UpdateAsync(portfolio);
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
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.AddItem)]
    public async Task<PortfolioItemDto> AddItemAsync(Guid id, AddPortfolioItemDto input)
    {
        try
        {
            var portfolioItem = await _portfolioManager.AddPortfolioItemAsync(
                id,
                input.ItemId,
                input.ItemType,
                input.ItemName,
                input.CategoryId);

            // Set notes if provided
            if (!string.IsNullOrEmpty(input.Notes))
            {
                portfolioItem.UpdateNotes(input.Notes);
            }

            await _portfolioRepository.UpdateAsync(
                await _portfolioRepository.GetAsync(id));

            var result = ObjectMapper.Map<PortfolioItem, PortfolioItemDto>(portfolioItem);

            // Get category name if available
            if (input.CategoryId.HasValue)
            {
                var portfolio = await _portfolioRepository.GetWithAllDetailsAsync(id);
                var category = portfolio.GetCategory(input.CategoryId.Value);
                if (category != null)
                {
                    result.CategoryName = category.Name;
                }
            }

            return result;
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            
            if (bex is DuplicatePortfolioItemException)
                throw new UserFriendlyException(L["Portfolio:ErrorDuplicateItem", 
                    bex.Data["itemId"], bex.Data["itemType"], bex.Data["portfolioId"]]);
                
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Update)]
    public async Task<PortfolioItemDto> UpdateItemAsync(Guid portfolioId, Guid itemId, UpdatePortfolioItemDto input)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetWithItemsAsync(portfolioId);
            
            var portfolioItem = portfolio.GetItem(itemId);
            if (portfolioItem == null)
            {
                throw new BusinessException("ContractPortfolioManagement:ErrorMessage:PortfolioItemNotFound")
                    .WithData("itemId", itemId)
                    .WithData("portfolioId", portfolioId);
            }

            if (input.CategoryId.HasValue)
            {
                portfolioItem.UpdateCategory(input.CategoryId);
            }

            if (input.Progress.HasValue)
            {
                portfolioItem.UpdateProgress(input.Progress.Value);
            }

            if (input.Notes != null)
            {
                portfolioItem.UpdateNotes(input.Notes);
            }

            await _portfolioRepository.UpdateAsync(portfolio);

            var result = ObjectMapper.Map<PortfolioItem, PortfolioItemDto>(portfolioItem);

            // Get category name if available
            if (input.CategoryId.HasValue)
            {
                var category = portfolio.GetCategory(input.CategoryId.Value);
                if (category != null)
                {
                    result.CategoryName = category.Name;
                }
            }

            return result;
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
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", portfolioId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.RemoveItem)]
    public async Task RemoveItemAsync(Guid portfolioId, Guid itemId)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetWithItemsAsync(portfolioId);
            
            portfolio.RemoveItem(itemId);

            await _portfolioRepository.UpdateAsync(portfolio);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", portfolioId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Update)]
    public async Task<PortfolioCategoryDto> CreateCategoryAsync(Guid id, CreatePortfolioCategoryDto input)
    {
        try
        {
            var category = await _portfolioManager.CreatePortfolioCategoryAsync(
                id,
                input.Name,
                input.Description,
                input.ColorCode);

            var result = ObjectMapper.Map<PortfolioCategory, PortfolioCategoryDto>(category);

            // Calculate item count
            var portfolio = await _portfolioRepository.GetWithItemsAsync(id);
            result.ItemCount = portfolio.Items.Count(i => i.CategoryId == category.Id);

            return result;
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
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Update)]
    public async Task DeleteCategoryAsync(Guid portfolioId, Guid categoryId)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetWithAllDetailsAsync(portfolioId);
            
            portfolio.RemoveCategory(categoryId);

            await _portfolioRepository.UpdateAsync(portfolio);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", portfolioId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Update)]
    public async Task<PortfolioRiskDto> AddRiskAsync(Guid id, CreatePortfolioRiskDto input)
    {
        try
        {
            var risk = await _portfolioManager.AddPortfolioRiskAsync(
                id,
                input.Title,
                input.Level,
                input.Description,
                input.MitigationPlan,
                input.Probability,
                input.Impact);

            return ObjectMapper.Map<PortfolioRisk, PortfolioRiskDto>(risk);
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
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Update)]
    public async Task RemoveRiskAsync(Guid portfolioId, Guid riskId)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetWithRisksAsync(portfolioId);
            
            portfolio.RemoveRisk(riskId);

            await _portfolioRepository.UpdateAsync(portfolio);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", portfolioId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.View)]
    public async Task<List<PortfolioListDto>> GetPortfoliosContainingItemAsync(Guid itemId, int itemType)
    {
        try
        {
            // Validate the item type is valid
            if (!Enum.IsDefined(typeof(PortfolioItemType), itemType))
            {
                throw new BusinessException("ContractPortfolioManagement:ErrorMessage:InvalidValue")
                    .WithData("propertyName", "ItemType");
            }

            var portfolioItemType = (PortfolioItemType)itemType;
            
            var portfolios = await _portfolioManager.GetPortfoliosContainingItemAsync(
                itemId,
                portfolioItemType,
                CurrentTenant.Id);

            return ObjectMapper.Map<List<PortfolioAggregate>, List<PortfolioListDto>>(portfolios);
        }
        catch (BusinessException bex)
        {
            await HandleErrorAsync(bex, LogLevel.Warning);
            throw new UserFriendlyException(L[bex.Code ?? "Error:Generic", bex.Message]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Share)]
    public async Task<SharingLinkDto> CreateSharingLinkAsync(Guid id, CreateSharingLinkDto input)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            var permissions = new AccessPermissions(
                input.CanView, 
                input.CanEdit, 
                input.CanShare, 
                input.CanDelete);
                
            var sharingLink = portfolio.CreateSharingLink(input.ExpirationDate, permissions);
            
            await _portfolioRepository.UpdateAsync(portfolio);

            return ObjectMapper.Map<SharingLink, SharingLinkDto>(sharingLink);
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
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.Share)]
    public async Task RemoveSharingLinkAsync(Guid portfolioId, Guid linkId)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetAsync(portfolioId);
            
            portfolio.RemoveSharingLink(linkId);

            await _portfolioRepository.UpdateAsync(portfolio);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", portfolioId]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.View)]
    public async Task<PortfolioPerformanceReportDto> GetPerformanceReportAsync(Guid id)
    {
        try
        {
            var portfolio = await _portfolioRepository.GetWithAllDetailsAsync(id);
            
            var reportDto = ObjectMapper.Map<PortfolioAggregate, PortfolioPerformanceReportDto>(portfolio);
            
            // Calculate overall progress by averaging item progress values
            if (portfolio.Items.Any(i => i.Progress.HasValue))
            {
                var itemsWithProgress = portfolio.Items.Where(i => i.Progress.HasValue).ToList();
                if (itemsWithProgress.Any())
                {
                    reportDto.OverallProgress = itemsWithProgress.Average(i => i.Progress.Value);
                }
            }
            
            return reportDto;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            
            if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                throw new UserFriendlyException(L["Portfolio:ErrorNotFound", id]);
                
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    [Authorize(ContractPortfolioManagementPermissions.Portfolio.View)]
    public async Task<List<PortfolioListDto>> GetHighRiskPortfoliosAsync(decimal riskScoreThreshold = 6.0m)
    {
        try
        {
            var portfolios = await _portfolioManager.GetHighRiskPortfoliosAsync(
                riskScoreThreshold,
                CurrentTenant.Id);

            return ObjectMapper.Map<List<PortfolioAggregate>, List<PortfolioListDto>>(portfolios);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
            throw new UserFriendlyException(L["Error:Generic"]);
        }
    }

    protected async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
    {
        _logger.Log(logLevel, exception, 
            "Error in Portfolio Application Service: {ErrorMessage}", exception.Message);
        
        await Task.CompletedTask;
    }
}

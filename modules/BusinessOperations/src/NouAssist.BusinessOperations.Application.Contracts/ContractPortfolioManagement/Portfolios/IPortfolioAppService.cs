using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.ContractPortfolioManagement.Portfolios.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.ContractPortfolioManagement.Portfolios;

/// <summary>
/// Application service interface for Portfolio management
/// </summary>
public interface IPortfolioAppService : IApplicationService
{
    /// <summary>
    /// Gets portfolio details by ID
    /// </summary>
    Task<PortfolioDto> GetAsync(Guid id);

    /// <summary>
    /// Gets a paged list of portfolios
    /// </summary>
    Task<PagedResultDto<PortfolioListDto>> GetListAsync(GetPortfolioListDto input);

    /// <summary>
    /// Creates a new portfolio
    /// </summary>
    Task<PortfolioDto> CreateAsync(CreatePortfolioDto input);

    /// <summary>
    /// Updates an existing portfolio
    /// </summary>
    Task<PortfolioDto> UpdateAsync(Guid id, UpdatePortfolioDto input);

    /// <summary>
    /// Deletes a portfolio
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Adds a goal to a portfolio
    /// </summary>
    Task AddGoalAsync(Guid id, CreatePortfolioGoalDto input);

    /// <summary>
    /// Adds a performance metric to a portfolio
    /// </summary>
    Task AddMetricAsync(Guid id, CreatePerformanceMetricDto input);

    /// <summary>
    /// Updates a performance metric
    /// </summary>
    Task UpdateMetricAsync(Guid id, UpdatePerformanceMetricDto input);

    /// <summary>
    /// Adds an item to a portfolio
    /// </summary>
    Task<PortfolioItemDto> AddItemAsync(Guid id, AddPortfolioItemDto input);

    /// <summary>
    /// Updates a portfolio item
    /// </summary>
    Task<PortfolioItemDto> UpdateItemAsync(Guid portfolioId, Guid itemId, UpdatePortfolioItemDto input);

    /// <summary>
    /// Removes an item from a portfolio
    /// </summary>
    Task RemoveItemAsync(Guid portfolioId, Guid itemId);

    /// <summary>
    /// Creates a category in a portfolio
    /// </summary>
    Task<PortfolioCategoryDto> CreateCategoryAsync(Guid id, CreatePortfolioCategoryDto input);

    /// <summary>
    /// Deletes a category from a portfolio
    /// </summary>
    Task DeleteCategoryAsync(Guid portfolioId, Guid categoryId);

    /// <summary>
    /// Adds a risk to a portfolio
    /// </summary>
    Task<PortfolioRiskDto> AddRiskAsync(Guid id, CreatePortfolioRiskDto input);

    /// <summary>
    /// Removes a risk from a portfolio
    /// </summary>
    Task RemoveRiskAsync(Guid portfolioId, Guid riskId);

    /// <summary>
    /// Gets portfolios containing a specific item
    /// </summary>
    Task<List<PortfolioListDto>> GetPortfoliosContainingItemAsync(Guid itemId, int itemType);

    /// <summary>
    /// Creates a sharing link for a portfolio
    /// </summary>
    Task<SharingLinkDto> CreateSharingLinkAsync(Guid id, CreateSharingLinkDto input);

    /// <summary>
    /// Removes a sharing link
    /// </summary>
    Task RemoveSharingLinkAsync(Guid portfolioId, Guid linkId);

    /// <summary>
    /// Gets performance report for a portfolio
    /// </summary>
    Task<PortfolioPerformanceReportDto> GetPerformanceReportAsync(Guid id);

    /// <summary>
    /// Gets portfolios with high risk
    /// </summary>
    Task<List<PortfolioListDto>> GetHighRiskPortfoliosAsync(decimal riskScoreThreshold = 6.0m);
}

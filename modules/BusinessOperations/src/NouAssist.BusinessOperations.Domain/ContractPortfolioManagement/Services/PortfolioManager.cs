using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio;
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
    /// Implementation of portfolio management service
    /// </summary>
    public class PortfolioManager : DomainService, IPortfolioManager
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ILogger<PortfolioManager> _logger;

        public PortfolioManager(
            IPortfolioRepository portfolioRepository,
            ILogger<PortfolioManager> logger)
        {
            _portfolioRepository = portfolioRepository;
            _logger = logger;
        }

        public async Task<PortfolioAggregate> CreatePortfolioAsync(
            string name,
            string description,
            Guid? tenantId = null)
        {
            try
            {
                // Check if portfolio with same name exists
                var existingPortfolio = await _portfolioRepository.GetByNameAsync(name, tenantId);
                if (existingPortfolio != null)
                {
                    throw new BusinessException("ContractPortfolioManagement:ErrorMessage:DuplicateName")
                        .WithData("entityType", "Portfolio")
                        .WithData("name", name);
                }

                var portfolio = PortfolioAggregate.Create(
                    GuidGenerator.Create(),
                    tenantId,
                    name,
                    description);

                await _portfolioRepository.InsertAsync(portfolio);
                return portfolio;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating portfolio with name {Name}", name);
                throw;
            }
        }

        public async Task<PortfolioItem> AddPortfolioItemAsync(
            Guid portfolioId,
            Guid itemId,
            PortfolioItemType itemType,
            string itemName,
            Guid? categoryId = null)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetWithItemsAsync(portfolioId);
                if (portfolio == null)
                {
                    throw new PortfolioNotFoundException(portfolioId);
                }

                var itemReference = new ItemReference(itemId, itemType, itemName);
                var portfolioItem = portfolio.AddItem(itemReference, categoryId);
                
                await _portfolioRepository.UpdateAsync(portfolio);
                return portfolioItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item {ItemId} of type {ItemType} to portfolio {PortfolioId}", 
                    itemId, itemType, portfolioId);
                throw;
            }
        }

        public async Task<PortfolioCategory> CreatePortfolioCategoryAsync(
            Guid portfolioId,
            string name,
            string description,
            string colorCode = null)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetAsync(portfolioId);
                if (portfolio == null)
                {
                    throw new PortfolioNotFoundException(portfolioId);
                }

                var category = portfolio.AddCategory(name, description, colorCode);
                await _portfolioRepository.UpdateAsync(portfolio);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category {Name} in portfolio {PortfolioId}", 
                    name, portfolioId);
                throw;
            }
        }

        public async Task<PortfolioRisk> AddPortfolioRiskAsync(
            Guid portfolioId,
            string title,
            RiskLevel level,
            string description,
            string mitigationPlan,
            decimal probability,
            decimal impact)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetWithRisksAsync(portfolioId);
                if (portfolio == null)
                {
                    throw new PortfolioNotFoundException(portfolioId);
                }

                var assessmentData = new RiskAssessmentData(
                    level,
                    description,
                    mitigationPlan,
                    probability,
                    impact,
                    Clock.Now);

                var risk = portfolio.AddRisk(title, assessmentData);
                await _portfolioRepository.UpdateAsync(portfolio);
                return risk;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding risk {Title} to portfolio {PortfolioId}", 
                    title, portfolioId);
                throw;
            }
        }

        public async Task<SharingLink> CreatePortfolioSharingLinkAsync(
            Guid portfolioId,
            DateTime expirationDate,
            bool canView,
            bool canEdit,
            bool canShare,
            bool canDelete)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetAsync(portfolioId);
                if (portfolio == null)
                {
                    throw new PortfolioNotFoundException(portfolioId);
                }

                var permissions = new AccessPermissions(canView, canEdit, canShare, canDelete);
                var link = portfolio.CreateSharingLink(expirationDate, permissions);
                
                await _portfolioRepository.UpdateAsync(portfolio);
                return link;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sharing link for portfolio {PortfolioId}", portfolioId);
                throw;
            }
        }

        public async Task<List<PortfolioAggregate>> GetHighRiskPortfoliosAsync(
            decimal riskScoreThreshold = 6.0m,
            Guid? tenantId = null)
        {
            try
            {
                var specification = new HighRiskPortfolioSpecification(riskScoreThreshold);
                return await _portfolioRepository.GetListAsync(specification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting high risk portfolios");
                throw;
            }
        }

        public async Task<List<PortfolioAggregate>> GetPortfoliosContainingItemAsync(
            Guid itemId,
            PortfolioItemType itemType,
            Guid? tenantId = null)
        {
            try
            {
                var specification = new PortfolioContainingItemSpecification(itemId, itemType);
                return await _portfolioRepository.GetListAsync(specification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolios containing item {ItemId} of type {ItemType}", 
                    itemId, itemType);
                throw;
            }
        }
    }
}

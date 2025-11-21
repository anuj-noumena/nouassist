using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Localization;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.NetworkIntelligence.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Permissions;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.NetworkIntelligence
{
    /// <summary>
    /// Application service for network intelligence and mapping
    /// </summary>
    [Authorize(IRMPermissions.NetworkIntelligence.Default)]
    public class NetworkIntelligenceAppService : ApplicationService, INetworkIntelligenceAppService
    {
        private readonly INetworkMappingService _networkMappingService;
        private readonly IInvestorProfileRepository _investorRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;

        public NetworkIntelligenceAppService(
            INetworkMappingService networkMappingService,
            IInvestorProfileRepository investorRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant)
        {
            _networkMappingService = networkMappingService;
            _investorRepository = investorRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;

            LocalizationResource = typeof(IRMResource);
        }

        [Authorize(IRMPermissions.NetworkIntelligence.View)]
        public async Task<NetworkMapDto> GenerateNetworkMapAsync(Guid investorId)
        {
            try
            {
                // Verify investor exists and belongs to current tenant
                var investor = await _investorRepository.GetAsync(investorId);
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var networkMap = await _networkMappingService.GenerateInvestorNetworkMapAsync(investorId);
                return ObjectMapper.Map<NetworkMap, NetworkMapDto>(networkMap);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.NetworkIntelligence.View)]
        public async Task<List<InvestorConnectionDto>> FindPotentialConnectionsAsync(Guid investorId)
        {
            try
            {
                // Verify investor exists and belongs to current tenant
                var investor = await _investorRepository.GetAsync(investorId);
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var connections = await _networkMappingService.FindPotentialConnectionsAsync(investorId);
                return ObjectMapper.Map<List<InvestorConnection>, List<InvestorConnectionDto>>(connections);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.NetworkIntelligence.GenerateReports)]
        public async Task<NetworkInsightsDto> AnalyzeNetworkInsightsAsync(List<Guid> investorIds)
        {
            try
            {
                // Verify all investors exist and belong to current tenant
                var investors = await _investorRepository.GetByIdsAsync(investorIds);
                if (investors.Any(i => i.TenantId != _currentTenant.Id))
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var insights = await _networkMappingService.AnalyzeNetworkInsightsAsync(investorIds);
                return ObjectMapper.Map<NetworkInsights, NetworkInsightsDto>(insights);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.NetworkIntelligence.ViewInfluenceScores)]
        public async Task<List<InvestorInfluenceDto>> CalculateInfluenceScoresAsync()
        {
            try
            {
                var influences = await _networkMappingService.CalculateInvestorInfluenceScoresAsync();
                return ObjectMapper.Map<List<InvestorInfluence>, List<InvestorInfluenceDto>>(influences);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.NetworkIntelligence.GenerateReports)]
        public async Task<List<string>> GetNetworkRecommendationsAsync(List<Guid> investorIds)
        {
            try
            {
                // Verify all investors exist and belong to current tenant
                var investors = await _investorRepository.GetByIdsAsync(investorIds);
                if (investors.Any(i => i.TenantId != _currentTenant.Id))
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var insights = await _networkMappingService.AnalyzeNetworkInsightsAsync(investorIds);
                return insights.Recommendations;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in NetworkIntelligenceAppService: {ErrorMessage}", exception.Message);
        }
    }
}

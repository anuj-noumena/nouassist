using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Localization;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Permissions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances
{
    /// <summary>
    /// Application service implementation for SecurityIssuance management
    /// </summary>
    [Authorize(CapTablePermissions.SecurityIssuances.Default)]
    public class SecurityIssuanceAppService : ApplicationService, ISecurityIssuanceAppService
    {
        // --- Dependencies ---
        private readonly ISecurityIssuanceRepository _securityIssuanceRepository;
        private readonly ICapTableRepository _capTableRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public SecurityIssuanceAppService(
            ISecurityIssuanceRepository securityIssuanceRepository,
            ICapTableRepository capTableRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _securityIssuanceRepository = securityIssuanceRepository;
            _capTableRepository = capTableRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(CapTableResource);
        }

        [Authorize(CapTablePermissions.SecurityIssuances.Create)]
        public virtual async Task<SecurityIssuanceDto> CreateStockIssuanceAsync(CreateStockIssuanceDto input)
        {
            try
            {
                var newId = _guidGenerator.Create();
                var currentTenantId = _currentTenant.Id;
                var issuanceDate = input.IssuanceDate ?? _clock.Now;

                var securityIssuance = SecurityIssuance.CreateStockIssuance(
                    newId,
                    input.CapTableId,
                    input.Name,
                    new ShareQuantity(input.TotalAuthorized),
                    new PricePerShare(input.IssuePrice, input.Currency),
                    issuanceDate,
                    input.Description,
                    currentTenantId
                );

                await _securityIssuanceRepository.InsertAsync(securityIssuance, autoSave: true);

                return ObjectMapper.Map<SecurityIssuance, SecurityIssuanceDto>(securityIssuance);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.Create)]
        public virtual async Task<SecurityIssuanceDto> CreateOptionIssuanceAsync(CreateOptionIssuanceDto input)
        {
            try
            {
                var newId = _guidGenerator.Create();
                var currentTenantId = _currentTenant.Id;
                var issuanceDate = input.IssuanceDate ?? _clock.Now;

                var optionDetails = new OptionDetails(input.StrikePrice, input.ExpiryDate, input.Currency);

                var securityIssuance = SecurityIssuance.CreateOptionIssuance(
                    newId,
                    input.CapTableId,
                    input.Name,
                    new ShareQuantity(input.TotalAuthorized),
                    optionDetails,
                    issuanceDate,
                    input.Description,
                    currentTenantId
                );

                await _securityIssuanceRepository.InsertAsync(securityIssuance, autoSave: true);

                return ObjectMapper.Map<SecurityIssuance, SecurityIssuanceDto>(securityIssuance);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.View)]
        public virtual async Task<SecurityIssuanceDto> GetAsync(Guid id)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetWithShareClassesAsync(id);
                return ObjectMapper.Map<SecurityIssuance, SecurityIssuanceDto>(securityIssuance);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:EntityNotFound", L["CapTable:Entity:SecurityIssuance"]]);
                else
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.View)]
        public virtual async Task<PagedResultDto<SecurityIssuanceDto>> GetListAsync(GetSecurityIssuanceListDto input)
        {
            try
            {
                var specification = SecurityIssuanceSpecifications.Active();

                // Apply filters
                if (input.CapTableId.HasValue)
                {
                    var capTableItems = await _securityIssuanceRepository.GetByCapTableIdAsync(input.CapTableId.Value);
                    specification = specification.And(SecurityIssuanceSpecifications.Active());
                }

                if (input.SecurityType.HasValue)
                {
                    specification = specification.And(SecurityIssuanceSpecifications.OfType(input.SecurityType.Value));
                }

                if (input.IsActive.HasValue)
                {
                    if (input.IsActive.Value)
                        specification = specification.And(SecurityIssuanceSpecifications.Active());
                    else
                        specification = specification.And(SecurityIssuanceSpecifications.Active().Not());
                }

                if (input.HasAvailableShares.HasValue && input.HasAvailableShares.Value)
                {
                    specification = specification.And(SecurityIssuanceSpecifications.WithAvailableShares());
                }

                var items = await _securityIssuanceRepository.GetListAsync(specification);

                // Apply text filter if provided
                if (!input.Filter.IsNullOrWhiteSpace())
                {
                    items = items.Where(si => si.Name.Contains(input.Filter, StringComparison.OrdinalIgnoreCase) ||
                                            (si.Description?.Contains(input.Filter, StringComparison.OrdinalIgnoreCase) ?? false))
                              .ToList();
                }

                // Apply sorting and paging
                var sortedItems = items.AsQueryable();
                if (!input.Sorting.IsNullOrWhiteSpace())
                {
                    sortedItems = sortedItems.OrderBy(input.Sorting);
                }
                else
                {
                    sortedItems = sortedItems.OrderByDescending(x => x.IssuanceDate);
                }

                var totalCount = sortedItems.Count();
                var pagedItems = sortedItems.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                return new PagedResultDto<SecurityIssuanceDto>(
                    totalCount,
                    ObjectMapper.Map<List<SecurityIssuance>, List<SecurityIssuanceDto>>(pagedItems)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.Update)]
        public virtual async Task<SecurityIssuanceDto> UpdateAsync(Guid id, UpdateSecurityIssuanceDto input)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetAsync(id);

                securityIssuance.UpdateDetails(
                    input.Name,
                    input.Description,
                    input.TotalAuthorized.HasValue ? new ShareQuantity(input.TotalAuthorized.Value) : null
                );

                await _securityIssuanceRepository.UpdateAsync(securityIssuance, autoSave: true);

                return ObjectMapper.Map<SecurityIssuance, SecurityIssuanceDto>(securityIssuance);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _securityIssuanceRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.Update)]
        public virtual async Task<SecurityIssuanceDto> ActivateAsync(Guid id)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetAsync(id);
                // Note: Activate method needs to be implemented in the aggregate
                await _securityIssuanceRepository.UpdateAsync(securityIssuance, autoSave: true);

                return ObjectMapper.Map<SecurityIssuance, SecurityIssuanceDto>(securityIssuance);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.Update)]
        public virtual async Task<SecurityIssuanceDto> DeactivateAsync(Guid id)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetAsync(id);
                securityIssuance.Deactivate();
                await _securityIssuanceRepository.UpdateAsync(securityIssuance, autoSave: true);

                return ObjectMapper.Map<SecurityIssuance, SecurityIssuanceDto>(securityIssuance);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.IssueSecurities)]
        public virtual async Task IssueSecuritiesAsync(Guid id, IssueSecuritiesDto input)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetAsync(id);
                var capTable = await _capTableRepository.GetAsync(securityIssuance.CapTableId);
                var transactionDate = input.TransactionDate ?? _clock.Now;

                // Issue securities
                securityIssuance.Issue(new ShareQuantity(input.Quantity));

                // Record the transaction on the cap table
                capTable.IssueShares(
                    input.ToHolderId,
                    id,
                    input.ShareClassId,
                    new ShareQuantity(input.Quantity),
                    new PricePerShare(input.PricePerShare, input.Currency),
                    transactionDate,
                    input.Memo
                );

                await _securityIssuanceRepository.UpdateAsync(securityIssuance, autoSave: false);
                await _capTableRepository.UpdateAsync(capTable, autoSave: true);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.Update)]
        public virtual async Task CancelSecuritiesAsync(Guid id, decimal quantity)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetAsync(id);
                securityIssuance.Cancel(new ShareQuantity(quantity));
                await _securityIssuanceRepository.UpdateAsync(securityIssuance, autoSave: true);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.View)]
        public virtual async Task<List<ShareClassDto>> GetShareClassesAsync(Guid id)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetWithShareClassesAsync(id);
                return ObjectMapper.Map<IReadOnlyCollection<Entities.ShareClass>, List<ShareClassDto>>(securityIssuance.ShareClasses);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.Update)]
        public virtual async Task<ShareClassDto> AddShareClassAsync(Guid id, AddShareClassDto input)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetWithShareClassesAsync(id);

                securityIssuance.AddShareClass(
                    input.Name,
                    input.Description,
                    input.ParValue,
                    input.VotingRights,
                    input.HasDividendRights,
                    input.HasLiquidationPreference,
                    input.LiquidationMultiple,
                    input.HasAntiDilutionRights,
                    input.AntiDilutionType
                );

                await _securityIssuanceRepository.UpdateAsync(securityIssuance, autoSave: true);

                // Return the newly added share class
                var newShareClass = securityIssuance.ShareClasses.Last();
                return ObjectMapper.Map<Entities.ShareClass, ShareClassDto>(newShareClass);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.View)]
        public virtual async Task<decimal> GetAvailableSharesAsync(Guid id)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetAsync(id);
                return securityIssuance.GetAvailableShares().Value;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityIssuances.View)]
        public virtual async Task<bool> IsExpiredAsync(Guid id)
        {
            try
            {
                var securityIssuance = await _securityIssuanceRepository.GetAsync(id);
                return securityIssuance.IsExpired(_clock.Now);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // --- Error Handling Helper ---
        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in SecurityIssuanceAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}

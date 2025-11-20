using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Localization;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Permissions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools
{
    /// <summary>
    /// Application service implementation for OptionPool management
    /// </summary>
    [Authorize(CapTablePermissions.OptionPools.Default)]
    public class OptionPoolAppService : ApplicationService, IOptionPoolAppService
    {
        // --- Dependencies ---
        private readonly IOptionPoolRepository _optionPoolRepository;
        private readonly ICapTableRepository _capTableRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public OptionPoolAppService(
            IOptionPoolRepository optionPoolRepository,
            ICapTableRepository capTableRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _optionPoolRepository = optionPoolRepository;
            _capTableRepository = capTableRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(CapTableResource);
        }

        [Authorize(CapTablePermissions.OptionPools.Create)]
        public virtual async Task<OptionPoolDto> CreateAsync(CreateOptionPoolDto input)
        {
            try
            {
                var newId = _guidGenerator.Create();
                var currentTenantId = _currentTenant.Id;

                var totalReserved = new ShareQuantity(input.TotalReserved);
                var poolPercentage = new OwnershipPercentage(input.PoolPercentage);
                var vestingTerms = new VestingTerms(
                    input.DefaultVestingPeriodMonths,
                    input.DefaultCliffMonths,
                    input.DefaultVestingPercentage
                );

                var optionPool = OptionPool.Create(
                    newId,
                    input.CapTableId,
                    input.Name,
                    totalReserved,
                    poolPercentage,
                    vestingTerms,
                    input.Description,
                    currentTenantId
                );

                await _optionPoolRepository.InsertAsync(optionPool, autoSave: true);

                return ObjectMapper.Map<OptionPool, OptionPoolDto>(optionPool);
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

        [Authorize(CapTablePermissions.OptionPools.View)]
        public virtual async Task<OptionPoolDto> GetAsync(Guid id)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);
                return ObjectMapper.Map<OptionPool, OptionPoolDto>(optionPool);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:EntityNotFound", L["CapTable:Entity:OptionPool"]]);
                else
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.OptionPools.View)]
        public virtual async Task<PagedResultDto<OptionPoolDto>> GetListAsync(GetOptionPoolListDto input)
        {
            try
            {
                var items = await _optionPoolRepository.GetActivePoolsAsync();

                // Apply filters
                if (input.CapTableId.HasValue)
                {
                    items = items.Where(op => op.CapTableId == input.CapTableId.Value).ToList();
                }

                if (input.IsActive.HasValue)
                {
                    items = items.Where(op => op.IsActive == input.IsActive.Value).ToList();
                }

                if (input.HasAvailableOptions.HasValue && input.HasAvailableOptions.Value)
                {
                    items = items.Where(op => op.GetAvailableOptions().Value > 0).ToList();
                }

                // Apply text filter if provided
                if (!input.Filter.IsNullOrWhiteSpace())
                {
                    items = items.Where(op => op.Name.Contains(input.Filter, StringComparison.OrdinalIgnoreCase) ||
                                            (op.Description?.Contains(input.Filter, StringComparison.OrdinalIgnoreCase) ?? false))
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
                    sortedItems = sortedItems.OrderByDescending(x => x.CreatedDate);
                }

                var totalCount = sortedItems.Count();
                var pagedItems = sortedItems.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                return new PagedResultDto<OptionPoolDto>(
                    totalCount,
                    ObjectMapper.Map<List<OptionPool>, List<OptionPoolDto>>(pagedItems)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.OptionPools.Update)]
        public virtual async Task<OptionPoolDto> UpdateAsync(Guid id, UpdateOptionPoolDto input)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);

                var vestingTerms = optionPool.DefaultVestingTerms;
                if (input.DefaultVestingPeriodMonths.HasValue || input.DefaultCliffMonths.HasValue || input.DefaultVestingPercentage.HasValue)
                {
                    vestingTerms = new VestingTerms(
                        input.DefaultVestingPeriodMonths ?? vestingTerms.VestingPeriodMonths,
                        input.DefaultCliffMonths ?? vestingTerms.CliffMonths,
                        input.DefaultVestingPercentage ?? vestingTerms.VestingPercentage
                    );
                }

                optionPool.UpdateDetails(
                    input.Name,
                    input.Description,
                    vestingTerms
                );

                await _optionPoolRepository.UpdateAsync(optionPool, autoSave: true);

                return ObjectMapper.Map<OptionPool, OptionPoolDto>(optionPool);
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

        [Authorize(CapTablePermissions.OptionPools.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _optionPoolRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.OptionPools.Update)]
        public virtual async Task<OptionPoolDto> ActivateAsync(Guid id)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);
                // Activate method would need to be added to the aggregate
                await _optionPoolRepository.UpdateAsync(optionPool, autoSave: true);

                return ObjectMapper.Map<OptionPool, OptionPoolDto>(optionPool);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.OptionPools.Update)]
        public virtual async Task<OptionPoolDto> DeactivateAsync(Guid id)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);
                optionPool.Deactivate();
                await _optionPoolRepository.UpdateAsync(optionPool, autoSave: true);

                return ObjectMapper.Map<OptionPool, OptionPoolDto>(optionPool);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.OptionPools.GrantOptions)]
        public virtual async Task GrantOptionsAsync(Guid id, GrantOptionsDto input)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);
                var capTable = await _capTableRepository.GetAsync(optionPool.CapTableId);
                var grantDate = input.GrantDate ?? _clock.Now;

                // Grant options from the pool
                optionPool.GrantOptions(new ShareQuantity(input.Quantity));

                // Create option details
                var optionDetails = new OptionDetails(input.StrikePrice, input.ExpiryDate, input.Currency);

                // Record the option grant on the cap table
                capTable.GrantOptions(
                    input.ToHolderId,
                    input.SecurityIssuanceId,
                    new ShareQuantity(input.Quantity),
                    optionDetails,
                    grantDate,
                    input.Memo
                );

                await _optionPoolRepository.UpdateAsync(optionPool, autoSave: false);
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

        [Authorize(CapTablePermissions.OptionPools.Update)]
        public virtual async Task ExerciseOptionsAsync(Guid id, decimal quantity)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);
                optionPool.ExerciseOptions(new ShareQuantity(quantity));
                await _optionPoolRepository.UpdateAsync(optionPool, autoSave: true);
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

        [Authorize(CapTablePermissions.OptionPools.Update)]
        public virtual async Task ForfeitOptionsAsync(Guid id, decimal quantity)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);
                optionPool.ForfeitOptions(new ShareQuantity(quantity));
                await _optionPoolRepository.UpdateAsync(optionPool, autoSave: true);
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

        [Authorize(CapTablePermissions.OptionPools.Update)]
        public virtual async Task<OptionPoolDto> ExpandPoolAsync(Guid id, ExpandPoolDto input)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);
                optionPool.ExpandPool(
                    new ShareQuantity(input.AdditionalShares),
                    new OwnershipPercentage(input.NewPoolPercentage)
                );
                await _optionPoolRepository.UpdateAsync(optionPool, autoSave: true);

                return ObjectMapper.Map<OptionPool, OptionPoolDto>(optionPool);
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

        [Authorize(CapTablePermissions.OptionPools.View)]
        public virtual async Task<decimal> GetAvailableOptionsAsync(Guid id)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);
                return optionPool.GetAvailableOptions().Value;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.OptionPools.View)]
        public virtual async Task<decimal> GetOutstandingOptionsAsync(Guid id)
        {
            try
            {
                var optionPool = await _optionPoolRepository.GetAsync(id);
                return optionPool.GetOutstandingOptions().Value;
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
            Logger.Log(logLevel, exception, "Error in OptionPoolAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}

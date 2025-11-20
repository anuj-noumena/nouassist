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
using NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Services;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders
{
    /// <summary>
    /// Application service implementation for SecurityHolder management
    /// </summary>
    [Authorize(CapTablePermissions.SecurityHolders.Default)]
    public class SecurityHolderAppService : ApplicationService, ISecurityHolderAppService
    {
        // --- Dependencies ---
        private readonly ISecurityHolderRepository _securityHolderRepository;
        private readonly ISecurityIssuanceRepository _securityIssuanceRepository;
        private readonly IVestingService _vestingService;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public SecurityHolderAppService(
            ISecurityHolderRepository securityHolderRepository,
            ISecurityIssuanceRepository securityIssuanceRepository,
            IVestingService vestingService,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _securityHolderRepository = securityHolderRepository;
            _securityIssuanceRepository = securityIssuanceRepository;
            _vestingService = vestingService;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(CapTableResource);
        }

        [Authorize(CapTablePermissions.SecurityHolders.Create)]
        public virtual async Task<SecurityHolderDto> CreateAsync(CreateSecurityHolderDto input)
        {
            try
            {
                // Check for duplicate name
                if (await _securityHolderRepository.ExistsByNameAsync(input.Name))
                {
                    throw new BusinessException("CapTable:ErrorMessage:DuplicateSecurityHolder")
                        .WithData("holderName", input.Name);
                }

                var newId = _guidGenerator.Create();
                var currentTenantId = _currentTenant.Id;

                var securityHolder = SecurityHolder.Create(
                    newId,
                    input.Name,
                    input.HolderType,
                    input.Email,
                    input.TaxId,
                    input.Address,
                    input.DateJoined,
                    input.ExternalReference,
                    currentTenantId
                );

                await _securityHolderRepository.InsertAsync(securityHolder, autoSave: true);

                return ObjectMapper.Map<SecurityHolder, SecurityHolderDto>(securityHolder);
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

        [Authorize(CapTablePermissions.SecurityHolders.View)]
        public virtual async Task<SecurityHolderDto> GetAsync(Guid id)
        {
            try
            {
                var securityHolder = await _securityHolderRepository.GetWithVestingSchedulesAsync(id);
                return ObjectMapper.Map<SecurityHolder, SecurityHolderDto>(securityHolder);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:EntityNotFound", L["CapTable:Entity:SecurityHolder"]]);
                else
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityHolders.View)]
        public virtual async Task<PagedResultDto<SecurityHolderDto>> GetListAsync(GetSecurityHolderListDto input)
        {
            try
            {
                var specification = SecurityHolderSpecifications.Active();

                // Apply filters
                if (input.HolderType.HasValue)
                {
                    specification = specification.And(SecurityHolderSpecifications.OfType(input.HolderType.Value));
                }

                if (input.IsActive.HasValue)
                {
                    if (input.IsActive.Value)
                        specification = specification.And(SecurityHolderSpecifications.Active());
                    else
                        specification = specification.And(SecurityHolderSpecifications.Active().Not());
                }

                int totalCount = await _securityHolderRepository.GetCountAsync();
                List<SecurityHolder> items = new List<SecurityHolder>();

                if (totalCount > 0)
                {
                    items = await _securityHolderRepository.GetListAsync(specification);

                    // Apply text filter if provided
                    if (!input.Filter.IsNullOrWhiteSpace())
                    {
                        items = items.Where(h => h.Name.Contains(input.Filter, StringComparison.OrdinalIgnoreCase) ||
                                               (h.Email?.Contains(input.Filter, StringComparison.OrdinalIgnoreCase) ?? false))
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
                        sortedItems = sortedItems.OrderBy(x => x.Name);
                    }

                    items = sortedItems.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                    totalCount = sortedItems.Count();
                }

                return new PagedResultDto<SecurityHolderDto>(
                    totalCount,
                    ObjectMapper.Map<List<SecurityHolder>, List<SecurityHolderDto>>(items)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityHolders.Update)]
        public virtual async Task<SecurityHolderDto> UpdateAsync(Guid id, UpdateSecurityHolderDto input)
        {
            try
            {
                var securityHolder = await _securityHolderRepository.GetAsync(id);

                securityHolder.UpdateInfo(
                    input.Name,
                    input.Email,
                    input.TaxId,
                    input.Address
                );

                await _securityHolderRepository.UpdateAsync(securityHolder, autoSave: true);

                return ObjectMapper.Map<SecurityHolder, SecurityHolderDto>(securityHolder);
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

        [Authorize(CapTablePermissions.SecurityHolders.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _securityHolderRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityHolders.Update)]
        public virtual async Task<SecurityHolderDto> ActivateAsync(Guid id)
        {
            try
            {
                var securityHolder = await _securityHolderRepository.GetAsync(id);
                securityHolder.Activate();
                await _securityHolderRepository.UpdateAsync(securityHolder, autoSave: true);

                return ObjectMapper.Map<SecurityHolder, SecurityHolderDto>(securityHolder);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityHolders.Update)]
        public virtual async Task<SecurityHolderDto> DeactivateAsync(Guid id)
        {
            try
            {
                var securityHolder = await _securityHolderRepository.GetAsync(id);
                securityHolder.Deactivate();
                await _securityHolderRepository.UpdateAsync(securityHolder, autoSave: true);

                return ObjectMapper.Map<SecurityHolder, SecurityHolderDto>(securityHolder);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityHolders.View)]
        public virtual async Task<List<VestingScheduleDto>> GetVestingSchedulesAsync(Guid holderId)
        {
            try
            {
                var schedules = await _vestingService.GetVestingScheduleAsync(holderId);
                return ObjectMapper.Map<List<VestingScheduleInfo>, List<VestingScheduleDto>>(schedules);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityHolders.ManageVesting)]
        public virtual async Task<VestingScheduleDto> AddVestingScheduleAsync(Guid holderId, AddVestingScheduleDto input)
        {
            try
            {
                var securityHolder = await _securityHolderRepository.GetWithVestingSchedulesAsync(holderId);
                var vestingTerms = new VestingTerms(input.VestingPeriodMonths, input.CliffMonths, input.VestingPercentage);

                securityHolder.AddVestingSchedule(
                    input.SecurityIssuanceId,
                    input.GrantDate,
                    vestingTerms,
                    new ShareQuantity(input.TotalShares)
                );

                await _securityHolderRepository.UpdateAsync(securityHolder, autoSave: true);

                // Get the newly added vesting schedule
                var newSchedule = securityHolder.VestingSchedules.Last();
                return ObjectMapper.Map<Entities.VestingSchedule, VestingScheduleDto>(newSchedule);
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

        [Authorize(CapTablePermissions.SecurityHolders.ManageVesting)]
        public virtual async Task UpdateVestingAsync(Guid holderId, DateTime asOfDate)
        {
            try
            {
                var securityHolder = await _securityHolderRepository.GetWithVestingSchedulesAsync(holderId);
                securityHolder.UpdateVesting(asOfDate);
                await _securityHolderRepository.UpdateAsync(securityHolder, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityHolders.ManageVesting)]
        public virtual async Task ExerciseOptionsAsync(Guid holderId, ExerciseOptionsDto input)
        {
            try
            {
                var exerciseDate = input.ExerciseDate ?? _clock.Now;
                await _vestingService.ProcessOptionExerciseAsync(
                    holderId,
                    input.SecurityIssuanceId,
                    new ShareQuantity(input.Quantity),
                    exerciseDate
                );
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

        [Authorize(CapTablePermissions.SecurityHolders.View)]
        public virtual async Task<decimal> GetTotalVestedSharesAsync(Guid holderId, DateTime asOfDate)
        {
            try
            {
                var vestedShares = await _vestingService.CalculateVestedAmountAsync(holderId, asOfDate);
                return vestedShares.Value;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.SecurityHolders.View)]
        public virtual async Task<decimal> GetExercisableSharesAsync(Guid holderId, DateTime asOfDate)
        {
            try
            {
                var exercisableShares = await _vestingService.GetExercisableSharesAsync(holderId, asOfDate);
                return exercisableShares.Value;
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
            Logger.Log(logLevel, exception, "Error in SecurityHolderAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}

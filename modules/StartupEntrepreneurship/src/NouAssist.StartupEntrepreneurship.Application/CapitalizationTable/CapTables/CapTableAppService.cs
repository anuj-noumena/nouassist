using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Localization;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Permissions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables
{
    /// <summary>
    /// Application service implementation for CapTable management
    /// </summary>
    [Authorize(CapTablePermissions.CapTables.Default)]
    public class CapTableAppService : ApplicationService, ICapTableAppService
    {
        // --- Dependencies ---
        private readonly ICapTableRepository _capTableRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public CapTableAppService(
            ICapTableRepository capTableRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _capTableRepository = capTableRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(CapTableResource);
        }

        [Authorize(CapTablePermissions.CapTables.Create)]
        public virtual async Task<CapTableDto> CreateAsync(CreateCapTableDto input)
        {
            try
            {
                var newId = _guidGenerator.Create();
                var currentTime = _clock.Now;
                var currentTenantId = _currentTenant.Id;
                var asOfDate = input.AsOfDate ?? currentTime;

                var capTable = Aggregates.CapTable.Create(
                    newId,
                    input.CompanyName,
                    new ShareQuantity(input.TotalAuthorizedShares),
                    asOfDate,
                    input.CompanyIdentifier,
                    currentTenantId
                );

                await _capTableRepository.InsertAsync(capTable, autoSave: true);

                return ObjectMapper.Map<Aggregates.CapTable, CapTableDto>(capTable);
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

        [Authorize(CapTablePermissions.CapTables.View)]
        public virtual async Task<CapTableDto> GetAsync(Guid id)
        {
            try
            {
                var capTable = await _capTableRepository.GetAsync(id);
                return ObjectMapper.Map<Aggregates.CapTable, CapTableDto>(capTable);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:EntityNotFound", L["CapTable:Entity:CapTable"]]);
                else
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.CapTables.View)]
        public virtual async Task<PagedResultDto<CapTableDto>> GetListAsync(GetCapTableListDto input)
        {
            try
            {
                var specification = CapTableSpecifications.Active();

                // Apply filters
                if (!input.Filter.IsNullOrWhiteSpace())
                {
                    specification = specification.And(CapTableSpecifications.ForCompany(input.Filter));
                }

                if (input.IsActive.HasValue)
                {
                    if (input.IsActive.Value)
                        specification = specification.And(CapTableSpecifications.Active());
                    else
                        specification = specification.And(CapTableSpecifications.Active().Not());
                }

                int totalCount = await _capTableRepository.CountAsync(specification);
                List<Aggregates.CapTable> items = new List<Aggregates.CapTable>();

                if (totalCount > 0)
                {
                    items = await _capTableRepository.GetListAsync(specification);
                    // Apply sorting and paging manually since we're using specifications
                    var sortedItems = items.AsQueryable();
                    if (!input.Sorting.IsNullOrWhiteSpace())
                    {
                        sortedItems = sortedItems.OrderBy(input.Sorting);
                    }
                    else
                    {
                        sortedItems = sortedItems.OrderByDescending(x => x.AsOfDate);
                    }

                    items = sortedItems.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                }

                return new PagedResultDto<CapTableDto>(
                    totalCount,
                    ObjectMapper.Map<List<Aggregates.CapTable>, List<CapTableDto>>(items)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.CapTables.Update)]
        public virtual async Task<CapTableDto> UpdateAsync(Guid id, UpdateCapTableDto input)
        {
            try
            {
                var capTable = await _capTableRepository.GetAsync(id);

                // Update basic properties through a method that handles validation
                capTable.UpdateAsOfDate(input.AsOfDate ?? _clock.Now);

                await _capTableRepository.UpdateAsync(capTable, autoSave: true);

                return ObjectMapper.Map<Aggregates.CapTable, CapTableDto>(capTable);
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

        [Authorize(CapTablePermissions.CapTables.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _capTableRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.CapTables.Update)]
        public virtual async Task<CapTableDto> ActivateAsync(Guid id)
        {
            try
            {
                var capTable = await _capTableRepository.GetAsync(id);
                capTable.Activate();
                await _capTableRepository.UpdateAsync(capTable, autoSave: true);

                return ObjectMapper.Map<Aggregates.CapTable, CapTableDto>(capTable);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.CapTables.Update)]
        public virtual async Task<CapTableDto> DeactivateAsync(Guid id)
        {
            try
            {
                var capTable = await _capTableRepository.GetAsync(id);
                capTable.Deactivate();
                await _capTableRepository.UpdateAsync(capTable, autoSave: true);

                return ObjectMapper.Map<Aggregates.CapTable, CapTableDto>(capTable);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.CapTables.ManageValuation)]
        public virtual async Task<CapTableDto> RecordValuationAsync(Guid id, RecordValuationDto input)
        {
            try
            {
                var capTable = await _capTableRepository.GetAsync(id);
                var valuation = new Valuation(input.PreMoneyValuation, input.Investment, input.Currency);

                capTable.RecordValuation(
                    input.ValuationDate,
                    valuation,
                    input.Source,
                    input.Notes,
                    input.FundingRoundId
                );

                await _capTableRepository.UpdateAsync(capTable, autoSave: true);

                return ObjectMapper.Map<Aggregates.CapTable, CapTableDto>(capTable);
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

        [Authorize(CapTablePermissions.CapTables.Update)]
        public virtual async Task<CapTableDto> UpdateAsOfDateAsync(Guid id, DateTime newAsOfDate)
        {
            try
            {
                var capTable = await _capTableRepository.GetAsync(id);
                capTable.UpdateAsOfDate(newAsOfDate);
                await _capTableRepository.UpdateAsync(capTable, autoSave: true);

                return ObjectMapper.Map<Aggregates.CapTable, CapTableDto>(capTable);
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

        // --- Error Handling Helper ---
        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in CapTableAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}

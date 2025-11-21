using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Localization;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Permissions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds
{
    /// <summary>
    /// Application service implementation for FundingRound management
    /// </summary>
    [Authorize(CapTablePermissions.FundingRounds.Default)]
    public class FundingRoundAppService : ApplicationService, IFundingRoundAppService
    {
        // --- Dependencies ---
        private readonly IFundingRoundRepository _fundingRoundRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public FundingRoundAppService(
            IFundingRoundRepository fundingRoundRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _fundingRoundRepository = fundingRoundRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(CapTableResource);
        }

        [Authorize(CapTablePermissions.FundingRounds.Create)]
        public virtual async Task<FundingRoundDto> CreateAsync(CreateFundingRoundDto input)
        {
            try
            {
                var newId = _guidGenerator.Create();
                var currentTenantId = _currentTenant.Id;
                var startDate = input.StartDate ?? _clock.Now;

                var valuation = new Valuation(input.PreMoneyValuation, 0, input.Currency);
                var targetAmount = new InvestmentAmount(input.TargetAmount, input.Currency);

                var fundingRound = FundingRound.Create(
                    newId,
                    input.CapTableId,
                    input.Name,
                    valuation,
                    targetAmount,
                    startDate,
                    input.Description,
                    input.LeadInvestor,
                    currentTenantId
                );

                await _fundingRoundRepository.InsertAsync(fundingRound, autoSave: true);

                return ObjectMapper.Map<FundingRound, FundingRoundDto>(fundingRound);
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

        [Authorize(CapTablePermissions.FundingRounds.View)]
        public virtual async Task<FundingRoundDto> GetAsync(Guid id)
        {
            try
            {
                var fundingRound = await _fundingRoundRepository.GetAsync(id);
                return ObjectMapper.Map<FundingRound, FundingRoundDto>(fundingRound);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is Volo.Abp.Domain.Entities.EntityNotFoundException)
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:EntityNotFound", L["CapTable:Entity:FundingRound"]]);
                else
                    throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.FundingRounds.View)]
        public virtual async Task<PagedResultDto<FundingRoundDto>> GetListAsync(GetFundingRoundListDto input)
        {
            try
            {
                var specification = FundingRoundSpecifications.InProgress(); // Default to show active rounds

                // Apply filters
                if (input.CapTableId.HasValue)
                {
                    var capTableRounds = await _fundingRoundRepository.GetByCapTableIdAsync(input.CapTableId.Value);
                    // Filter in memory since we already have the filtered list
                }

                if (input.Status.HasValue)
                {
                    specification = FundingRoundSpecifications.InProgress(); // Reset and apply specific status
                    switch (input.Status.Value)
                    {
                        case Enums.FundingRoundStatus.Active:
                            specification = FundingRoundSpecifications.Active();
                            break;
                        case Enums.FundingRoundStatus.Closed:
                            specification = FundingRoundSpecifications.Closed();
                            break;
                        default:
                            // Keep current specification
                            break;
                    }
                }

                var items = await _fundingRoundRepository.GetListAsync(specification);

                // Apply text filter if provided
                if (!input.Filter.IsNullOrWhiteSpace())
                {
                    items = items.Where(fr => fr.Name.Contains(input.Filter, StringComparison.OrdinalIgnoreCase) ||
                                            (fr.Description?.Contains(input.Filter, StringComparison.OrdinalIgnoreCase) ?? false))
                              .ToList();
                }

                // Apply date range filters
                if (input.StartDateFrom.HasValue)
                {
                    items = items.Where(fr => fr.StartDate >= input.StartDateFrom.Value).ToList();
                }

                if (input.StartDateTo.HasValue)
                {
                    items = items.Where(fr => fr.StartDate <= input.StartDateTo.Value).ToList();
                }

                // Apply sorting and paging
                var sortedItems = items.AsQueryable();
                if (!input.Sorting.IsNullOrWhiteSpace())
                {
                    sortedItems = sortedItems.OrderBy(input.Sorting);
                }
                else
                {
                    sortedItems = sortedItems.OrderByDescending(x => x.StartDate);
                }

                var totalCount = sortedItems.Count();
                var pagedItems = sortedItems.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                return new PagedResultDto<FundingRoundDto>(
                    totalCount,
                    ObjectMapper.Map<List<FundingRound>, List<FundingRoundDto>>(pagedItems)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.FundingRounds.Update)]
        public virtual async Task<FundingRoundDto> UpdateAsync(Guid id, UpdateFundingRoundDto input)
        {
            try
            {
                var fundingRound = await _fundingRoundRepository.GetAsync(id);

                fundingRound.UpdateDetails(
                    input.Name,
                    input.Description,
                    input.TargetAmount.HasValue ? new InvestmentAmount(input.TargetAmount.Value) : null,
                    input.LeadInvestor
                );

                await _fundingRoundRepository.UpdateAsync(fundingRound, autoSave: true);

                return ObjectMapper.Map<FundingRound, FundingRoundDto>(fundingRound);
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

        [Authorize(CapTablePermissions.FundingRounds.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _fundingRoundRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.FundingRounds.Update)]
        public virtual async Task<FundingRoundDto> ActivateAsync(Guid id)
        {
            try
            {
                var fundingRound = await _fundingRoundRepository.GetAsync(id);
                fundingRound.Activate();
                await _fundingRoundRepository.UpdateAsync(fundingRound, autoSave: true);

                return ObjectMapper.Map<FundingRound, FundingRoundDto>(fundingRound);
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

        [Authorize(CapTablePermissions.FundingRounds.Close)]
        public virtual async Task<FundingRoundDto> CloseAsync(Guid id, DateTime? closeDate = null)
        {
            try
            {
                var fundingRound = await _fundingRoundRepository.GetAsync(id);
                var actualCloseDate = closeDate ?? _clock.Now;
                fundingRound.Close(actualCloseDate);
                await _fundingRoundRepository.UpdateAsync(fundingRound, autoSave: true);

                return ObjectMapper.Map<FundingRound, FundingRoundDto>(fundingRound);
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

        [Authorize(CapTablePermissions.FundingRounds.Update)]
        public virtual async Task<FundingRoundDto> CancelAsync(Guid id)
        {
            try
            {
                var fundingRound = await _fundingRoundRepository.GetAsync(id);
                fundingRound.Cancel();
                await _fundingRoundRepository.UpdateAsync(fundingRound, autoSave: true);

                return ObjectMapper.Map<FundingRound, FundingRoundDto>(fundingRound);
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

        [Authorize(CapTablePermissions.FundingRounds.Update)]
        public virtual async Task RecordInvestmentAsync(Guid id, RecordInvestmentDto input)
        {
            try
            {
                var fundingRound = await _fundingRoundRepository.GetAsync(id);
                var investmentAmount = new InvestmentAmount(input.InvestmentAmount, input.Currency);
                var sharesIssued = new ShareQuantity(input.SharesIssued);

                fundingRound.RecordInvestment(investmentAmount, sharesIssued);
                await _fundingRoundRepository.UpdateAsync(fundingRound, autoSave: true);
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

        [Authorize(CapTablePermissions.FundingRounds.View)]
        public virtual async Task<decimal> GetCompletionPercentageAsync(Guid id)
        {
            try
            {
                var fundingRound = await _fundingRoundRepository.GetAsync(id);
                return fundingRound.GetCompletionPercentage();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.FundingRounds.View)]
        public virtual async Task<decimal> GetRemainingAmountAsync(Guid id)
        {
            try
            {
                var fundingRound = await _fundingRoundRepository.GetAsync(id);
                return fundingRound.GetRemainingAmount().Value;
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
            Logger.Log(logLevel, exception, "Error in FundingRoundAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}

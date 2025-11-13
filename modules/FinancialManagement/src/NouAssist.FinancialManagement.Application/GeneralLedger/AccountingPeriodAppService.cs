using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    [Authorize(FinancialManagementPermissions.GeneralLedger.Default)]
    public class AccountingPeriodAppService : ApplicationService, IAccountingPeriodAppService
    {
        private readonly IAccountingPeriodRepository _periodRepository;
        private readonly IJournalEntryRepository _journalEntryRepository;

        public AccountingPeriodAppService(
            IAccountingPeriodRepository periodRepository,
            IJournalEntryRepository journalEntryRepository)
        {
            _periodRepository = periodRepository;
            _journalEntryRepository = journalEntryRepository;
        }

        public async Task<AccountingPeriodDto> GetAsync(Guid id)
        {
            var period = await _periodRepository.GetAsync(id);
            return MapToDto(period);
        }

        public async Task<PagedResultDto<AccountingPeriodDto>> GetListAsync(GetAccountingPeriodListDto input)
        {
            var queryable = await _periodRepository.GetQueryableAsync();

            if (input.FiscalYear.HasValue)
            {
                queryable = queryable.Where(p => p.FiscalYear == input.FiscalYear.Value);
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(p => p.Status == input.Status.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(p => p.FiscalYear)
                .ThenByDescending(p => p.PeriodNumber)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var periods = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<AccountingPeriodDto>(
                totalCount,
                periods.Select(MapToDto).ToList()
            );
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ManageAccounts)]
        public async Task<AccountingPeriodDto> CreateAsync(CreateAccountingPeriodDto input)
        {
            // Check if period already exists
            var existing = await _periodRepository.GetByFiscalPeriodAsync(
                input.FiscalYear,
                input.PeriodNumber);

            if (existing != null)
            {
                throw new Volo.Abp.UserFriendlyException(
                    $"Period {input.PeriodNumber} for fiscal year {input.FiscalYear} already exists");
            }

            var period = new AccountingPeriod(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.FiscalYear,
                input.PeriodNumber,
                input.StartDate,
                input.EndDate);

            await _periodRepository.InsertAsync(period);

            return MapToDto(period);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ManageAccounts)]
        public async Task<AccountingPeriodDto> OpenAsync(Guid id)
        {
            var period = await _periodRepository.GetAsync(id);
            period.Open();
            await _periodRepository.UpdateAsync(period);
            return MapToDto(period);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ClosePeriods)]
        public async Task<AccountingPeriodDto> CloseAsync(Guid id, CloseAccountingPeriodDto input)
        {
            var period = await _periodRepository.GetAsync(id);

            // Validate all journal entries are posted
            var unpostedBatches = await _journalEntryRepository.GetUnpostedBatchesInPeriodAsync(id);
            if (unpostedBatches.Any())
            {
                throw new Volo.Abp.UserFriendlyException(
                    $"Cannot close period. There are {unpostedBatches.Count} unposted journal entries.");
            }

            period.Close(CurrentUser.Id.Value);
            await _periodRepository.UpdateAsync(period);

            return MapToDto(period);
        }

        public async Task<AccountingPeriodDto> GetCurrentPeriodAsync()
        {
            var period = await _periodRepository.GetPeriodForDateAsync(DateTime.Today);
            if (period == null)
            {
                throw new Volo.Abp.UserFriendlyException(
                    "No accounting period is defined for the current date");
            }

            return MapToDto(period);
        }

        private AccountingPeriodDto MapToDto(AccountingPeriod period)
        {
            var dto = ObjectMapper.Map<AccountingPeriod, AccountingPeriodDto>(period);
            dto.CanClose = period.Status == Enums.PeriodStatus.Open;
            return dto;
        }
    }
}

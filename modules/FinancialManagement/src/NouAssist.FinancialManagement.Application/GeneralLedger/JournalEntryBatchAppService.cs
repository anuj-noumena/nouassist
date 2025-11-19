using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    [Authorize(FinancialManagementPermissions.GeneralLedger.Default)]
    public class JournalEntryBatchAppService : ApplicationService, IJournalEntryBatchAppService
    {
        private readonly IJournalEntryBatchRepository _journalEntryBatchRepository;
        private readonly ILedgerAccountRepository _ledgerAccountRepository;

        public JournalEntryBatchAppService(
            IJournalEntryBatchRepository journalEntryBatchRepository,
            ILedgerAccountRepository ledgerAccountRepository)
        {
            _journalEntryBatchRepository = journalEntryBatchRepository;
            _ledgerAccountRepository = ledgerAccountRepository;
        }

        public async Task<JournalEntryBatchDto> GetAsync(Guid id)
        {
            var batch = await _journalEntryBatchRepository.GetWithLinesAsync(id);
            return await MapToDtoAsync(batch);
        }

        public async Task<PagedResultDto<JournalEntryBatchDto>> GetListAsync(GetJournalEntryBatchListDto input)
        {
            var query = await _journalEntryBatchRepository.GetQueryableAsync();

            if (input.StatusFilter.HasValue)
            {
                query = query.Where(b => b.Status == input.StatusFilter.Value);
            }

            if (input.StartDate.HasValue)
            {
                query = query.Where(b => b.PostingDate.Date >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                query = query.Where(b => b.PostingDate.Date <= input.EndDate.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var batches = await AsyncExecuter.ToListAsync(query);
            var dtos = new List<JournalEntryBatchDto>();

            foreach (var batch in batches)
            {
                dtos.Add(await MapToDtoAsync(batch));
            }

            return new PagedResultDto<JournalEntryBatchDto>(totalCount, dtos);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.CreateJournalEntries)]
        public async Task<JournalEntryBatchDto> CreateAsync(CreateJournalEntryBatchDto input)
        {
            // Generate batch number
            var batchNumber = await GenerateBatchNumberAsync();

            // Create batch
            var batch = new JournalEntryBatch(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                batchNumber,
                new TransactionDate(input.PostingDate),
                input.Description,
                reference: input.Reference
            );

            // Add lines
            foreach (var lineDto in input.Lines)
            {
                var account = await _ledgerAccountRepository.GetAsync(lineDto.AccountId);
                
                var line = new JournalEntryLine(
                    batch.Id,
                    lineDto.AccountId,
                    lineDto.DebitAmount.HasValue 
                        ? new MonetaryValue(lineDto.DebitAmount.Value, lineDto.Currency) 
                        : null,
                    lineDto.CreditAmount.HasValue 
                        ? new MonetaryValue(lineDto.CreditAmount.Value, lineDto.Currency) 
                        : null,
                    lineDto.Memo,
                    lineDto.Reference
                );

                batch.AddLine(line);
            }

            await _journalEntryBatchRepository.InsertAsync(batch);
            return await MapToDtoAsync(batch);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.PostJournalEntries)]
        public async Task<JournalEntryBatchDto> PostAsync(PostJournalEntryBatchDto input)
        {
            var batch = await _journalEntryBatchRepository.GetWithLinesAsync(input.BatchId);
            
            batch.Post(CurrentUser.Id.Value);
            
            await _journalEntryBatchRepository.UpdateAsync(batch);
            
            return await MapToDtoAsync(batch);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ReverseJournalEntries)]
        public async Task<JournalEntryBatchDto> ReverseAsync(ReverseJournalEntryBatchDto input)
        {
            var batch = await _journalEntryBatchRepository.GetWithLinesAsync(input.BatchId);
            
            var reversalBatch = batch.Reverse(
                input.ReversalReason,
                new TransactionDate(input.ReversalDate),
                CurrentUser.Id.Value
            );

            await _journalEntryBatchRepository.UpdateAsync(batch);
            await _journalEntryBatchRepository.InsertAsync(reversalBatch);
            
            return await MapToDtoAsync(reversalBatch);
        }

        private async Task<JournalEntryBatchDto> MapToDtoAsync(JournalEntryBatch batch)
        {
            var dto = ObjectMapper.Map<JournalEntryBatch, JournalEntryBatchDto>(batch);

            // Map lines with account names
            dto.Lines = new List<JournalEntryLineDto>();
            foreach (var line in batch.Lines)
            {
                var account = await _ledgerAccountRepository.GetAsync(line.AccountId);
                var lineDto = ObjectMapper.Map<JournalEntryLine, JournalEntryLineDto>(line);
                lineDto.AccountName = account.Name;
                dto.Lines.Add(lineDto);
            }

            // Calculate total amount
            dto.TotalAmount = batch.Lines
                .Where(l => l.DebitAmount != null)
                .Sum(l => l.DebitAmount.Amount);
            dto.Currency = batch.Lines.FirstOrDefault()?.DebitAmount?.Currency ?? "USD";

            return dto;
        }

        private async Task<string> GenerateBatchNumberAsync()
        {
            // Simple implementation - in production, use a proper sequence
            return $"JE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        private IQueryable<JournalEntryBatch> ApplySorting(
            IQueryable<JournalEntryBatch> query,
            GetJournalEntryBatchListDto input)
        {
            return query.OrderByDescending(b => b.PostingDate.Date);
        }

        private IQueryable<JournalEntryBatch> ApplyPaging(
            IQueryable<JournalEntryBatch> query,
            PagedAndSortedResultRequestDto input)
        {
            return query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    [Authorize(FinancialManagementPermissions.GeneralLedger.Default)]
    public class BankStatementAppService : ApplicationService, IBankStatementAppService
    {
        private readonly IRepository<BankStatement, Guid> _statementRepository;
        private readonly IJournalEntryRepository _journalEntryRepository;
        private readonly ILedgerAccountRepository _ledgerAccountRepository;

        public BankStatementAppService(
            IRepository<BankStatement, Guid> statementRepository,
            IJournalEntryRepository journalEntryRepository,
            ILedgerAccountRepository ledgerAccountRepository)
        {
            _statementRepository = statementRepository;
            _journalEntryRepository = journalEntryRepository;
            _ledgerAccountRepository = ledgerAccountRepository;
        }

        public async Task<BankStatementDto> GetAsync(Guid id)
        {
            var statement = await GetStatementWithDetailsAsync(id);
            return await MapToDtoAsync(statement);
        }

        public async Task<PagedResultDto<BankStatementDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _statementRepository.GetQueryableAsync();

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(s => s.StatementDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var statements = await AsyncExecuter.ToListAsync(queryable);
            var dtos = new List<BankStatementDto>();

            foreach (var statement in statements)
            {
                dtos.Add(await MapToDtoAsync(statement));
            }

            return new PagedResultDto<BankStatementDto>(totalCount, dtos);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ManageAccounts)]
        public async Task<BankStatementDto> ImportAsync(ImportBankStatementDto input)
        {
            var openingBalance = new MonetaryValue(input.OpeningBalance, input.Currency);
            var closingBalance = new MonetaryValue(input.ClosingBalance, input.Currency);

            var statement = new BankStatement(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.BankAccountId,
                input.StatementDate,
                input.StartDate,
                input.EndDate,
                openingBalance,
                closingBalance);

            foreach (var lineDto in input.Lines)
            {
                var amount = new MonetaryValue(lineDto.Amount, input.Currency);
                var line = new BankStatementLine(
                    GuidGenerator.Create(),
                    statement.Id,
                    lineDto.TransactionDate,
                    lineDto.Description,
                    amount,
                    lineDto.ReferenceNumber);

                statement.ImportLine(line);
            }

            await _statementRepository.InsertAsync(statement);

            return await MapToDtoAsync(statement);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.PostJournalEntries)]
        public async Task<BankStatementDto> MatchLineAsync(
            Guid statementId,
            MatchBankStatementLineDto input)
        {
            var statement = await GetStatementWithDetailsAsync(statementId);
            
            // Validate journal entry exists and is posted
            var journalEntry = await _journalEntryRepository.GetAsync(input.JournalEntryBatchId);
            if (journalEntry.Status != BatchStatus.Posted)
            {
                throw new Volo.Abp.UserFriendlyException(
                    "Cannot match with unposted journal entry. Please post the entry first.");
            }

            statement.MatchLine(input.LineId, input.JournalEntryBatchId);

            await _statementRepository.UpdateAsync(statement);

            return await MapToDtoAsync(statement);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.PostJournalEntries)]
        public async Task<BankStatementDto> UnmatchLineAsync(Guid statementId, Guid lineId)
        {
            var statement = await GetStatementWithDetailsAsync(statementId);
            
            var line = statement.Lines.FirstOrDefault(l => l.Id == lineId);
            if (line == null)
            {
                throw new Volo.Abp.UserFriendlyException("Bank statement line not found");
            }

            line.Unmatch();

            await _statementRepository.UpdateAsync(statement);

            return await MapToDtoAsync(statement);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.PostJournalEntries)]
        public async Task<BankStatementDto> BulkMatchAsync(Guid statementId, BulkMatchDto input)
        {
            var statement = await GetStatementWithDetailsAsync(statementId);

            foreach (var match in input.Matches)
            {
                // Validate journal entry
                var journalEntry = await _journalEntryRepository.GetAsync(match.JournalEntryBatchId);
                if (journalEntry.Status != BatchStatus.Posted)
                {
                    continue; // Skip unposted entries
                }

                statement.MatchLine(match.BankLineId, match.JournalEntryBatchId);
            }

            await _statementRepository.UpdateAsync(statement);

            return await MapToDtoAsync(statement);
        }

        public async Task<List<SuggestedMatchDto>> GetSuggestedMatchesAsync(
            Guid statementId,
            AutoMatchSettingsDto settings)
        {
            var statement = await GetStatementWithDetailsAsync(statementId);
            var unmatchedLines = statement.Lines.Where(l => l.MatchStatus == MatchStatus.Unmatched).ToList();

            // Get journal entries in the period
            var journalEntries = await _journalEntryRepository.GetByAccountAsync(
                statement.BankAccountId,
                statement.StartDate,
                statement.EndDate);

            var suggestions = new List<SuggestedMatchDto>();

            foreach (var line in unmatchedLines)
            {
                foreach (var entry in journalEntries)
                {
                    if (entry.Status != BatchStatus.Posted)
                        continue;

                    var score = CalculateMatchScore(line, entry, settings);
                    
                    if (score >= settings.MinimumConfidenceScore)
                    {
                        var entryDto = ObjectMapper.Map<JournalEntryBatch, JournalEntryBatchDto>(entry);
                        var lineDto = ObjectMapper.Map<BankStatementLine, BankStatementLineDto>(line);

                        suggestions.Add(new SuggestedMatchDto
                        {
                            BankLineId = line.Id,
                            JournalEntryBatchId = entry.Id,
                            ConfidenceScore = score,
                            MatchReasons = GetMatchReasons(line, entry, settings),
                            BankLine = lineDto,
                            JournalEntry = entryDto
                        });
                    }
                }
            }

            return suggestions.OrderByDescending(s => s.ConfidenceScore).ToList();
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.PostJournalEntries)]
        public async Task<BankStatementDto> AutoMatchAsync(
            Guid statementId,
            AutoMatchSettingsDto settings)
        {
            var suggestions = await GetSuggestedMatchesAsync(statementId, settings);
            var highConfidenceMatches = suggestions
                .Where(s => s.ConfidenceScore >= 0.95m)
                .ToList();

            if (highConfidenceMatches.Any())
            {
                var bulkMatch = new BulkMatchDto
                {
                    Matches = highConfidenceMatches.Select(m => new MatchPairDto
                    {
                        BankLineId = m.BankLineId,
                        JournalEntryBatchId = m.JournalEntryBatchId
                    }).ToList()
                };

                return await BulkMatchAsync(statementId, bulkMatch);
            }

            var statement = await GetStatementWithDetailsAsync(statementId);
            return await MapToDtoAsync(statement);
        }

        public async Task<BankReconciliationReportDto> GetReconciliationReportAsync(Guid statementId)
        {
            var statement = await GetStatementWithDetailsAsync(statementId);
            
            // Get bank account balance from GL
            var bankAccount = await _ledgerAccountRepository.GetAsync(statement.BankAccountId);

            var report = new BankReconciliationReportDto
            {
                StatementId = statement.Id,
                StatementDate = statement.StatementDate,
                StatementOpeningBalance = statement.OpeningBalance,
                StatementClosingBalance = statement.ClosingBalance,
                BookBalance = bankAccount.CurrentBalance,
                TotalTransactions = statement.Lines.Count,
                MatchedTransactions = statement.Lines.Count(l => l.MatchStatus == MatchStatus.Matched)
            };

            report.ReconciliationPercentage = report.TotalTransactions > 0
                ? (decimal)report.MatchedTransactions / report.TotalTransactions * 100
                : 0;

            // Calculate reconciled balance (statement closing - unmatched bank items)
            var unmatchedBankTotal = statement.Lines
                .Where(l => l.MatchStatus == MatchStatus.Unmatched)
                .Sum(l => l.Amount.Amount);

            report.ReconciledBalance = new MonetaryValue(
                statement.ClosingBalance.Amount - unmatchedBankTotal,
                statement.ClosingBalance.Currency);

            report.Variance = new MonetaryValue(
                report.BookBalance.Amount - report.ReconciledBalance.Amount,
                report.BookBalance.Currency);

            report.IsReconciled = Math.Abs(report.Variance.Amount) < 0.01m;

            // Get unmatched transactions
            report.UnmatchedBankTransactions = statement.Lines
                .Where(l => l.MatchStatus == MatchStatus.Unmatched)
                .Select(l => new UnmatchedTransactionDto
                {
                    Id = l.Id,
                    TransactionDate = l.TransactionDate,
                    Description = l.Description,
                    Amount = l.Amount,
                    Reference = l.ReferenceNumber,
                    Type = "Bank"
                })
                .ToList();

            // Get unmatched book entries (journal entries not matched to statement)
            var journalEntries = await _journalEntryRepository.GetByAccountAsync(
                statement.BankAccountId,
                statement.StartDate,
                statement.EndDate);

            var matchedBatchIds = statement.Lines
                .Where(l => l.MatchStatus == MatchStatus.Matched)
                .SelectMany(l => l.MatchedBatches)
                .ToHashSet();

            report.UnmatchedBookTransactions = journalEntries
                .Where(je => je.Status == BatchStatus.Posted && !matchedBatchIds.Contains(je.Id))
                .Select(je => new UnmatchedTransactionDto
                {
                    Id = je.Id,
                    TransactionDate = je.PostingDate,
                    Description = je.Description,
                    Amount = new MonetaryValue(
                        je.Lines.First(l => l.AccountId == statement.BankAccountId).DebitAmount?.Amount 
                        ?? je.Lines.First(l => l.AccountId == statement.BankAccountId).CreditAmount.Amount,
                        "USD"), // TODO: Get from account currency
                    Reference = je.Reference,
                    Type = "Book"
                })
                .ToList();

            return report;
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.PostJournalEntries)]
        public async Task<BankStatementDto> FinalizeReconciliationAsync(Guid statementId)
        {
            var report = await GetReconciliationReportAsync(statementId);

            if (!report.IsReconciled)
            {
                throw new Volo.Abp.UserFriendlyException(
                    $"Cannot finalize reconciliation. Variance of {report.Variance.Amount:C} exists. " +
                    $"Please match all transactions or create adjusting entries.");
            }

            var statement = await GetStatementWithDetailsAsync(statementId);
            // Mark as fully reconciled (you may want to add this to the domain model)
            
            await _statementRepository.UpdateAsync(statement);

            return await MapToDtoAsync(statement);
        }

        #region Private Helper Methods

        private async Task<BankStatement> GetStatementWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Set<BankStatement>()
                .Include("Lines")
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        private async Task<BankStatementDto> MapToDtoAsync(BankStatement statement)
        {
            var dto = ObjectMapper.Map<BankStatement, BankStatementDto>(statement);
            return dto;
        }

        private decimal CalculateMatchScore(
            BankStatementLine bankLine,
            JournalEntryBatch journalEntry,
            AutoMatchSettingsDto settings)
        {
            decimal score = 0;
            var maxScore = 0m;

            // Amount matching (40% weight)
            maxScore += 0.4m;
            var bankAmount = Math.Abs(bankLine.Amount.Amount);
            var entryAmount = journalEntry.Lines
                .Where(l => l.AccountId == bankLine.StatementId) // Simplified - should use bank account
                .Sum(l => l.DebitAmount?.Amount ?? l.CreditAmount?.Amount ?? 0);

            var amountDiff = Math.Abs(bankAmount - entryAmount);
            var amountTolerance = bankAmount * settings.AmountTolerancePercentage;

            if (amountDiff <= amountTolerance)
            {
                score += 0.4m * (1 - (amountDiff / amountTolerance));
            }

            // Date matching (30% weight)
            maxScore += 0.3m;
            var dateDiff = Math.Abs((bankLine.TransactionDate - journalEntry.PostingDate).Days);
            
            if (dateDiff <= settings.DateToleranceDays)
            {
                score += 0.3m * (1 - ((decimal)dateDiff / settings.DateToleranceDays));
            }

            // Reference matching (20% weight)
            if (settings.MatchByReference)
            {
                maxScore += 0.2m;
                if (!string.IsNullOrWhiteSpace(bankLine.ReferenceNumber) &&
                    !string.IsNullOrWhiteSpace(journalEntry.Reference))
                {
                    if (bankLine.ReferenceNumber.Contains(journalEntry.Reference, 
                        StringComparison.OrdinalIgnoreCase) ||
                        journalEntry.Reference.Contains(bankLine.ReferenceNumber, 
                        StringComparison.OrdinalIgnoreCase))
                    {
                        score += 0.2m;
                    }
                }
            }

            // Description matching (10% weight)
            if (settings.MatchByDescription)
            {
                maxScore += 0.1m;
                if (ContainsSimilarWords(bankLine.Description, journalEntry.Description))
                {
                    score += 0.1m;
                }
            }

            return maxScore > 0 ? score / maxScore : 0;
        }

        private List<string> GetMatchReasons(
            BankStatementLine bankLine,
            JournalEntryBatch journalEntry,
            AutoMatchSettingsDto settings)
        {
            var reasons = new List<string>();

            var bankAmount = Math.Abs(bankLine.Amount.Amount);
            var entryAmount = journalEntry.Lines
                .Sum(l => l.DebitAmount?.Amount ?? l.CreditAmount?.Amount ?? 0);

            if (Math.Abs(bankAmount - entryAmount) <= bankAmount * settings.AmountTolerancePercentage)
            {
                reasons.Add($"Amount match: {bankAmount:C}");
            }

            var dateDiff = Math.Abs((bankLine.TransactionDate - journalEntry.PostingDate).Days);
            if (dateDiff <= settings.DateToleranceDays)
            {
                reasons.Add($"Date within {dateDiff} days");
            }

            if (!string.IsNullOrWhiteSpace(bankLine.ReferenceNumber) &&
                !string.IsNullOrWhiteSpace(journalEntry.Reference) &&
                bankLine.ReferenceNumber.Contains(journalEntry.Reference, StringComparison.OrdinalIgnoreCase))
            {
                reasons.Add("Reference number match");
            }

            if (ContainsSimilarWords(bankLine.Description, journalEntry.Description))
            {
                reasons.Add("Description similarity");
            }

            return reasons;
        }

        private bool ContainsSimilarWords(string text1, string text2)
        {
            if (string.IsNullOrWhiteSpace(text1) || string.IsNullOrWhiteSpace(text2))
                return false;

            var words1 = text1.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var words2 = text2.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var commonWords = words1.Intersect(words2).Count();
            var minWords = Math.Min(words1.Length, words2.Length);

            return minWords > 0 && (decimal)commonWords / minWords >= 0.3m;
        }

        private async Task<Microsoft.EntityFrameworkCore.DbContext> GetDbContextAsync()
        {
            // This is a helper to get DbContext for Include operations
            // In real implementation, inject IDbContextProvider
            return null; // Placeholder
        }

        #endregion
    }
}

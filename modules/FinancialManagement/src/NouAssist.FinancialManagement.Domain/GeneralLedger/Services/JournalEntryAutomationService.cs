using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.Shared.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.GeneralLedger.Services
{
    /// <summary>
    /// Domain service for automated journal entry creation from financial transactions
    /// Implements double-entry bookkeeping automation for AP/AR/FP modules
    /// </summary>
    public class JournalEntryAutomationService : DomainService
    {
        private readonly IJournalEntryBatchRepository _journalEntryBatchRepository;
        private readonly ILedgerAccountRepository _ledgerAccountRepository;
        private readonly JournalEntryValidationService _validationService;

        public JournalEntryAutomationService(
            IJournalEntryBatchRepository journalEntryBatchRepository,
            ILedgerAccountRepository ledgerAccountRepository,
            JournalEntryValidationService validationService)
        {
            _journalEntryBatchRepository = journalEntryBatchRepository;
            _ledgerAccountRepository = ledgerAccountRepository;
            _validationService = validationService;
        }

        /// <summary>
        /// Create journal entry for bill payment
        /// Debit: Expense Account
        /// Credit: Cash/Bank Account
        /// </summary>
        [UnitOfWork]
        public virtual async Task<JournalEntryBatch> CreateBillPaymentEntryAsync(
            Guid billId,
            string billNumber,
            MonetaryValue amount,
            Guid expenseAccountId,
            Guid paymentAccountId,
            DateTime paymentDate,
            Guid vendorId,
            string vendorName,
            string paymentReference,
            Guid paidBy,
            Guid? tenantId = null)
        {
            Check.NotNull(amount, nameof(amount));
            Check.NotNullOrWhiteSpace(billNumber, nameof(billNumber));

            // Generate batch number
            var batchNumber = await GenerateBatchNumberAsync("BP", tenantId);

            // Create journal entry batch
            var batch = new JournalEntryBatch(
                GuidGenerator.Create(),
                tenantId,
                batchNumber,
                new TransactionDate(paymentDate),
                $"Bill Payment: {billNumber} - {vendorName}",
                billId,
                EntityType.Bill,
                paymentReference
            );

            // Debit: Expense Account
            var debitLine = new JournalEntryLine(
                batch.Id,
                expenseAccountId,
                amount, // Debit
                null,   // No Credit
                $"Bill #{billNumber} - {vendorName}",
                paymentReference,
                billId,
                EntityType.Bill
            );
            batch.AddLine(debitLine);

            // Credit: Cash/Bank Account
            var creditLine = new JournalEntryLine(
                batch.Id,
                paymentAccountId,
                null,   // No Debit
                amount, // Credit
                $"Bill Payment #{billNumber}",
                paymentReference,
                billId,
                EntityType.Bill
            );
            batch.AddLine(creditLine);

            // Validate
            var validation = _validationService.ValidateForPosting(batch);
            if (!validation.IsValid)
            {
                throw new BusinessException("GL:ValidationFailed",
                    $"Journal entry validation failed: {string.Join(", ", validation.Errors)}");
            }

            // Post
            batch.Post(paidBy);

            // Save
            await _journalEntryBatchRepository.InsertAsync(batch, autoSave: true);

            return batch;
        }

        /// <summary>
        /// Create journal entry for invoice payment (customer payment received)
        /// Debit: Cash/Bank Account
        /// Credit: Accounts Receivable
        /// </summary>
        [UnitOfWork]
        public virtual async Task<JournalEntryBatch> CreateInvoicePaymentEntryAsync(
            Guid invoiceId,
            string invoiceNumber,
            MonetaryValue paymentAmount,
            Guid depositAccountId,
            Guid receivableAccountId,
            DateTime paymentDate,
            Guid customerId,
            string customerName,
            string paymentReference,
            Guid? tenantId = null)
        {
            Check.NotNull(paymentAmount, nameof(paymentAmount));
            Check.NotNullOrWhiteSpace(invoiceNumber, nameof(invoiceNumber));

            var batchNumber = await GenerateBatchNumberAsync("IP", tenantId);

            var batch = new JournalEntryBatch(
                GuidGenerator.Create(),
                tenantId,
                batchNumber,
                new TransactionDate(paymentDate),
                $"Invoice Payment: {invoiceNumber} - {customerName}",
                invoiceId,
                EntityType.Invoice,
                paymentReference
            );

            // Debit: Cash/Bank Account
            var debitLine = new JournalEntryLine(
                batch.Id,
                depositAccountId,
                paymentAmount, // Debit
                null,          // No Credit
                $"Payment received for Invoice #{invoiceNumber}",
                paymentReference,
                invoiceId,
                EntityType.Invoice
            );
            batch.AddLine(debitLine);

            // Credit: Accounts Receivable
            var creditLine = new JournalEntryLine(
                batch.Id,
                receivableAccountId,
                null,          // No Debit
                paymentAmount, // Credit
                $"Invoice #{invoiceNumber} - {customerName}",
                paymentReference,
                invoiceId,
                EntityType.Invoice
            );
            batch.AddLine(creditLine);

            var validation = _validationService.ValidateForPosting(batch);
            if (!validation.IsValid)
            {
                throw new BusinessException("GL:ValidationFailed",
                    $"Journal entry validation failed: {string.Join(", ", validation.Errors)}");
            }

            batch.Post(CurrentUser.Id ?? Guid.Empty);
            await _journalEntryBatchRepository.InsertAsync(batch, autoSave: true);

            return batch;
        }

        /// <summary>
        /// Create journal entry for invoice sent (accrual basis revenue recognition)
        /// Debit: Accounts Receivable
        /// Credit: Revenue Account
        /// </summary>
        [UnitOfWork]
        public virtual async Task<JournalEntryBatch> CreateInvoiceAccrualEntryAsync(
            Guid invoiceId,
            string invoiceNumber,
            MonetaryValue totalAmount,
            Guid receivableAccountId,
            Guid revenueAccountId,
            DateTime issueDate,
            Guid customerId,
            string customerName,
            Guid? tenantId = null)
        {
            Check.NotNull(totalAmount, nameof(totalAmount));
            Check.NotNullOrWhiteSpace(invoiceNumber, nameof(invoiceNumber));

            var batchNumber = await GenerateBatchNumberAsync("IS", tenantId);

            var batch = new JournalEntryBatch(
                GuidGenerator.Create(),
                tenantId,
                batchNumber,
                new TransactionDate(issueDate),
                $"Invoice Issued: {invoiceNumber} - {customerName}",
                invoiceId,
                EntityType.Invoice,
                invoiceNumber
            );

            // Debit: Accounts Receivable
            var debitLine = new JournalEntryLine(
                batch.Id,
                receivableAccountId,
                totalAmount, // Debit
                null,        // No Credit
                $"Invoice #{invoiceNumber} - {customerName}",
                invoiceNumber,
                invoiceId,
                EntityType.Invoice
            );
            batch.AddLine(debitLine);

            // Credit: Revenue Account
            var creditLine = new JournalEntryLine(
                batch.Id,
                revenueAccountId,
                null,        // No Debit
                totalAmount, // Credit
                $"Revenue from Invoice #{invoiceNumber}",
                invoiceNumber,
                invoiceId,
                EntityType.Invoice
            );
            batch.AddLine(creditLine);

            var validation = _validationService.ValidateForPosting(batch);
            if (!validation.IsValid)
            {
                throw new BusinessException("GL:ValidationFailed",
                    $"Journal entry validation failed: {string.Join(", ", validation.Errors)}");
            }

            batch.Post(CurrentUser.Id ?? Guid.Empty);
            await _journalEntryBatchRepository.InsertAsync(batch, autoSave: true);

            return batch;
        }

        /// <summary>
        /// Create journal entry for expense
        /// Debit: Expense Account
        /// Credit: Cash/Card Account
        /// </summary>
        [UnitOfWork]
        public virtual async Task<JournalEntryBatch> CreateExpenseEntryAsync(
            Guid expenseId,
            MonetaryValue amount,
            Guid expenseAccountId,
            Guid paymentAccountId,
            DateTime expenseDate,
            string description,
            Guid userId,
            string paymentMethod,
            Guid? tenantId = null)
        {
            Check.NotNull(amount, nameof(amount));
            Check.NotNullOrWhiteSpace(description, nameof(description));

            var batchNumber = await GenerateBatchNumberAsync("EX", tenantId);

            var batch = new JournalEntryBatch(
                GuidGenerator.Create(),
                tenantId,
                batchNumber,
                new TransactionDate(expenseDate),
                $"Expense: {description}",
                expenseId,
                EntityType.Expense,
                paymentMethod
            );

            // Debit: Expense Account
            var debitLine = new JournalEntryLine(
                batch.Id,
                expenseAccountId,
                amount, // Debit
                null,   // No Credit
                description,
                paymentMethod,
                expenseId,
                EntityType.Expense
            );
            batch.AddLine(debitLine);

            // Credit: Cash/Card Account
            var creditLine = new JournalEntryLine(
                batch.Id,
                paymentAccountId,
                null,   // No Debit
                amount, // Credit
                $"Payment: {description}",
                paymentMethod,
                expenseId,
                EntityType.Expense
            );
            batch.AddLine(creditLine);

            var validation = _validationService.ValidateForPosting(batch);
            if (!validation.IsValid)
            {
                throw new BusinessException("GL:ValidationFailed",
                    $"Journal entry validation failed: {string.Join(", ", validation.Errors)}");
            }

            batch.Post(userId);
            await _journalEntryBatchRepository.InsertAsync(batch, autoSave: true);

            return batch;
        }

        /// <summary>
        /// Create journal entry for debt payment
        /// Debit: Debt Liability Account (Principal) + Interest Expense Account (Interest)
        /// Credit: Cash/Bank Account (Total Payment)
        /// </summary>
        [UnitOfWork]
        public virtual async Task<JournalEntryBatch> CreateDebtPaymentEntryAsync(
            Guid paymentId,
            Guid debtAccountId,
            string debtAccountName,
            MonetaryValue principalAmount,
            MonetaryValue interestAmount,
            Guid debtLiabilityGLAccountId,
            Guid interestExpenseGLAccountId,
            Guid paymentGLAccountId,
            DateTime paymentDate,
            Guid userId,
            Guid? tenantId = null)
        {
            Check.NotNull(principalAmount, nameof(principalAmount));
            Check.NotNull(interestAmount, nameof(interestAmount));
            Check.NotNullOrWhiteSpace(debtAccountName, nameof(debtAccountName));

            var batchNumber = await GenerateBatchNumberAsync("DP", tenantId);
            var totalPayment = principalAmount.Add(interestAmount);

            var batch = new JournalEntryBatch(
                GuidGenerator.Create(),
                tenantId,
                batchNumber,
                new TransactionDate(paymentDate),
                $"Debt Payment: {debtAccountName}",
                paymentId,
                EntityType.Payment,
                $"DebtPayment-{debtAccountId}"
            );

            // Debit: Debt Liability Account (Principal)
            if (principalAmount.Amount > 0)
            {
                var principalLine = new JournalEntryLine(
                    batch.Id,
                    debtLiabilityGLAccountId,
                    principalAmount, // Debit
                    null,            // No Credit
                    $"Principal payment - {debtAccountName}",
                    $"Payment-{paymentId}",
                    paymentId,
                    EntityType.Payment
                );
                batch.AddLine(principalLine);
            }

            // Debit: Interest Expense Account (Interest)
            if (interestAmount.Amount > 0)
            {
                var interestLine = new JournalEntryLine(
                    batch.Id,
                    interestExpenseGLAccountId,
                    interestAmount, // Debit
                    null,           // No Credit
                    $"Interest payment - {debtAccountName}",
                    $"Payment-{paymentId}",
                    paymentId,
                    EntityType.Payment
                );
                batch.AddLine(interestLine);
            }

            // Credit: Cash/Bank Account (Total Payment)
            var paymentLine = new JournalEntryLine(
                batch.Id,
                paymentGLAccountId,
                null,         // No Debit
                totalPayment, // Credit
                $"Debt payment - {debtAccountName}",
                $"Payment-{paymentId}",
                paymentId,
                EntityType.Payment
            );
            batch.AddLine(paymentLine);

            var validation = _validationService.ValidateForPosting(batch);
            if (!validation.IsValid)
            {
                throw new BusinessException("GL:ValidationFailed",
                    $"Journal entry validation failed: {string.Join(", ", validation.Errors)}");
            }

            batch.Post(userId);
            await _journalEntryBatchRepository.InsertAsync(batch, autoSave: true);

            return batch;
        }

        /// <summary>
        /// Generate sequential batch number with prefix
        /// Format: {prefix}-YYYYMMDD-{sequence}
        /// </summary>
        private async Task<string> GenerateBatchNumberAsync(string prefix, Guid? tenantId)
        {
            // In production, this would use a sequence generator or database counter
            // For now, using timestamp + GUID to ensure uniqueness
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
            var sequence = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            return $"{prefix}-{timestamp}-{sequence}";
        }
    }
}

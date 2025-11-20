using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using NouAssist.FinancialManagement.AuditTrail.Enums;
using NouAssist.FinancialManagement.AuditTrail.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace NouAssist.FinancialManagement.AuditTrail.Services
{
    public class FinancialAuditService : DomainService
    {
        private readonly ILogger<FinancialAuditService> _logger;
        private readonly ICurrentUser _currentUser;

        public FinancialAuditService(
            ILogger<FinancialAuditService> logger,
            ICurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<AuditEntry> AuditBillPaymentAsync(
            Guid billId,
            string billNumber,
            decimal amount,
            string currency,
            Guid vendorId,
            string vendorName,
            string paymentMethod,
            Guid paidBy,
            Guid? tenantId = null)
        {
            var entry = new AuditEntry(
                GuidGenerator.Create(),
                tenantId,
                new EntityReference("Bill", billId.ToString(), billNumber),
                AuditEventType.Post,
                new UserContext(paidBy, _currentUser.UserName, GetClientIpAddress()),
                $"Paid bill {billNumber} with amount {amount} {currency}",
                null,
                null,
                new AuditMetadata(new Dictionary<string, object>
                {
                    ["VendorId"] = vendorId.ToString(),
                    ["VendorName"] = vendorName,
                    ["PaymentMethod"] = paymentMethod,
                }),
                AuditSeverity.High,
                ComplianceFlag.SOX | ComplianceFlag.FinancialTransaction
            );

            _logger.LogInformation(
                "Audit: Bill Payment - {BillNumber} - {Amount} {Currency} - {VendorName} by {User}",
                billNumber, amount, currency, vendorName, _currentUser.UserName);

            return await Task.FromResult(entry);
        }

        public async Task<AuditEntry> AuditInvoicePaymentAsync(
            Guid invoiceId,
            string invoiceNumber,
            decimal amount,
            string currency,
            Guid customerId,
            string customerName,
            string paymentSource,
            Guid? tenantId = null)
        {
            var entry = new AuditEntry(
                GuidGenerator.Create(),
                tenantId,
                new EntityReference("Invoice", invoiceId.ToString(), invoiceNumber),
                AuditEventType.Post,
                new UserContext(_currentUser.Id ?? Guid.Empty, _currentUser.UserName, GetClientIpAddress()),
                $"Received payment for invoice {invoiceNumber} with amount {amount} {currency}",
                null,
                null,
                new AuditMetadata(new Dictionary<string, object>
                {
                    ["CustomerId"] = customerId.ToString(),
                    ["CustomerName"] = customerName,
                    ["PaymentSource"] = paymentSource,
                }),
                AuditSeverity.High,
                ComplianceFlag.SOX | ComplianceFlag.FinancialTransaction
            );

            _logger.LogInformation(
                "Audit: Invoice Payment Received - {InvoiceNumber} - {Amount} {Currency} from {CustomerName}",
                invoiceNumber, amount, currency, customerName);

            return await Task.FromResult(entry);
        }

        public async Task<AuditEntry> AuditJournalEntryAsync(
            Guid batchId,
            string batchNumber,
            decimal totalAmount,
            string description,
            Guid postedBy,
            string sourceType,
            Guid? tenantId = null)
        {
            var entry = new AuditEntry(
                GuidGenerator.Create(),
                tenantId,
                new EntityReference("JournalEntryBatch", batchId.ToString(), batchNumber),
                AuditEventType.Post,
                new UserContext(postedBy, _currentUser.UserName, GetClientIpAddress()),
                $"Posted journal entry batch {batchNumber} with total amount {totalAmount}",
                null,
                null,
                new AuditMetadata(new Dictionary<string, object>
                {
                    ["Description"] = description,
                    ["SourceType"] = sourceType,
                }),
                AuditSeverity.High,
                ComplianceFlag.SOX | ComplianceFlag.FinancialTransaction
            );

            _logger.LogInformation(
                "Audit: Journal Entry Posted - {BatchNumber} - {Description} - {Amount}",
                batchNumber, description, totalAmount);

            return await Task.FromResult(entry);
        }

        public async Task<AuditEntry> AuditBudgetVarianceAsync(
            Guid budgetId,
            string budgetName,
            Guid categoryId,
            string categoryName,
            decimal variance,
            bool isOverBudget,
            Guid? tenantId = null)
        {
            var entry = new AuditEntry(
                GuidGenerator.Create(),
                tenantId,
                new EntityReference("Budget", budgetId.ToString(), budgetName),
                AuditEventType.Update,
                new UserContext(_currentUser.Id ?? Guid.Empty, "System", "Internal"),
                $"Budget variance detected for {budgetName}/{categoryName}. Variance: {variance:F2}%",
                null,
                null,
                new AuditMetadata(new Dictionary<string, object>
                {
                    ["CategoryId"] = categoryId.ToString(),
                    ["CategoryName"] = categoryName,
                    ["VariancePercent"] = $"{variance:F2}%",
                    ["IsOverBudget"] = isOverBudget.ToString(),
                }),
                isOverBudget ? AuditSeverity.Critical : AuditSeverity.Medium,
                ComplianceFlag.FinancialTransaction
            );
            
            _logger.LogWarning(
                "Audit: Budget Variance - {BudgetName}/{CategoryName} - {Variance:F2}% - OverBudget: {IsOverBudget}",
                budgetName, categoryName, variance, isOverBudget);

            return await Task.FromResult(entry);
        }

        public async Task<AuditEntry> AuditDataAccessAsync(
            string entityType,
            Guid entityId,
            string accessType,
            Guid? tenantId = null)
        {
            var entry = new AuditEntry(
                GuidGenerator.Create(),
                tenantId,
                new EntityReference(entityType, entityId.ToString(), null),
                AuditEventType.Read,
                new UserContext(_currentUser.Id ?? Guid.Empty, _currentUser.UserName, GetClientIpAddress()),
                $"Accessed {entityType} with id {entityId}",
                null,
                null,
                new AuditMetadata(new Dictionary<string, object>
                {
                    ["AccessType"] = accessType,
                }),
                AuditSeverity.Low,
                ComplianceFlag.GDPR
            );

            return await Task.FromResult(entry);
        }

        public async Task<List<AuditEntry>> SearchAuditTrailAsync(
            DateTime startDate,
            DateTime endDate,
            string entityType = null,
            string actionType = null,
            Guid? userId = null,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Searching audit trail: {StartDate} to {EndDate}, EntityType={EntityType}, ActionType={ActionType}",
                startDate, endDate, entityType, actionType);

            // In production, query audit repository with filters
            return await Task.FromResult(new List<AuditEntry>());
        }

        private string GetClientIpAddress()
        {
            // In production, extract from HTTP context
            return "127.0.0.1";
        }
    }
}

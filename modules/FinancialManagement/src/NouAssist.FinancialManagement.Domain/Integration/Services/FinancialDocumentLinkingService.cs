using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.Integration.Services
{
    /// <summary>
    /// Domain service for linking documents to financial entities
    /// Provides a unified way to attach receipts, invoices, statements, etc.
    /// </summary>
    public class FinancialDocumentLinkingService : DomainService
    {
        private readonly ILogger<FinancialDocumentLinkingService> _logger;

        public FinancialDocumentLinkingService(
            ILogger<FinancialDocumentLinkingService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Link a document to a bill (e.g., vendor invoice, receipt)
        /// </summary>
        public async Task<DocumentLink> LinkDocumentToBillAsync(
            Guid billId,
            Guid documentId,
            string documentType,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Linking document {DocumentId} ({DocumentType}) to Bill {BillId}",
                documentId,
                documentType,
                billId);

            var link = new DocumentLink
            {
                EntityType = "Bill",
                EntityId = billId,
                DocumentId = documentId,
                DocumentType = documentType,
                LinkedAt = DateTime.UtcNow,
                TenantId = tenantId
            };

            // In production, persist this to a DocumentLink repository
            return await Task.FromResult(link);
        }

        /// <summary>
        /// Link a document to an expense (e.g., receipt)
        /// </summary>
        public async Task<DocumentLink> LinkDocumentToExpenseAsync(
            Guid expenseId,
            Guid documentId,
            string documentType,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Linking document {DocumentId} ({DocumentType}) to Expense {ExpenseId}",
                documentId,
                documentType,
                expenseId);

            var link = new DocumentLink
            {
                EntityType = "Expense",
                EntityId = expenseId,
                DocumentId = documentId,
                DocumentType = documentType,
                LinkedAt = DateTime.UtcNow,
                TenantId = tenantId
            };

            return await Task.FromResult(link);
        }

        /// <summary>
        /// Link a document to an invoice (e.g., invoice PDF, delivery proof)
        /// </summary>
        public async Task<DocumentLink> LinkDocumentToInvoiceAsync(
            Guid invoiceId,
            Guid documentId,
            string documentType,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Linking document {DocumentId} ({DocumentType}) to Invoice {InvoiceId}",
                documentId,
                documentType,
                invoiceId);

            var link = new DocumentLink
            {
                EntityType = "Invoice",
                EntityId = invoiceId,
                DocumentId = documentId,
                DocumentType = documentType,
                LinkedAt = DateTime.UtcNow,
                TenantId = tenantId
            };

            return await Task.FromResult(link);
        }

        /// <summary>
        /// Link a document to a tax return (e.g., W-2, 1099, receipts)
        /// </summary>
        public async Task<DocumentLink> LinkDocumentToTaxReturnAsync(
            Guid taxReturnId,
            Guid documentId,
            string documentType,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Linking document {DocumentId} ({DocumentType}) to Tax Return {TaxReturnId}",
                documentId,
                documentType,
                taxReturnId);

            var link = new DocumentLink
            {
                EntityType = "TaxReturn",
                EntityId = taxReturnId,
                DocumentId = documentId,
                DocumentType = documentType,
                LinkedAt = DateTime.UtcNow,
                TenantId = tenantId
            };

            return await Task.FromResult(link);
        }

        /// <summary>
        /// Link a document to an investment account (e.g., statements, trade confirmations)
        /// </summary>
        public async Task<DocumentLink> LinkDocumentToPortfolioAsync(
            Guid portfolioId,
            Guid documentId,
            string documentType,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Linking document {DocumentId} ({DocumentType}) to Portfolio {PortfolioId}",
                documentId,
                documentType,
                portfolioId);

            var link = new DocumentLink
            {
                EntityType = "Portfolio",
                EntityId = portfolioId,
                DocumentId = documentId,
                DocumentType = documentType,
                LinkedAt = DateTime.UtcNow,
                TenantId = tenantId
            };

            return await Task.FromResult(link);
        }

        /// <summary>
        /// Get all documents linked to an entity
        /// </summary>
        public async Task<List<DocumentLink>> GetDocumentsForEntityAsync(
            string entityType,
            Guid entityId,
            Guid? tenantId = null)
        {
            _logger.LogInformation(
                "Retrieving documents for {EntityType} {EntityId}",
                entityType,
                entityId);

            // In production, query DocumentLink repository
            return await Task.FromResult(new List<DocumentLink>());
        }

        /// <summary>
        /// Categorize document type for easier searching
        /// </summary>
        public string CategorizeDocumentType(string fileName)
        {
            var extension = System.IO.Path.GetExtension(fileName)?.ToLower();
            var lowerFileName = fileName.ToLower();

            if (lowerFileName.Contains("receipt"))
            {
                return "Receipt";
            }

            if (lowerFileName.Contains("invoice"))
            {
                return "Invoice";
            }

            if (lowerFileName.Contains("statement"))
            {
                return "Statement";
            }

            if (lowerFileName.Contains("w-2") || lowerFileName.Contains("w2"))
            {
                return "TaxForm-W2";
            }

            if (lowerFileName.Contains("1099"))
            {
                return "TaxForm-1099";
            }

            if (extension == ".pdf" && lowerFileName.Contains("contract"))
            {
                return "Contract";
            }

            return extension switch
            {
                ".pdf" => "PDF Document",
                ".jpg" or ".jpeg" or ".png" => "Image",
                ".xlsx" or ".xls" => "Spreadsheet",
                _ => "Other"
            };
        }
    }

    /// <summary>
    /// Represents a link between a financial entity and a document
    /// </summary>
    public class DocumentLink
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentType { get; set; }
        public DateTime LinkedAt { get; set; }
        public Guid? TenantId { get; set; }
        public string Notes { get; set; }
    }
}

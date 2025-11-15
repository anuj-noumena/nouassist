using System;
using NouAssist.AuditTrail.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.AuditTrail.Services
{
    public class ComplianceFlagDeterminationService : DomainService
    {
        public ComplianceFlag DetermineComplianceFlags(
            AuditEventType eventType,
            string entityType,
            object newValue)
        {
            var flags = ComplianceFlag.None;

            // Financial transaction monitoring
            if (IsFinancialEntity(entityType))
            {
                flags |= ComplianceFlag.FinancialTransaction;
                flags |= ComplianceFlag.SOX;
            }

            // Payment card data
            if (entityType.Contains("Payment", StringComparison.OrdinalIgnoreCase) ||
                entityType.Contains("Card", StringComparison.OrdinalIgnoreCase))
            {
                flags |= ComplianceFlag.PCI_DSS;
            }

            // Personal data
            if (entityType.Contains("Customer", StringComparison.OrdinalIgnoreCase) ||
                entityType.Contains("User", StringComparison.OrdinalIgnoreCase))
            {
                flags |= ComplianceFlag.GDPR;
            }

            // High-risk operations
            if (eventType == AuditEventType.Delete || eventType == AuditEventType.Post)
            {
                flags |= ComplianceFlag.HighRisk;
            }

            // Check for suspicious patterns
            if (IsSuspiciousActivity(eventType, entityType))
            {
                flags |= ComplianceFlag.Suspicious;
            }

            return flags;
        }

        private bool IsFinancialEntity(string entityType)
        {
            var financialEntities = new[] 
            { 
                "Bill", "Invoice", "Payment", "JournalEntry", 
                "BankTransaction", "Expense", "Budget", "Account" 
            };
            
            return Array.Exists(financialEntities, e => 
                entityType.Equals(e, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsSuspiciousActivity(AuditEventType eventType, string entityType)
        {
            // Multiple deletes or high-value operations could be suspicious
            // This is a simplified check - real implementation would use ML/pattern detection
            return eventType == AuditEventType.Delete && IsFinancialEntity(entityType);
        }
    }
}

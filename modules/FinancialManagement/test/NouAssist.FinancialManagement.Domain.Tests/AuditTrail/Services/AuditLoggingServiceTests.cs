using System;
using System.Collections.Generic;
using NouAssist.AuditTrail.Enums;
using NouAssist.AuditTrail.Services;
using Shouldly;
using Xunit;

namespace NouAssist.AuditTrail.Tests.Services
{
    public class AuditLoggingServiceTests
    {
        private readonly AuditLoggingService _auditLoggingService;

        public AuditLoggingServiceTests()
        {
            var complianceService = new ComplianceFlagDeterminationService();
            _auditLoggingService = new AuditLoggingService(complianceService);
        }

        [Fact]
        public void CreateAuditEntry_Should_CreateValidEntry()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            var entry = _auditLoggingService.CreateAuditEntry(
                tenantId,
                "Bill",
                Guid.NewGuid().ToString(),
                "Bill-001",
                AuditEventType.Create,
                userId,
                "john.doe@example.com",
                "Created new bill",
                null,
                new { Amount = 1000.00m, Vendor = "ACME Corp" },
                new Dictionary<string, object> { ["BillNumber"] = "BILL-001" },
                "192.168.1.1",
                "Mozilla/5.0",
                "session-123");

            // Assert
            entry.ShouldNotBeNull();
            entry.EntityReference.EntityType.ShouldBe("Bill");
            entry.EventType.ShouldBe(AuditEventType.Create);
            entry.UserContext.UserId.ShouldBe(userId);
            entry.Severity.ShouldBe(AuditSeverity.High); // Financial entity
            entry.ComplianceFlags.HasFlag(ComplianceFlag.FinancialTransaction).ShouldBeTrue();
            entry.ComplianceFlags.HasFlag(ComplianceFlag.SOX).ShouldBeTrue();
        }

        [Fact]
        public void CreateAuditEntry_ForNonFinancialEntity_Should_HaveLowerSeverity()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var entry = _auditLoggingService.CreateAuditEntry(
                null,
                "Document",
                Guid.NewGuid().ToString(),
                "Document-001",
                AuditEventType.Read,
                userId,
                "jane.doe@example.com",
                "Viewed document",
                null,
                null);

            // Assert
            entry.Severity.ShouldBe(AuditSeverity.Low);
            entry.ComplianceFlags.HasFlag(ComplianceFlag.FinancialTransaction).ShouldBeFalse();
        }

        [Fact]
        public void CreateAuditEntry_ForPayment_Should_IncludePCIDSSFlag()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var entry = _auditLoggingService.CreateAuditEntry(
                null,
                "Payment",
                Guid.NewGuid().ToString(),
                "Payment-001",
                AuditEventType.Create,
                userId,
                "user@example.com",
                "Created payment",
                null,
                new { CardNumber = "****1234" });

            // Assert
            entry.ComplianceFlags.HasFlag(ComplianceFlag.PCI_DSS).ShouldBeTrue();
        }

        [Fact]
        public void AuditEntry_RetentionPolicy_Should_BeSevenYears_ForSOXData()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var entry = _auditLoggingService.CreateAuditEntry(
                null,
                "JournalEntry",
                Guid.NewGuid().ToString(),
                "JE-001",
                AuditEventType.Post,
                userId,
                "accountant@example.com",
                "Posted journal entry",
                null,
                new { Amount = 5000.00m });

            // Assert
            entry.RetentionPolicy.RetentionDays.ShouldBe(2555); // 7 years
            entry.ComplianceFlags.HasFlag(ComplianceFlag.SOX).ShouldBeTrue();
        }
    }
}

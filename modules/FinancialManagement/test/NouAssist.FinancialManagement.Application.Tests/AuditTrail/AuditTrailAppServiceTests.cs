using System;
using System.Threading.Tasks;
using NouAssist.AuditTrail.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.AuditTrail
{
    public class AuditTrailAppServiceTests : AuditTrailApplicationTestBase
    {
        private readonly IAuditTrailAppService _auditTrailAppService;

        public AuditTrailAppServiceTests()
        {
            _auditTrailAppService = GetRequiredService<IAuditTrailAppService>();
        }

        [Fact]
        public async Task Should_Search_Audit_Entries()
        {
            // Arrange
            var input = new AuditSearchDto
            {
                StartDate = DateTime.Today.AddDays(-7),
                EndDate = DateTime.Today,
                MaxResultCount = 10
            };

            // Act
            var result = await _auditTrailAppService.SearchAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Generate_Audit_Report()
        {
            // Arrange
            var input = new AuditSearchDto
            {
                StartDate = DateTime.Today.AddMonths(-1),
                EndDate = DateTime.Today,
                MaxResultCount = 1000
            };

            // Act
            var report = await _auditTrailAppService.GenerateReportAsync(input);

            // Assert
            report.ShouldNotBeNull();
            report.Summary.ShouldNotBeNull();
            report.Entries.ShouldNotBeNull();
        }
    }
}

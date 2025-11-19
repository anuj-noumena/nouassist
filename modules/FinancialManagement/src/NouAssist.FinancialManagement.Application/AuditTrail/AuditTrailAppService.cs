using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using NouAssist.FinancialManagement.AuditTrail.Data;
using NouAssist.FinancialManagement.AuditTrail.Dtos;
using NouAssist.FinancialManagement.AuditTrail.Enums;
using NouAssist.FinancialManagement.AuditTrail.Permissions;
using NouAssist.FinancialManagement.AuditTrail.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AuditTrail
{
    [Authorize(AuditTrailPermissions.AuditEntries.View)]
    public class AuditTrailAppService : ApplicationService, IAuditTrailAppService
    {
        private readonly IAuditEntryRepository _auditEntryRepository;
        private readonly AuditArchiveService _auditArchiveService;

        public AuditTrailAppService(
            IAuditEntryRepository auditEntryRepository,
            AuditArchiveService auditArchiveService)
        {
            _auditEntryRepository = auditEntryRepository;
            _auditArchiveService = auditArchiveService;
        }

        public async Task<AuditEntryDto> GetAsync(Guid id)
        {
            var auditEntry = await _auditEntryRepository.GetAsync(id);
            return ObjectMapper.Map<AuditEntry, AuditEntryDto>(auditEntry);
        }

        public async Task<PagedResultDto<AuditEntryDto>> SearchAsync(AuditSearchDto input)
        {
            var criteria = new AuditSearchCriteria(input.StartDate, input.EndDate, CurrentTenant.Id)
            {
                EntityType = input.EntityType,
                EntityId = input.EntityId,
                UserId = input.UserId,
                EventType = input.EventType,
                MinimumSeverity = input.MinimumSeverity,
                ComplianceFlags = input.ComplianceFlags,
                IncludeArchived = input.IncludeArchived,
                MaxResults = input.MaxResultCount
            };

            var auditEntries = await _auditEntryRepository.SearchAsync(criteria);
            var totalCount = await _auditEntryRepository.GetCountAsync(criteria);

            return new PagedResultDto<AuditEntryDto>(
                totalCount,
                ObjectMapper.Map<List<AuditEntry>, List<AuditEntryDto>>(auditEntries));
        }

        public async Task<EntityAuditHistoryDto> GetEntityHistoryAsync(string entityType, string entityId)
        {
            var auditEntries = await _auditEntryRepository.GetByEntityAsync(entityType, entityId);

            return new EntityAuditHistoryDto
            {
                EntityType = entityType,
                EntityId = entityId,
                EntityName = auditEntries.FirstOrDefault()?.EntityReference.EntityName,
                History = ObjectMapper.Map<List<AuditEntry>, List<AuditEntryDto>>(auditEntries)
            };
        }

        [Authorize(AuditTrailPermissions.AuditEntries.GenerateReports)]
        public async Task<AuditTrailReportDto> GenerateReportAsync(AuditSearchDto input)
        {
            var criteria = new AuditSearchCriteria(input.StartDate, input.EndDate, CurrentTenant.Id)
            {
                EntityType = input.EntityType,
                UserId = input.UserId,
                EventType = input.EventType,
                MinimumSeverity = input.MinimumSeverity,
                ComplianceFlags = input.ComplianceFlags,
                MaxResults = 10000 // Higher limit for reports
            };

            var auditEntries = await _auditEntryRepository.SearchAsync(criteria);

            var summary = new AuditTrailSummaryDto
            {
                TotalEntries = auditEntries.Count,
                UniqueUsers = auditEntries.Select(e => e.UserContext.UserId).Distinct().Count(),
                EntitiesModified = auditEntries.Select(e => e.EntityReference.EntityId).Distinct().Count(),
                HighRiskOperations = auditEntries.Count(e => e.ComplianceFlags.HasFlag(ComplianceFlag.HighRisk)),
                SuspiciousActivity = auditEntries.Count(e => e.ComplianceFlags.HasFlag(ComplianceFlag.Suspicious))
            };

            return new AuditTrailReportDto
            {
                GeneratedAt = Clock.Now,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                Summary = summary,
                Entries = ObjectMapper.Map<List<AuditEntry>, List<AuditEntryDto>>(auditEntries)
            };
        }

        [Authorize(AuditTrailPermissions.AuditEntries.Archive)]
        public async Task<int> ArchiveOldEntriesAsync()
        {
            return await _auditArchiveService.ArchiveOldEntriesAsync("azure-archive-storage");
        }
    }
}

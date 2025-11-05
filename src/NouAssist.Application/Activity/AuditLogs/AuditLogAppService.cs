using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Activity.Aggregates;
using NouAssist.Activity.AuditLogs.Dtos;
using NouAssist.Activity.Data;
using NouAssist.Activity.Enums;
using NouAssist.Activity.Permissions;
using NouAssist.Activity.Specifications;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Activity.AuditLogs
{
    [Authorize(ActivityPermissions.AuditLogs.View)]
    public class AuditLogAppService : ApplicationService, IAuditLogAppService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogAppService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<AuditLogDto> GetAsync(Guid id)
        {
            var auditLog = await _auditLogRepository.GetAsync(id);
            return ObjectMapper.Map<AuditLog, AuditLogDto>(auditLog);
        }

        [Authorize(ActivityPermissions.AuditLogs.ViewAll)]
        public async Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogListDto input)
        {
            var spec = CreateListSpecification(input);
            
            var auditLogs = await _auditLogRepository.GetListAsync(spec, input.MaxResultCount);
            var totalCount = await _auditLogRepository.GetCountAsync(spec);

            return new PagedResultDto<AuditLogDto>(
                totalCount,
                ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(auditLogs)
            );
        }

        private ISpecification<AuditLog> CreateListSpecification(GetAuditLogListDto input)
        {
            var spec = Specification.True<AuditLog>();

            if (input.UserId.HasValue)
            {
                spec = spec.And(new Specification<AuditLog>(a => a.UserId == input.UserId.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                var category = Enum.Parse<AuditCategory>(input.Category);
                spec = spec.And(new AuditLogsByCategorySpecification(category));
            }

            if (!string.IsNullOrWhiteSpace(input.Severity))
            {
                var severity = Enum.Parse<AuditSeverity>(input.Severity);
                spec = spec.And(new Specification<AuditLog>(a => a.Severity == severity));
            }

            if (input.FromDate.HasValue && input.ToDate.HasValue)
            {
                spec = spec.And(new AuditLogsByDateRangeSpecification(input.FromDate.Value, input.ToDate.Value));
            }

            return spec;
        }
    }
}

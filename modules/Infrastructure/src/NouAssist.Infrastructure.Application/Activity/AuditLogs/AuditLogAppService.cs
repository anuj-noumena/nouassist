using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Activity.AuditLogs.Dtos;
using NouAssist.Infrastructure.Activity.Data;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace NouAssist.Infrastructure.Activity.AuditLogs
{
    public class AuditLogAppService : ApplicationService, IAuditLogAppService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IAsyncQueryableExecuter _asyncExecuter;

        public AuditLogAppService(
            IAuditLogRepository auditLogRepository,
            IAsyncQueryableExecuter asyncExecuter)
        {
            _auditLogRepository = auditLogRepository;
            _asyncExecuter = asyncExecuter;
        }

        public async Task<AuditLogDto> GetAsync(Guid id)
        {
            var auditLog = await _auditLogRepository.GetAsync(id);
            return ObjectMapper.Map<AuditLog, AuditLogDto>(auditLog);
        }

        public async Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(AuditLog.Timestamp) + " DESC";
            }

            var queryable = await _auditLogRepository.GetQueryableAsync();

            var query = queryable
                .WhereIf(input.UserId.HasValue, x => x.UserId == input.UserId.Value)
                .WhereIf(!input.Category.IsNullOrWhiteSpace(), x => x.Category.ToString() == input.Category)
                .WhereIf(!input.Severity.IsNullOrWhiteSpace(), x => x.Severity.ToString() == input.Severity)
                .WhereIf(input.FromDate.HasValue, x => x.Timestamp >= input.FromDate.Value)
                .WhereIf(input.ToDate.HasValue, x => x.Timestamp <= input.ToDate.Value);

            var totalCount = await _asyncExecuter.CountAsync(query);

            var items = await _asyncExecuter.ToListAsync(
                query.PageBy(input)
            );

            return new PagedResultDto<AuditLogDto>(
                totalCount,
                ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(items)
            );
        }
    }
}

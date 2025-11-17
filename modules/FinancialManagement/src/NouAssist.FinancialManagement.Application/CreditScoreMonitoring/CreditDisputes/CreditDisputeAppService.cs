using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.CreditScoreMonitoring.CreditDisputes.Dtos;
using NouAssist.CreditScoreMonitoring.Enums;
using NouAssist.CreditScoreMonitoring.Permissions;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Data;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CreditScoreMonitoring.CreditDisputes
{
    [Authorize(CreditScoreMonitoringPermissions.CreditDisputes.Default)]
    public class CreditDisputeAppService : ApplicationService, ICreditDisputeAppService
    {
        private readonly ICreditDisputeRepository _creditDisputeRepository;

        public CreditDisputeAppService(ICreditDisputeRepository creditDisputeRepository)
        {
            _creditDisputeRepository = creditDisputeRepository;
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditDisputes.View)]
        public async Task<CreditDisputeDto> GetAsync(Guid id)
        {
            var dispute = await _creditDisputeRepository.GetAsync(id);
            return ObjectMapper.Map<CreditDispute, CreditDisputeDto>(dispute);
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditDisputes.View)]
        public async Task<PagedResultDto<CreditDisputeDto>> GetListAsync(GetCreditDisputeListDto input)
        {
            var userId = CurrentUser.GetId();
            var disputes = await _creditDisputeRepository.GetUserDisputesAsync(userId);

            if (input.Status.HasValue)
            {
                disputes = disputes.Where(d => d.Status == input.Status.Value).ToList();
            }

            if (input.Bureau.HasValue)
            {
                disputes = disputes.Where(d => d.Bureau == input.Bureau.Value).ToList();
            }

            var totalCount = disputes.Count;
            var pagedDisputes = disputes
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<CreditDisputeDto>(
                totalCount,
                ObjectMapper.Map<List<CreditDispute>, List<CreditDisputeDto>>(pagedDisputes)
            );
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditDisputes.Create)]
        public async Task<CreditDisputeDto> CreateAsync(CreateCreditDisputeDto input)
        {
            var userId = CurrentUser.GetId();

            var dispute = new CreditDispute(
                GuidGenerator.Create(),
                userId,
                CurrentTenant.Id,
                input.Bureau,
                input.ItemType,
                input.ItemDescription,
                input.DisputeReason,
                input.SupportingDocumentation
            );

            await _creditDisputeRepository.InsertAsync(dispute);

            return ObjectMapper.Map<CreditDispute, CreditDisputeDto>(dispute);
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditDisputes.Update)]
        public async Task<CreditDisputeDto> UpdateStatusAsync(Guid id, UpdateDisputeStatusDto input)
        {
            var dispute = await _creditDisputeRepository.GetAsync(id);

            switch (input.Status)
            {
                case DisputeStatus.UnderInvestigation:
                    dispute.MarkAsUnderInvestigation(Guid.NewGuid().ToString());
                    break;
                case DisputeStatus.Resolved:
                    dispute.Resolve(input.ResolutionNotes);
                    break;
                case DisputeStatus.Rejected:
                    dispute.Reject(input.ResolutionNotes);
                    break;
            }

            await _creditDisputeRepository.UpdateAsync(dispute);

            return ObjectMapper.Map<CreditDispute, CreditDisputeDto>(dispute);
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditDisputes.Cancel)]
        public async Task CancelAsync(Guid id)
        {
            var dispute = await _creditDisputeRepository.GetAsync(id);
            dispute.Cancel();
            await _creditDisputeRepository.UpdateAsync(dispute);
        }
    }
}

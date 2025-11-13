using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    public interface ICollectionCaseAppService : IApplicationService
    {
        Task<CollectionCaseDto> GetAsync(Guid id);
        Task<PagedResultDto<CollectionCaseDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<CollectionCaseDto> CreateAsync(CreateCollectionCaseDto input);
        Task<CollectionCaseDto> AddActivityAsync(Guid caseId, AddCollectionActivityDto input);
        Task<CollectionCaseDto> UpdateStatusAsync(Guid caseId, UpdateCollectionStatusDto input);
        Task<CollectionCaseDto> AssignAsync(Guid caseId, Guid assigneeId);
    }
}

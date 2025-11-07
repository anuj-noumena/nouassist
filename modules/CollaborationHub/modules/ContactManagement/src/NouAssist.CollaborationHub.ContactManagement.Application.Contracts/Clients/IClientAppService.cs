using System;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.ContactManagement.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.ContactManagement.Clients
{
    public interface IClientAppService : IApplicationService
    {
        Task<ClientDto> GetAsync(Guid id);
        Task<PagedResultDto<ClientDto>> GetListAsync(GetClientListDto input);
        Task<ClientDto> CreateAsync(CreateClientDto input);
        Task<ClientDto> UpdateAsync(Guid id, UpdateClientDto input);
        Task DeleteAsync(Guid id);

        Task<ClientDto> ActivateAsync(Guid id);
        Task<ClientDto> DeactivateAsync(Guid id, string reason = null);
        Task<ClientDto> PutOnHoldAsync(Guid id, string reason = null);
        Task<ClientDto> MarkAsLostAsync(Guid id, string reason = null);
        Task<ClientDto> ConvertToPayingAsync(Guid id);

        Task<ClientDto> RecordContactAsync(Guid id);
        Task<ClientDto> UpdateBalanceAsync(Guid id, decimal amount);

        Task<ClientDto> AddTagAsync(Guid id, string tag);
    }
}

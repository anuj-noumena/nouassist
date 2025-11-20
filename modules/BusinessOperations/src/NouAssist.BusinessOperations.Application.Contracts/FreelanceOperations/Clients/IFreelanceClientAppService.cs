using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Clients.Dtos;
using NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.FreelanceOperations.Clients
{
    public interface IFreelanceClientAppService : IApplicationService
    {
        // Client CRUD
        Task<FreelanceClientDto> GetAsync(Guid id);
        Task<PagedResultDto<FreelanceClientDto>> GetListAsync(GetFreelanceClientListDto input);
        Task<FreelanceClientDto> CreateAsync(CreateFreelanceClientDto input);
        Task<FreelanceClientDto> UpdateAsync(Guid id, UpdateFreelanceClientDto input);
        Task DeleteAsync(Guid id);
        
        // Client specific operations
        Task<FreelanceClientDto> ChangeStatusAsync(Guid id, ChangeClientStatusDto input);
        Task<List<FreelanceProjectDto>> GetClientProjectsAsync(Guid id);
        
        // Communication records
        Task<ClientCommunicationRecordDto> GetCommunicationRecordAsync(Guid id);
        Task<List<ClientCommunicationRecordDto>> GetCommunicationRecordsAsync(Guid clientId);
        Task<ClientCommunicationRecordDto> AddCommunicationRecordAsync(Guid clientId, CreateCommunicationRecordDto input);
        Task<ClientCommunicationRecordDto> UpdateCommunicationFollowUpAsync(Guid id, UpdateCommunicationFollowUpDto input);
        Task MarkCommunicationFollowUpCompletedAsync(Guid id);
        Task DeleteCommunicationRecordAsync(Guid id);
        
        // Follow-ups
        Task<List<ClientCommunicationRecordDto>> GetPendingFollowUpsAsync(Guid clientId);
        Task<List<FreelanceClientDto>> GetClientsWithPendingFollowUpsAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects
{
    public interface IFreelanceProjectAppService : IApplicationService
    {
        // Project CRUD
        Task<FreelanceProjectDto> GetAsync(Guid id);
        Task<PagedResultDto<FreelanceProjectDto>> GetListAsync(GetFreelanceProjectListDto input);
        Task<FreelanceProjectDto> CreateAsync(CreateFreelanceProjectDto input);
        Task<FreelanceProjectDto> UpdateAsync(Guid id, UpdateFreelanceProjectDto input);
        Task DeleteAsync(Guid id);
        
        // Project specific operations
        Task<FreelanceProjectDto> ChangeStatusAsync(Guid id, ChangeProjectStatusDto input);
        Task<FreelanceProjectDto> LinkToProposalAsync(Guid id, Guid proposalId);
        Task<decimal> CalculateTotalAmountAsync(Guid id);
        
        // Time entries
        Task<TimeEntryDto> GetTimeEntryAsync(Guid projectId, DateTime date, TimeSpan startTime);
        Task<List<TimeEntryDto>> GetTimeEntriesAsync(Guid projectId);
        Task<TimeEntryDto> AddTimeEntryAsync(Guid projectId, CreateTimeEntryDto input);
        Task<TimeEntryDto> UpdateTimeEntryAsync(Guid projectId, DateTime date, TimeSpan startTime, UpdateTimeEntryDto input);
        Task DeleteTimeEntryAsync(Guid projectId, DateTime date, TimeSpan startTime);
        
        // Milestones
        Task<MilestoneDto> GetMilestoneAsync(Guid id);
        Task<List<MilestoneDto>> GetMilestonesAsync(Guid projectId);
        Task<MilestoneDto> AddMilestoneAsync(Guid projectId, CreateMilestoneDto input);
        Task<MilestoneDto> UpdateMilestoneAsync(Guid id, UpdateMilestoneDto input);
        Task<MilestoneDto> CompleteMilestoneAsync(Guid id);
        Task<MilestoneDto> ReopenMilestoneAsync(Guid id);
        Task DeleteMilestoneAsync(Guid id);
    }
}

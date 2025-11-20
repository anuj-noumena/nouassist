using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Reminders.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.FreelanceOperations.Reminders
{
    public interface IReminderAppService : IApplicationService
    {
        Task<PagedResultDto<ReminderDto>> GetListAsync(GetReminderListDto input);
        Task<ReminderDto> GetAsync(Guid id);
        Task MarkAsCompletedAsync(Guid id);
    }
}

using System;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Planning.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment.Habits.Planning
{
    public interface IPlanningAppService : IApplicationService
    {
        Task<DailyPlanDto> GetDailyPlanAsync(GetDailyPlanDto input);
        
        Task<DailyReflectionDto> GetReflectionAsync(DateTime date);
        Task<DailyReflectionDto> UpdateReflectionAsync(UpdateDailyReflectionDto input);
        
        Task<DailyIntentionDto> GetIntentionAsync(DateTime date);
        Task<DailyIntentionDto> UpdateIntentionAsync(UpdateDailyIntentionDto input);
        
        Task<DailyQuoteDto> GetQuoteAsync(DateTime date);
        Task<DailyQuoteDto> MarkQuoteAsViewedAsync(Guid id);
    }
}

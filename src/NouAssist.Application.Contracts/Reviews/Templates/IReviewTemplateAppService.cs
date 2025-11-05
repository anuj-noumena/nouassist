using System;
using System.Threading.Tasks;
using NouAssist.Reviews.Enums;
using NouAssist.Reviews.Templates.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Reviews.Templates
{
    public interface IReviewTemplateAppService : IApplicationService
    {
        Task<ReviewTemplateDto> GetAsync(Guid id);
        Task<PagedResultDto<ReviewTemplateDto>> GetListAsync(GetTemplateListDto input);
        Task<ReviewTemplateDto> CreateAsync(CreateReviewTemplateDto input);
        Task<ReviewTemplateDto> UpdateAsync(Guid id, UpdateReviewTemplateDto input);
        Task DeleteAsync(Guid id);

        Task<ReviewTemplateDto> GetByReviewTypeAsync(ReviewType reviewType);
        Task<ListResultDto<ReviewTemplateDto>> GetSystemTemplatesAsync();
        Task<ListResultDto<ReviewTemplateDto>> GetCustomTemplatesAsync();

        Task<ReviewTemplateDto> AddChecklistStepAsync(Guid id, AddChecklistStepDto input);
        Task<ReviewTemplateDto> RemoveChecklistStepAsync(Guid id, int stepNumber);
        Task<ReviewTemplateDto> AddReflectionPromptAsync(Guid id, string prompt);
        Task<ReviewTemplateDto> RemoveReflectionPromptAsync(Guid id, string prompt);
    }
}

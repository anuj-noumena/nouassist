using System;
using System.Threading.Tasks;
using NouAssist.Learning.LearningNotes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Learning.LearningNotes
{
    public interface ILearningNoteAppService : IApplicationService
    {
        Task<LearningNoteDto> GetAsync(Guid id);
        Task<PagedResultDto<LearningNoteDto>> GetListAsync(GetLearningNoteListDto input);
        Task<LearningNoteDto> CreateAsync(CreateLearningNoteDto input);
        Task<LearningNoteDto> UpdateAsync(Guid id, UpdateLearningNoteDto input);
        Task DeleteAsync(Guid id);
    }
}

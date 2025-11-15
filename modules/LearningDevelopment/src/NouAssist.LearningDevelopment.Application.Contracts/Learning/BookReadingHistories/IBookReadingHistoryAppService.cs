using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Learning.BookReadingHistories.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Learning.BookReadingHistories
{
    public interface IBookReadingHistoryAppService : IApplicationService
    {
        Task<BookReadingHistoryDto> GetAsync(Guid id);
        Task<List<BookReadingHistoryDto>> GetUserHistoriesAsync();
        Task<List<BookReadingHistoryDto>> GetByTopicIdAsync(Guid topicId);
        Task<BookReadingHistoryDto> CreateAsync(CreateBookReadingHistoryDto input);
        Task<BookReadingHistoryDto> UpdateProgressAsync(Guid id, UpdateBookProgressDto input);
        Task DeleteAsync(Guid id);
    }
}

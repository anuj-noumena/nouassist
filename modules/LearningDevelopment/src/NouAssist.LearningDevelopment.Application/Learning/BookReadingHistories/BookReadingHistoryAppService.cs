using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.BookReadingHistories.Dtos;
using NouAssist.Learning.Data;
using NouAssist.Learning.Permissions;
using Volo.Abp.Application.Services;

namespace NouAssist.Learning.BookReadingHistories
{
    [Authorize(LearningPermissions.BookReadingHistories.Default)]
    public class BookReadingHistoryAppService : ApplicationService, IBookReadingHistoryAppService
    {
        private readonly IBookReadingHistoryRepository _historyRepository;

        public BookReadingHistoryAppService(IBookReadingHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<BookReadingHistoryDto> GetAsync(Guid id)
        {
            var history = await _historyRepository.GetAsync(id);
            return ObjectMapper.Map<BookReadingHistory, BookReadingHistoryDto>(history);
        }

        public async Task<List<BookReadingHistoryDto>> GetUserHistoriesAsync()
        {
            var histories = await _historyRepository.GetByUserIdAsync(CurrentUser.GetId());
            return ObjectMapper.Map<List<BookReadingHistory>, List<BookReadingHistoryDto>>(histories);
        }

        public async Task<List<BookReadingHistoryDto>> GetByTopicIdAsync(Guid topicId)
        {
            var histories = await _historyRepository.GetByTopicIdAsync(topicId);
            return ObjectMapper.Map<List<BookReadingHistory>, List<BookReadingHistoryDto>>(histories);
        }

        [Authorize(LearningPermissions.BookReadingHistories.Create)]
        public async Task<BookReadingHistoryDto> CreateAsync(CreateBookReadingHistoryDto input)
        {
            // Check if history already exists for this book
            var existing = await _historyRepository.GetByBookIdAsync(
                CurrentUser.GetId(), input.BookId);

            if (existing != null)
            {
                throw new Volo.Abp.BusinessException("Learning:BookHistoryAlreadyExists",
                    "A reading history already exists for this book.");
            }

            var history = new BookReadingHistory(
                GuidGenerator.Create(),
                CurrentUser.GetId(),
                input.BookId,
                input.Title,
                input.TopicId
            );

            history.UpdateBookDetails(input.Title, input.Author, input.CoverImageUrl);

            await _historyRepository.InsertAsync(history);
            return ObjectMapper.Map<BookReadingHistory, BookReadingHistoryDto>(history);
        }

        [Authorize(LearningPermissions.BookReadingHistories.Update)]
        public async Task<BookReadingHistoryDto> UpdateProgressAsync(
            Guid id, UpdateBookProgressDto input)
        {
            var history = await _historyRepository.GetAsync(id);
            history.UpdateProgress(input.LastReadPage, input.TotalPages);
            await _historyRepository.UpdateAsync(history);
            return ObjectMapper.Map<BookReadingHistory, BookReadingHistoryDto>(history);
        }

        [Authorize(LearningPermissions.BookReadingHistories.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _historyRepository.DeleteAsync(id);
        }
    }
}

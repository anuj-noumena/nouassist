using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Reviews.Progress.Dtos;
using NouAssist.WorkManagement.Reviews.Reviews.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Reviews.Reviews
{
    public interface IReviewAppService : IApplicationService
    {
        Task<ReviewDto> GetAsync(Guid id);
        Task<PagedResultDto<ReviewDto>> GetListAsync(GetReviewListDto input);
        Task<ReviewDto> CreateAsync(CreateReviewDto input);
        Task DeleteAsync(Guid id);

        Task<ReviewDto> UpdateReflectionsAsync(Guid id, UpdateReviewReflectionsDto input);
        Task<ReviewDto> SetScoresAsync(Guid id, SetReviewScoresDto input);
        Task<ReviewDto> CompleteAsync(Guid id, CompleteReviewDto input);
        Task<ReviewDto> RecordTimeSpentAsync(Guid id, RecordTimeSpentDto input);

        Task<ReviewDto> AddActionItemAsync(Guid id, AddReviewActionItemDto input);
        Task<ReviewDto> CompleteActionItemAsync(Guid reviewId, int actionItemIndex);

        Task<ReviewDto> AddReviewItemAsync(Guid id, AddReviewItemDto input);
        Task<ReviewItemDto> ReviewItemAsync(Guid reviewId, Guid itemId, ReviewItemDecisionDto input);
        Task<PagedResultDto<ReviewItemDto>> GetReviewItemsAsync(Guid reviewId, bool? onlyUnreviewed = null);

        Task<ReviewProgressDto> GetProgressAsync(Guid reviewId);
        Task<ReviewQualityAnalysisDto> GetQualityAnalysisAsync(Guid id);
    }
}

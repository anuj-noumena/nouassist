using System;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.DiscussionThreads.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.DiscussionThreads.Comments
{
    public interface ICommentAppService : IApplicationService
    {
        Task<CommentDto> GetAsync(Guid id);
        Task<PagedResultDto<CommentDto>> GetListAsync(GetCommentListDto input);
        Task<CommentDto> CreateAsync(CreateCommentDto input);
        Task<CommentDto> UpdateAsync(Guid id, UpdateCommentDto input);
        Task DeleteAsync(Guid id);

        Task<CommentDto> PinAsync(Guid id);
        Task<CommentDto> UnpinAsync(Guid id);

        Task<CommentDto> AddReactionAsync(Guid id, AddReactionDto input);
        Task<CommentDto> RemoveReactionAsync(Guid id, string emoji);

        Task<PagedResultDto<CommentDto>> GetRepliesAsync(Guid parentCommentId, PagedAndSortedResultRequestDto input);
    }
}

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions
{
    public interface IDiscussionBoardAppService : IApplicationService
    {
        Task<DiscussionBoardDto> GetAsync(Guid id);
        Task<PagedResultDto<DiscussionBoardDto>> GetListAsync(GetDiscussionBoardListDto input);
        Task<DiscussionBoardDto> GetOrCreateForResourceAsync(TargetResourceType resourceType, Guid resourceId);
        
        Task<PagedResultDto<DiscussionThreadDto>> GetThreadsAsync(GetDiscussionThreadListDto input);
        Task<DiscussionThreadDto> GetThreadAsync(Guid threadId);
        
        Task<DiscussionThreadDto> CreateThreadAsync(CreateDiscussionThreadDto input);
        Task CloseThreadAsync(Guid threadId);
        Task ReopenThreadAsync(Guid threadId);
        
        Task<CommentDto> AddCommentAsync(AddCommentDto input);
        Task<CommentDto> UpdateCommentAsync(Guid commentId, UpdateCommentDto input);
    }
}

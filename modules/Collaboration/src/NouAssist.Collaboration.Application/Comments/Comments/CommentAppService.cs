using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Collaboration.Comments.Aggregates;
using NouAssist.Collaboration.Comments.Comments.Dtos;
using NouAssist.Collaboration.Comments.Data;
using NouAssist.Collaboration.Comments.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Collaboration.Comments.Comments
{
    [Authorize(CommentsPermissions.Comments.Default)]
    public class CommentAppService : ApplicationService, ICommentAppService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentAppService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDto> GetAsync(Guid id)
        {
            var comment = await _commentRepository.GetAsync(id);
            return await MapToGetOutputDtoAsync(comment);
        }

        public async Task<PagedResultDto<CommentDto>> GetListAsync(GetCommentListDto input)
        {
            var queryable = await _commentRepository.GetQueryableAsync();

            // Apply filters
            if (input.EntityType.HasValue)
            {
                queryable = queryable.Where(c => c.EntityType == input.EntityType.Value);
            }

            if (input.EntityId.HasValue)
            {
                queryable = queryable.Where(c => c.EntityId == input.EntityId.Value);
            }

            if (input.ParentCommentId.HasValue)
            {
                queryable = queryable.Where(c => c.ParentCommentId == input.ParentCommentId.Value);
            }

            if (input.AuthorId.HasValue)
            {
                queryable = queryable.Where(c => c.AuthorId == input.AuthorId.Value);
            }

            if (input.IsPinned.HasValue)
            {
                queryable = queryable.Where(c => c.IsPinned == input.IsPinned.Value);
            }

            if (input.OnlyTopLevel == true)
            {
                queryable = queryable.Where(c => c.ParentCommentId == null);
            }

            var totalCount = queryable.Count();

            // Apply sorting
            queryable = !string.IsNullOrWhiteSpace(input.Sorting)
                ? queryable.OrderBy(input.Sorting)
                : queryable.OrderByDescending(c => c.CreationTime);

            // Apply paging
            var comments = queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var commentDtos = new List<CommentDto>();
            foreach (var comment in comments)
            {
                commentDtos.Add(await MapToGetOutputDtoAsync(comment));
            }

            return new PagedResultDto<CommentDto>(totalCount, commentDtos);
        }

        [Authorize(CommentsPermissions.Comments.Create)]
        public async Task<CommentDto> CreateAsync(CreateCommentDto input)
        {
            var authorId = CurrentUser.GetId();

            var comment = new Comment(
                GuidGenerator.Create(),
                authorId,
                input.EntityType,
                input.EntityId,
                input.Content,
                CurrentTenant.Id,
                input.ParentCommentId,
                input.MentionedUserIds
            );

            // If this is a reply, increment the parent's reply count
            if (input.ParentCommentId.HasValue)
            {
                var parentComment = await _commentRepository.GetAsync(input.ParentCommentId.Value);
                parentComment.IncrementRepliesCount();
                await _commentRepository.UpdateAsync(parentComment);
            }

            await _commentRepository.InsertAsync(comment);

            return await MapToGetOutputDtoAsync(comment);
        }

        [Authorize(CommentsPermissions.Comments.Edit)]
        public async Task<CommentDto> UpdateAsync(Guid id, UpdateCommentDto input)
        {
            var comment = await _commentRepository.GetAsync(id);

            if (comment.AuthorId != CurrentUser.GetId())
            {
                throw new BusinessException("Comments:NotAuthor", "You don't have permission to edit this comment");
            }

            comment.Edit(input.Content);
            await _commentRepository.UpdateAsync(comment);

            return await MapToGetOutputDtoAsync(comment);
        }

        [Authorize(CommentsPermissions.Comments.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var comment = await _commentRepository.GetAsync(id);

            if (comment.AuthorId != CurrentUser.GetId())
            {
                throw new BusinessException("Comments:NotAuthor", "You don't have permission to delete this comment");
            }

            comment.Delete();
            await _commentRepository.UpdateAsync(comment);
        }

        [Authorize(CommentsPermissions.Comments.Pin)]
        public async Task<CommentDto> PinAsync(Guid id)
        {
            var comment = await _commentRepository.GetAsync(id);
            comment.Pin();
            await _commentRepository.UpdateAsync(comment);

            return await MapToGetOutputDtoAsync(comment);
        }

        [Authorize(CommentsPermissions.Comments.Pin)]
        public async Task<CommentDto> UnpinAsync(Guid id)
        {
            var comment = await _commentRepository.GetAsync(id);
            comment.Unpin();
            await _commentRepository.UpdateAsync(comment);

            return await MapToGetOutputDtoAsync(comment);
        }

        [Authorize(CommentsPermissions.Comments.React)]
        public async Task<CommentDto> AddReactionAsync(Guid id, AddReactionDto input)
        {
            var comment = await _commentRepository.GetAsync(id);
            var userId = CurrentUser.GetId();

            comment.AddReaction(userId, input.Emoji);
            await _commentRepository.UpdateAsync(comment);

            return await MapToGetOutputDtoAsync(comment);
        }

        [Authorize(CommentsPermissions.Comments.React)]
        public async Task<CommentDto> RemoveReactionAsync(Guid id, string emoji)
        {
            var comment = await _commentRepository.GetAsync(id);
            var userId = CurrentUser.GetId();

            comment.RemoveReaction(userId, emoji);
            await _commentRepository.UpdateAsync(comment);

            return await MapToGetOutputDtoAsync(comment);
        }

        public async Task<PagedResultDto<CommentDto>> GetRepliesAsync(Guid parentCommentId, PagedAndSortedResultRequestDto input)
        {
            var replies = await _commentRepository.GetRepliesAsync(parentCommentId);
            var totalCount = replies.Count;

            var pagedReplies = replies
                .OrderBy(r => r.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var replyDtos = new List<CommentDto>();
            foreach (var reply in pagedReplies)
            {
                replyDtos.Add(await MapToGetOutputDtoAsync(reply));
            }

            return new PagedResultDto<CommentDto>(totalCount, replyDtos);
        }

        // Private helper methods
        private async Task<CommentDto> MapToGetOutputDtoAsync(Comment comment)
        {
            var dto = ObjectMapper.Map<Comment, CommentDto>(comment);
            // TODO: Load author name from user service
            dto.AuthorName = "User"; // Placeholder
            return dto;
        }
    }
}

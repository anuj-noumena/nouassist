using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Exceptions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Integration;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Permissions;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Services;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions
{
    [Authorize(CollaborationPermissions.Discussions.Default)]
    public class DiscussionBoardAppService : ApplicationService, IDiscussionBoardAppService
    {
        private readonly IDiscussionBoardRepository _discussionBoardRepository;
        private readonly IDiscussionPersistenceService _discussionPersistenceService;
        private readonly IIdentityUserLookupService _userLookupService;
        private readonly IResourceDisplayNameProvider _resourceDisplayNameProvider;

        public DiscussionBoardAppService(
            IDiscussionBoardRepository discussionBoardRepository,
            IDiscussionPersistenceService discussionPersistenceService,
            IIdentityUserLookupService userLookupService,
            IResourceDisplayNameProvider resourceDisplayNameProvider)
        {
            _discussionBoardRepository = discussionBoardRepository;
            _discussionPersistenceService = discussionPersistenceService;
            _userLookupService = userLookupService;
            _resourceDisplayNameProvider = resourceDisplayNameProvider;
        }

        public virtual async Task<DiscussionBoardDto> GetAsync(Guid id)
        {
            try
            {
                var board = await _discussionBoardRepository.GetAsync(id);
                return await EnrichDiscussionBoardDtoAsync(board);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotFindDiscussionBoard"]);
            }
        }

        public virtual async Task<PagedResultDto<DiscussionBoardDto>> GetListAsync(GetDiscussionBoardListDto input)
        {
            try
            {
                var query = await _discussionBoardRepository.GetQueryableAsync();
                
                // Apply filters
                if (input.ResourceType.HasValue)
                {
                    query = query.Where(d => EF.Property<string>(d, "Resource_Type") == input.ResourceType.ToString());
                }
                
                if (input.OnlyActive.HasValue && input.OnlyActive.Value)
                {
                    query = query.Where(d => d.IsActive);
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting and paging
                query = query.OrderBy(input.Sorting ?? "CreationTime desc")
                    .PageBy(input);
                
                // Execute query and map to DTOs
                var boards = await AsyncExecuter.ToListAsync(query);
                var boardDtos = new List<DiscussionBoardDto>();
                
                foreach (var board in boards)
                {
                    boardDtos.Add(await EnrichDiscussionBoardDtoAsync(board));
                }
                
                return new PagedResultDto<DiscussionBoardDto>(totalCount, boardDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotListDiscussionBoards"]);
            }
        }

        public virtual async Task<DiscussionBoardDto> GetOrCreateForResourceAsync(TargetResourceType resourceType, Guid resourceId)
        {
            try
            {
                var currentUserId = CurrentUser.GetId();
                var board = await _discussionPersistenceService.GetOrCreateDiscussionBoardAsync(
                    resourceType,
                    resourceId,
                    currentUserId);
                    
                return await EnrichDiscussionBoardDtoAsync(board);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is PermissionDeniedException)
                {
                    throw new UserFriendlyException(L["Error:PermissionDenied"]);
                }
                throw new UserFriendlyException(L["Error:CouldNotAccessDiscussionBoard"]);
            }
        }

        public virtual async Task<PagedResultDto<DiscussionThreadDto>> GetThreadsAsync(GetDiscussionThreadListDto input)
        {
            try
            {
                var board = await _discussionBoardRepository.GetAsync(input.DiscussionBoardId);
                var threads = board.Threads.AsQueryable();
                
                // Apply filters
                if (input.OnlyOpen.HasValue && input.OnlyOpen.Value)
                {
                    threads = threads.Where(t => !t.IsClosed);
                }
                
                // Get total count
                var totalCount = threads.Count();
                
                // Apply sorting and paging
                threads = threads.OrderBy(input.Sorting ?? "CreationTime desc")
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                
                // Execute query and map to DTOs
                var threadDtos = new List<DiscussionThreadDto>();
                foreach (var thread in threads)
                {
                    threadDtos.Add(await EnrichDiscussionThreadDtoAsync(thread));
                }
                
                return new PagedResultDto<DiscussionThreadDto>(totalCount, threadDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotListDiscussionThreads"]);
            }
        }

        public virtual async Task<DiscussionThreadDto> GetThreadAsync(Guid threadId)
        {
            try
            {
                var board = await _discussionBoardRepository.WithDetailsAsync();
                board = await AsyncExecuter.FirstOrDefaultAsync(
                    board.Where(b => b.Threads.Any(t => t.Id == threadId)));
                    
                if (board == null)
                {
                    throw new EntityNotFoundException(typeof(DiscussionThread), threadId);
                }
                
                var thread = board.GetThread(threadId);
                return await EnrichDiscussionThreadDtoAsync(thread);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotFindDiscussionThread"]);
            }
        }

        [Authorize(CollaborationPermissions.Discussions.CreateThreads)]
        public virtual async Task<DiscussionThreadDto> CreateThreadAsync(CreateDiscussionThreadDto input)
        {
            try
            {
                var currentUserId = CurrentUser.GetId();
                var thread = await _discussionPersistenceService.CreateThreadAsync(
                    input.DiscussionBoardId,
                    input.Subject,
                    input.InitialComment,
                    currentUserId);
                    
                return await EnrichDiscussionThreadDtoAsync(thread);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is PermissionDeniedException)
                {
                    throw new UserFriendlyException(L["Error:PermissionDenied"]);
                }
                if (ex is CollaborationDomainException domainException && 
                    domainException.Code == "Collaboration:ErrorMessage:InvalidOperation")
                {
                    throw new UserFriendlyException(L["Error:CannotCreateThreadInInactiveBoard"]);
                }
                throw new UserFriendlyException(L["Error:CouldNotCreateThread"]);
            }
        }

        [Authorize(CollaborationPermissions.Discussions.ManageThreads)]
        public virtual async Task CloseThreadAsync(Guid threadId)
        {
            try
            {
                // Find the board containing this thread
                var board = await _discussionBoardRepository.WithDetailsAsync();
                board = await AsyncExecuter.FirstOrDefaultAsync(
                    board.Where(b => b.Threads.Any(t => t.Id == threadId)));
                    
                if (board == null)
                {
                    throw new EntityNotFoundException(typeof(DiscussionThread), threadId);
                }
                
                var currentUserId = CurrentUser.GetId();
                board.CloseThread(threadId, currentUserId, Clock.Now);
                
                await _discussionBoardRepository.UpdateAsync(board);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotCloseThread"]);
            }
        }

        [Authorize(CollaborationPermissions.Discussions.ManageThreads)]
        public virtual async Task ReopenThreadAsync(Guid threadId)
        {
            try
            {
                // Find the board containing this thread
                var board = await _discussionBoardRepository.WithDetailsAsync();
                board = await AsyncExecuter.FirstOrDefaultAsync(
                    board.Where(b => b.Threads.Any(t => t.Id == threadId)));
                    
                if (board == null)
                {
                    throw new EntityNotFoundException(typeof(DiscussionThread), threadId);
                }
                
                var currentUserId = CurrentUser.GetId();
                board.ReopenThread(threadId, currentUserId, Clock.Now);
                
                await _discussionBoardRepository.UpdateAsync(board);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:CouldNotReopenThread"]);
            }
        }

        [Authorize(CollaborationPermissions.Discussions.AddComments)]
        public virtual async Task<CommentDto> AddCommentAsync(AddCommentDto input)
        {
            try
            {
                // Find the board containing this thread
                var board = await _discussionBoardRepository.WithDetailsAsync();
                board = await AsyncExecuter.FirstOrDefaultAsync(
                    board.Where(b => b.Threads.Any(t => t.Id == input.ThreadId)));
                    
                if (board == null)
                {
                    throw new EntityNotFoundException(typeof(DiscussionThread), input.ThreadId);
                }
                
                var currentUserId = CurrentUser.GetId();
                var comment = await _discussionPersistenceService.AddCommentAsync(
                    board.Id,
                    input.ThreadId,
                    input.Content,
                    currentUserId);
                
                return await EnrichCommentDtoAsync(comment);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is PermissionDeniedException)
                {
                    throw new UserFriendlyException(L["Error:PermissionDenied"]);
                }
                if (ex is CollaborationDomainException domainException)
                {
                    if (domainException.Code == "Collaboration:ErrorMessage:InvalidOperation")
                    {
                        throw new UserFriendlyException(L["Error:CannotCommentInClosedThread"]);
                    }
                }
                throw new UserFriendlyException(L["Error:CouldNotAddComment"]);
            }
        }

        [Authorize(CollaborationPermissions.Discussions.ManageComments)]
        public virtual async Task<CommentDto> UpdateCommentAsync(Guid commentId, UpdateCommentDto input)
        {
            try
            {
                // Find the board and thread containing this comment
                var board = await _discussionBoardRepository.WithDetailsAsync();
                
                // Find the board containing the comment
                DiscussionThread thread = null;
                Comment comment = null;
                
                foreach (var b in await AsyncExecuter.ToListAsync(board))
                {
                    foreach (var t in b.Threads)
                    {
                        var c = t.GetComment(commentId);
                        if (c != null)
                        {
                            board = b;
                            thread = t;
                            comment = c;
                            break;
                        }
                    }
                    
                    if (comment != null) break;
                }
                
                if (comment == null)
                {
                    throw new EntityNotFoundException(typeof(Comment), commentId);
                }
                
                var currentUserId = CurrentUser.GetId();
                
                // Check if user is author or has manage permissions
                if (comment.AuthorId != currentUserId)
                {
                    throw new PermissionDeniedException(
                        currentUserId,
                        PermissionType.Write,
                        board.Resource.Type,
                        board.Resource.Id.Value);
                }
                
                board.UpdateComment(
                    thread.Id,
                    commentId,
                    currentUserId,
                    new CommentContent(input.Content),
                    Clock.Now);
                
                await _discussionBoardRepository.UpdateAsync(board);
                
                // Get the updated comment
                comment = thread.GetComment(commentId);
                return await EnrichCommentDtoAsync(comment);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is PermissionDeniedException)
                {
                    throw new UserFriendlyException(L["Error:PermissionDenied"]);
                }
                throw new UserFriendlyException(L["Error:CouldNotUpdateComment"]);
            }
        }

        private async Task<DiscussionBoardDto> EnrichDiscussionBoardDtoAsync(DiscussionBoard board)
        {
            var dto = ObjectMapper.Map<DiscussionBoard, DiscussionBoardDto>(board);
            
            // Enrich with resource display name
            dto.ResourceDisplayName = await _resourceDisplayNameProvider
                .GetResourceDisplayNameAsync(dto.ResourceType, dto.ResourceId);
                
            return dto;
        }

        private async Task<DiscussionThreadDto> EnrichDiscussionThreadDtoAsync(DiscussionThread thread)
        {
            var dto = ObjectMapper.Map<DiscussionThread, DiscussionThreadDto>(thread);
            
            // Enrich with creator name
            var creator = await _userLookupService.FindByIdAsync(thread.CreatorId);
            if (creator != null)
            {
                dto.CreatorUserName = creator.UserName;
            }
            
            // Enrich comments
            dto.Comments = new List<CommentDto>();
            foreach (var comment in thread.Comments)
            {
                dto.Comments.Add(await EnrichCommentDtoAsync(comment));
            }
            
            return dto;
        }

        private async Task<CommentDto> EnrichCommentDtoAsync(Comment comment)
        {
            var dto = ObjectMapper.Map<Comment, CommentDto>(comment);
            
            // Enrich with author name
            var author = await _userLookupService.FindByIdAsync(comment.AuthorId);
            if (author != null)
            {
                dto.AuthorUserName = author.UserName;
            }
            
            // Enrich with mentioned user names
            dto.MentionedUserNames = new List<string>();
            foreach (var userId in dto.MentionedUserIds)
            {
                var user = await _userLookupService.FindByIdAsync(userId);
                if (user != null)
                {
                    dto.MentionedUserNames.Add(user.UserName);
                }
                else
                {
                    dto.MentionedUserNames.Add(userId.ToString());
                }
            }
            
            return dto;
        }

        protected virtual async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in DiscussionBoardAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }
    }
}

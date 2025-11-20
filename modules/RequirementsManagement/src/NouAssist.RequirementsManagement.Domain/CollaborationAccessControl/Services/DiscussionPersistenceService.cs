namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Services
{
    /// <summary>
    /// Domain service for managing discussion boards and ensuring they exist for resources.
    /// </summary>
    public class DiscussionPersistenceService : DomainService, IDiscussionPersistenceService
    {
        private readonly IDiscussionBoardRepository _discussionBoardRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<DiscussionPersistenceService> _logger;

        public DiscussionPersistenceService(
            IDiscussionBoardRepository discussionBoardRepository,
            IAuthorizationService authorizationService,
            ILogger<DiscussionPersistenceService> logger)
        {
            _discussionBoardRepository = discussionBoardRepository;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        /// <summary>
        /// Gets or creates a discussion board for a resource.
        /// </summary>
        public async Task<DiscussionBoard> GetOrCreateDiscussionBoardAsync(
            TargetResourceType resourceType,
            Guid resourceId,
            Guid currentUserId)
        {
            try
            {
                // Ensure user has permission to view this resource
                await _authorizationService.AuthorizeAsync(
                    currentUserId,
                    PermissionType.Read,
                    resourceType,
                    resourceId);
                
                // Try to get existing board
                var board = await _discussionBoardRepository.GetByResourceAsync(
                    resourceType,
                    resourceId);
                
                if (board != null)
                {
                    return board;
                }
                
                // Create new board if it doesn't exist
                var newBoard = DiscussionBoard.Create(
                    GuidGenerator.Create(),
                    new TargetResource(resourceType, resourceId),
                    CurrentTenant.Id);
                
                await _discussionBoardRepository.InsertAsync(newBoard);
                
                return newBoard;
            }
            catch (PermissionDeniedException)
            {
                // Re-throw permission exceptions
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error creating or retrieving discussion board for {ResourceType} with ID {ResourceId}",
                    resourceType, resourceId);
                throw;
            }
        }

        /// <summary>
        /// Creates a new thread in a discussion board.
        /// </summary>
        public async Task<DiscussionThread> CreateThreadAsync(
            Guid discussionBoardId,
            string subject,
            string initialComment,
            Guid currentUserId)
        {
            try
            {
                var board = await _discussionBoardRepository.GetAsync(discussionBoardId);
                
                if (board == null)
                {
                    throw new EntityNotFoundException(nameof(DiscussionBoard), discussionBoardId);
                }
                
                // Ensure user has permission to add threads to this resource
                await _authorizationService.AuthorizeAsync(
                    currentUserId,
                    PermissionType.Write,
                    board.Resource.Type,
                    board.Resource.Id.GetValueOrDefault());
                
                // Create the thread
                var thread = board.CreateThread(
                    GuidGenerator.Create(),
                    new ThreadSubject(subject),
                    currentUserId,
                    new CommentContent(initialComment),
                    Clock.Now);
                
                await _discussionBoardRepository.UpdateAsync(board);
                
                return thread;
            }
            catch (EntityNotFoundException)
            {
                // Re-throw not found exceptions
                throw;
            }
            catch (PermissionDeniedException)
            {
                // Re-throw permission exceptions
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error creating thread in discussion board {DiscussionBoardId}",
                    discussionBoardId);
                throw;
            }
        }

        /// <summary>
        /// Adds a comment to an existing thread.
        /// </summary>
        public async Task<Comment> AddCommentAsync(
            Guid discussionBoardId,
            Guid threadId,
            string content,
            Guid currentUserId)
        {
            try
            {
                var board = await _discussionBoardRepository.GetAsync(discussionBoardId);
                
                if (board == null)
                {
                    throw new EntityNotFoundException(nameof(DiscussionBoard), discussionBoardId);
                }
                
                // Ensure user has permission to add comments to this resource
                await _authorizationService.AuthorizeAsync(
                    currentUserId,
                    PermissionType.Write,
                    board.Resource.Type,
                    board.Resource.Id.GetValueOrDefault());
                
                // Add the comment
                var comment = board.AddComment(
                    threadId,
                    GuidGenerator.Create(),
                    currentUserId,
                    new CommentContent(content),
                    Clock.Now);
                
                await _discussionBoardRepository.UpdateAsync(board);
                
                return comment;
            }
            catch (EntityNotFoundException)
            {
                // Re-throw not found exceptions
                throw;
            }
            catch (PermissionDeniedException)
            {
                // Re-throw permission exceptions
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error adding comment to thread {ThreadId} in discussion board {DiscussionBoardId}",
                    threadId, discussionBoardId);
                throw;
            }
        }
    }
}

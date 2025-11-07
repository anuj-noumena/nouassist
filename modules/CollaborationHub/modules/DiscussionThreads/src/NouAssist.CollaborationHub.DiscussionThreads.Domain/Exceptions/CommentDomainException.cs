using System;
using Volo.Abp;

namespace NouAssist.CollaborationHub.DiscussionThreads.Exceptions
{
    public class CommentDomainException : BusinessException
    {
        public CommentDomainException(string code, string message = null) 
            : base(code, message)
        {
        }
    }

    public class CommentNotFoundException : CommentDomainException
    {
        public CommentNotFoundException(Guid commentId) 
            : base("Comments:NotFound", $"Comment with ID {commentId} was not found")
        {
            WithData("CommentId", commentId);
        }
    }

    public class CommentModerationException : CommentDomainException
    {
        public CommentModerationException(string reason) 
            : base("Comments:ModerationFailed", reason)
        {
        }
    }
}

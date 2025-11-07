using System;
using Volo.Abp;

namespace NouAssist.CollaborationHub.TeamCollaboration.Exceptions
{
    public class TeamException : BusinessException
    {
        public TeamException(string message, string code = null) 
            : base(code ?? "Team:Error", message)
        {
        }
    }

    public class TeamMembershipException : BusinessException
    {
        public TeamMembershipException(string message, string code = null) 
            : base(code ?? "Team:MembershipError", message)
        {
        }
    }

    public class InsufficientPermissionException : BusinessException
    {
        public InsufficientPermissionException(string requiredPermission) 
            : base("Team:InsufficientPermission", 
                  $"User does not have the required permission: {requiredPermission}")
        {
            WithData("RequiredPermission", requiredPermission);
        }
    }
}

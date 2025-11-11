using System;
using System.Collections.Generic;
using NouAssist.IdentityAccess.Team.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.IdentityAccess.Team.Entities
{
    public class TeamMember : FullAuditedEntity<Guid>
    {
        public Guid TeamId { get; private set; }
        public Guid UserId { get; private set; }
        public TeamMemberRole Role { get; private set; }
        public TeamMemberStatus Status { get; private set; }
        public string Title { get; private set; }
        public Guid? InvitedBy { get; private set; }
        public DateTime? InvitedAt { get; private set; }
        public DateTime? JoinedAt { get; private set; }
        public DateTime? LeftAt { get; private set; }
        public string LeftReason { get; private set; }
        public Dictionary<string, object> Permissions { get; private set; }

        protected TeamMember() { /* ORM */ }

        public TeamMember(
            Guid id,
            Guid teamId,
            Guid userId,
            TeamMemberRole role,
            Guid? invitedBy = null,
            string title = null
        ) : base(id)
        {
            TeamId = teamId;
            UserId = userId;
            Role = role;
            InvitedBy = invitedBy;
            Title = title;
            Status = TeamMemberStatus.Invited;
            InvitedAt = DateTime.UtcNow;
            Permissions = new Dictionary<string, object>();
        }

        public bool IsActive => Status == TeamMemberStatus.Active;
        public bool IsInvited => Status == TeamMemberStatus.Invited;
        public bool HasJoined => JoinedAt.HasValue;
        public bool HasLeft => LeftAt.HasValue;

        public void AcceptInvite()
        {
            if (Status != TeamMemberStatus.Invited)
            {
                throw new BusinessException("Team:CanOnlyAcceptInvitedStatus");
            }

            Status = TeamMemberStatus.Active;
            JoinedAt = DateTime.UtcNow;
        }

        public void Promote(TeamMemberRole newRole)
        {
            Role = newRole;
        }

        public void Leave(string reason = null)
        {
            Status = TeamMemberStatus.Left;
            LeftAt = DateTime.UtcNow;
            LeftReason = reason;
        }

        public void Remove(string reason)
        {
            Status = TeamMemberStatus.Removed;
            LeftAt = DateTime.UtcNow;
            LeftReason = Check.NotNullOrWhiteSpace(reason, nameof(reason));
        }

        public void SetPermission(string key, object value)
        {
            Permissions[key] = value;
        }

        public bool HasPermission(string key)
        {
            return Permissions.ContainsKey(key);
        }
    }
}

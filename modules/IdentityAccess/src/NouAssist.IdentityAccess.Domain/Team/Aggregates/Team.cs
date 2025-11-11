using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IdentityAccess.Team.Entities;
using NouAssist.IdentityAccess.Team.Enums;
using NouAssist.IdentityAccess.Team.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IdentityAccess.Team.Aggregates
{
    public class Team : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Avatar { get; private set; }
        public string Color { get; private set; }
        public TeamType TeamType { get; private set; }
        public Guid? ParentTeamId { get; private set; }
        public Guid? LeaderId { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsPrivate { get; private set; }
        public int? MaxMembers { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<Guid> _adminIds = new();
        public IReadOnlyCollection<Guid> AdminIds => _adminIds.AsReadOnly();

        private readonly List<TeamMember> _members = new();
        public IReadOnlyCollection<TeamMember> Members => _members.AsReadOnly();

        protected Team() { /* ORM */ }

        public Team(
            Guid id,
            string name,
            TeamType teamType,
            Guid? tenantId,
            string description = null
        ) : base(id)
        {
            SetName(name);
            TeamType = teamType;
            TenantId = tenantId;
            Description = description;
            IsActive = true;
            IsPrivate = false;
            Metadata = new Dictionary<string, object>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
        }

        public void SetDescription(string description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 2000)
            {
                throw new BusinessException("Team:DescriptionTooLong");
            }
            Description = description;
        }

        public void SetAvatar(string avatar)
        {
            Avatar = avatar;
        }

        public void SetColor(string color)
        {
            Color = color;
        }

        public void SetParentTeam(Guid parentTeamId)
        {
            if (parentTeamId == Id)
            {
                throw new BusinessException("Team:CannotBeOwnParent");
            }
            ParentTeamId = parentTeamId;
        }

        public void AssignLeader(Guid userId)
        {
            // Ensure user is a member
            if (!_members.Any(m => m.UserId == userId && m.IsActive))
            {
                throw new BusinessException("Team:UserNotMember");
            }

            LeaderId = userId;
        }

        public void AddAdmin(Guid userId)
        {
            if (!_members.Any(m => m.UserId == userId && m.IsActive))
            {
                throw new BusinessException("Team:UserNotMember");
            }

            if (!_adminIds.Contains(userId))
            {
                _adminIds.Add(userId);
            }
        }

        public void RemoveAdmin(Guid userId)
        {
            _adminIds.Remove(userId);
            
            // If this was the only admin and there's no leader, promote a member
            if (!_adminIds.Any() && !LeaderId.HasValue)
            {
                var nextAdmin = _members
                    .Where(m => m.IsActive && m.Role == TeamMemberRole.Member)
                    .OrderBy(m => m.JoinedAt)
                    .FirstOrDefault();
                
                if (nextAdmin != null)
                {
                    AddAdmin(nextAdmin.UserId);
                }
            }
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void MakePrivate()
        {
            IsPrivate = true;
        }

        public void MakePublic()
        {
            IsPrivate = false;
        }

        public void SetMaxMembers(int maxMembers)
        {
            if (maxMembers < _members.Count(m => m.IsActive))
            {
                throw new BusinessException("Team:MaxMembersBelowCurrentCount")
                    .WithData("CurrentCount", _members.Count(m => m.IsActive));
            }
            MaxMembers = maxMembers;
        }

        // Member management
        public TeamMember AddMember(
            Guid userId,
            TeamMemberRole role,
            Guid? invitedBy = null,
            string title = null
        )
        {
            if (_members.Any(m => m.UserId == userId && m.IsActive))
            {
                throw new BusinessException("Team:UserAlreadyMember");
            }

            if (MaxMembers.HasValue && 
                _members.Count(m => m.IsActive) >= MaxMembers.Value)
            {
                throw new BusinessException("Team:MaxMembersReached");
            }

            var member = new TeamMember(
                Guid.NewGuid(),
                Id,
                userId,
                role,
                invitedBy,
                title
            );

            _members.Add(member);

            // Add domain event
            AddDistributedEvent(new TeamMemberAddedEto
            {
                TeamId = Id,
                UserId = userId,
                Role = role,
                InvitedBy = invitedBy,
                TenantId = TenantId,
                AddedAt = DateTime.UtcNow
            });

            return member;
        }

        public void RemoveMember(Guid userId, string reason)
        {
            var member = _members.FirstOrDefault(m => m.UserId == userId && m.IsActive);
            if (member == null)
            {
                throw new BusinessException("Team:MemberNotFound");
            }

            // Cannot remove last owner/admin
            if (member.Role == TeamMemberRole.Owner || _adminIds.Contains(userId))
            {
                var otherAdmins = _members.Count(m => 
                    m.IsActive && 
                    m.UserId != userId && 
                    (m.Role == TeamMemberRole.Owner || _adminIds.Contains(m.UserId)));

                if (otherAdmins == 0)
                {
                    throw new BusinessException("Team:CannotRemoveLastAdmin");
                }
            }

            member.Remove(reason);
            
            // Clean up admin list
            if (_adminIds.Contains(userId))
            {
                _adminIds.Remove(userId);
            }

            // Reassign leader if needed
            if (LeaderId == userId)
            {
                LeaderId = null;
            }
            
            AddDistributedEvent(new TeamMemberRemovedEto
            {
                TeamId = Id,
                UserId = userId,
                Reason = reason,
                TenantId = TenantId,
                RemovedAt = DateTime.UtcNow
            });
        }

        public void UpdateMemberRole(Guid userId, TeamMemberRole newRole)
        {
            var member = _members.FirstOrDefault(m => m.UserId == userId && m.IsActive);
            if (member == null)
            {
                throw new BusinessException("Team:MemberNotFound");
            }

            member.Promote(newRole);
        }

        public void AcceptMemberInvitation(Guid userId)
        {
            var member = _members.FirstOrDefault(m => m.UserId == userId);
            if (member == null)
            {
                throw new BusinessException("Team:MemberNotFound");
            }

            member.AcceptInvite();
        }

        public int GetActiveMemberCount() => _members.Count(m => m.IsActive);

        public bool IsMember(Guid userId) =>
            _members.Any(m => m.UserId == userId && m.IsActive);

        public bool IsAdmin(Guid userId) =>
            _adminIds.Contains(userId) || LeaderId == userId;

        public bool IsLeader(Guid userId) =>
            LeaderId.HasValue && LeaderId.Value == userId;
    }
}

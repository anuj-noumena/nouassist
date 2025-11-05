using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Approval.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.Approval.Entities
{
    public class ApprovalStep : FullAuditedEntity<Guid>
    {
        public Guid WorkflowId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int OrderIndex { get; private set; }
        public ApprovalRequirement Requirement { get; private set; }
        public int? RequiredApprovals { get; private set; }
        public bool AllowSelfApproval { get; private set; }
        public int? TimeoutHours { get; private set; }
        public string TimeoutAction { get; private set; }
        public Dictionary<string, object> Conditions { get; private set; }
        public Dictionary<string, object> CustomFieldValues { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }

        private readonly List<Guid> _approverUserIds = new();
        public IReadOnlyCollection<Guid> ApproverUserIds => _approverUserIds.AsReadOnly();

        private readonly List<Guid> _approverRoleIds = new();
        public IReadOnlyCollection<Guid> ApproverRoleIds => _approverRoleIds.AsReadOnly();

        private readonly List<Guid> _approverTeamIds = new();
        public IReadOnlyCollection<Guid> ApproverTeamIds => _approverTeamIds.AsReadOnly();

        protected ApprovalStep() { /* ORM */ }

        public ApprovalStep(
            Guid id,
            Guid workflowId,
            string name,
            int orderIndex,
            ApprovalRequirement requirement,
            string description = null
        ) : base(id)
        {
            WorkflowId = workflowId;
            SetName(name);
            OrderIndex = orderIndex;
            Requirement = requirement;
            Description = description;
            AllowSelfApproval = false;
            Conditions = new Dictionary<string, object>();
            CustomFieldValues = new Dictionary<string, object>();
            Metadata = new Dictionary<string, object>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
        }

        public void SetOrderIndex(int orderIndex)
        {
            if (orderIndex < 1)
            {
                throw new BusinessException("Approval:InvalidOrderIndex");
            }
            OrderIndex = orderIndex;
        }

        public void SetRequiredApprovals(int count)
        {
            if (count < 1)
            {
                throw new BusinessException("Approval:InvalidRequiredApprovalsCount");
            }
            RequiredApprovals = count;
        }

        public void AllowSelfApprovalForStep()
        {
            AllowSelfApproval = true;
        }

        public void SetTimeout(int hours, string action)
        {
            if (hours <= 0)
            {
                throw new BusinessException("Approval:InvalidTimeoutHours");
            }
            TimeoutHours = hours;
            TimeoutAction = action;
        }

        public void AddApproverUser(Guid userId)
        {
            if (!_approverUserIds.Contains(userId))
            {
                _approverUserIds.Add(userId);
            }
        }

        public void RemoveApproverUser(Guid userId)
        {
            _approverUserIds.Remove(userId);
        }

        public void AddApproverRole(Guid roleId)
        {
            if (!_approverRoleIds.Contains(roleId))
            {
                _approverRoleIds.Add(roleId);
            }
        }

        public void RemoveApproverRole(Guid roleId)
        {
            _approverRoleIds.Remove(roleId);
        }

        public void AddApproverTeam(Guid teamId)
        {
            if (!_approverTeamIds.Contains(teamId))
            {
                _approverTeamIds.Add(teamId);
            }
        }

        public void RemoveApproverTeam(Guid teamId)
        {
            _approverTeamIds.Remove(teamId);
        }

        public bool HasApprovers()
        {
            return _approverUserIds.Any() || 
                   _approverRoleIds.Any() || 
                   _approverTeamIds.Any();
        }

        public void ValidateApprovers()
        {
            if (!HasApprovers())
            {
                throw new BusinessException("Approval:StepMustHaveApprovers");
            }
        }

        public void SetCondition(string key, object value)
        {
            Conditions[key] = value;
        }

        public bool EvaluateConditions(Dictionary<string, object> context)
        {
            if (Conditions == null || !Conditions.Any())
            {
                return true;
            }

            // Simple condition evaluation - extend as needed
            foreach (var condition in Conditions)
            {
                if (!context.TryGetValue(condition.Key, out var value) || 
                    !value.Equals(condition.Value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

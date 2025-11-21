using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Events
{
    /// <summary>
    /// Event raised when a user's permissions are changed.
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.CollaborationAccessControl.PermissionChanged")]
    public class PermissionChangedEto
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public TargetResourceType ResourceType { get; set; }
        public Guid? ResourceId { get; set; }
        public List<PermissionType> AddedPermissions { get; set; } = new();
        public List<PermissionType> RemovedPermissions { get; set; } = new();
        public Guid ChangedByUserId { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}

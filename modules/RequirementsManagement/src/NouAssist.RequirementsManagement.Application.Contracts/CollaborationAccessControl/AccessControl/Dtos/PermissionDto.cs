using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos
{
    public class PermissionDto
    {
        public PermissionType Type { get; set; }
        public TargetResourceType ResourceType { get; set; }
        public Guid? ResourceId { get; set; }
        public string ResourceDisplayName { get; set; }
    }
}

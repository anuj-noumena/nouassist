using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos
{
    public class AddPermissionToRoleDto
    {
        [Required]
        public PermissionType PermissionType { get; set; }

        [Required]
        public TargetResourceType ResourceType { get; set; }

        public Guid? ResourceId { get; set; }
    }
}

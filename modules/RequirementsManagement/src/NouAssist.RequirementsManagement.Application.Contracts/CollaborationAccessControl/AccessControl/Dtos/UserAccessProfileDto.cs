namespace NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos
{
    public class UserAccessProfileDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } // Display name from identity system
        public List<RoleDto> Roles { get; set; }
    }
}

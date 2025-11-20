namespace NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos
{
    public class UpdateRoleDto
    {
        [Required]
        [StringLength(CollaborationConsts.MaxRoleNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(CollaborationConsts.MaxRoleDescriptionLength)]
        public string Description { get; set; }
    }
}

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.AccessControl.Dtos
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(CollaborationConsts.MaxRoleNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(CollaborationConsts.MaxRoleDescriptionLength)]
        public string Description { get; set; }
    }
}

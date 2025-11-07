namespace NouAssist.CollaborationHub.TeamCollaboration.Enums
{
    public enum Permission
    {
        // Project permissions
        ProjectCreate = 1,
        ProjectRead = 2,
        ProjectUpdate = 3,
        ProjectDelete = 4,
        ProjectArchive = 5,

        // Task permissions
        TaskCreate = 10,
        TaskRead = 11,
        TaskUpdate = 12,
        TaskDelete = 13,
        TaskAssign = 14,

        // Team permissions
        TeamManage = 20,
        TeamMemberAdd = 21,
        TeamMemberRemove = 22,
        TeamRoleAssign = 23,

        // Workflow permissions
        WorkflowManage = 30,
        WorkflowTransition = 31,

        // Admin permissions
        SettingsManage = 40,
        RoleManage = 41,
        UserManage = 42
    }

    public static class PermissionExtensions
    {
        public static string ToPermissionName(this Permission permission)
        {
            return $"NouAssist.{permission}";
        }

        public static PermissionCategory GetCategory(this Permission permission)
        {
            return (int)permission switch
            {
                >= 1 and < 10 => PermissionCategory.Project,
                >= 10 and < 20 => PermissionCategory.Task,
                >= 20 and < 30 => PermissionCategory.Team,
                >= 30 and < 40 => PermissionCategory.Workflow,
                >= 40 and < 50 => PermissionCategory.Admin,
                _ => PermissionCategory.Unknown
            };
        }
    }

    public enum PermissionCategory
    {
        Unknown,
        Project,
        Task,
        Team,
        Workflow,
        Admin
    }
}

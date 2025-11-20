namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Enums
{
    /// <summary>
    /// Defines the types of permissions that can be assigned to users.
    /// </summary>
    public enum PermissionType
    {
        Read = 1,
        Write = 2,
        ApproveReq = 3,
        CreateBaseline = 4,
        DeleteResource = 5,
        ManagePermissions = 6,
        ManageRoles = 7,
        FullControl = 99
    }
}

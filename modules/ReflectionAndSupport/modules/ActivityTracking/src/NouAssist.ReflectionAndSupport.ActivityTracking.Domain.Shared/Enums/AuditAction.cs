namespace NouAssist.ReflectionAndSupport.ActivityTracking.Enums
{
    public enum AuditAction : byte
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4,
        Login = 5,
        Logout = 6,
        PasswordChange = 7,
        PermissionChange = 8,
        RoleChange = 9,
        SettingsChange = 10,
        Export = 11,
        Import = 12,
        Share = 13,
        Unshare = 14,
        Approve = 15,
        Reject = 16
    }
}

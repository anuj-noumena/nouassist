namespace NouAssist.BusinessOperations.DocumentManagement.Enums
{
    /// <summary>
    /// Defines permission levels for document access
    /// </summary>
    public enum AccessPermissionLevel
    {
        None = 0,
        View = 10,
        Download = 20,
        Edit = 30,
        ManageAccess = 40,
        Owner = 50
    }
}

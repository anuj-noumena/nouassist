namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Constants
{
    /// <summary>
    /// Constants for the Collaboration module.
    /// </summary>
    public static class CollaborationConsts
    {
        // Role constants
        public const int MaxRoleNameLength = 64;
        public const int MaxRoleDescriptionLength = 512;

        // Permission constants
        public const int MaxPermissionDescriptionLength = 256;

        // Notification constants
        public const int MaxNotificationTitleLength = 128;
        public const int MaxNotificationContentLength = 1024;

        // Discussion constants
        public const int MaxThreadSubjectLength = 200;
        public const int MaxCommentContentLength = 4000;
        public const int MaxCommentAttachmentsCount = 5;
    }
}

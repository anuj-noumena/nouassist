namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Permissions
{
    public static class CollaborationPermissions
    {
        public const string GroupName = "Collaboration";

        // Access Control permissions
        public static class AccessControl
        {
            public const string Default = GroupName + ".AccessControl";
            public const string ManageRoles = Default + ".ManageRoles";
            public const string AssignRoles = Default + ".AssignRoles";
            public const string ManagePermissions = Default + ".ManagePermissions";
            public const string ViewPermissions = Default + ".ViewPermissions";
        }

        // Notification permissions
        public static class Notifications
        {
            public const string Default = GroupName + ".Notifications";
            public const string Configure = Default + ".Configure";
            public const string SubscribeToEvents = Default + ".SubscribeToEvents";
        }

        // Discussion permissions
        public static class Discussions
        {
            public const string Default = GroupName + ".Discussions";
            public const string CreateThreads = Default + ".CreateThreads";
            public const string AddComments = Default + ".AddComments";
            public const string ManageThreads = Default + ".ManageThreads";  // Close/reopen
            public const string ManageComments = Default + ".ManageComments"; // Edit/delete
        }
    }
}

namespace NouAssist.BusinessOperations.DocumentManagement.Constants
{
    public static class DocumentManagementConsts
    {
        public const string ModuleName = "DocumentManagement";
        
        public static class Document
        {
            public const int MaxFilenameLength = 256;
            public const int MaxMimeTypeLength = 128;
            public const int MaxPathLength = 1024;
            public const long MaxFileSizeBytes = 104857600; // 100MB default
        }
        
        public static class Permissions
        {
            public const string GroupName = "DocumentManagement";
            
            public static class Documents
            {
                public const string Default = GroupName + ".Documents";
                public const string Create = Default + ".Create";
                public const string Edit = Default + ".Edit";
                public const string Delete = Default + ".Delete";
                public const string ManageAccess = Default + ".ManageAccess";
            }
        }
    }
}

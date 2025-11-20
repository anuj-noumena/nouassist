namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Permissions
{
    public static class KnowledgeInnovationPermissions
    {
        public const string GroupName = "KnowledgeInnovation";

        // Knowledge Asset Permissions
        public static class KnowledgeAsset
        {
            public const string Default = GroupName + ".KnowledgeAsset";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }

        // Learning Resource Permissions
        public static class LearningResource
        {
            public const string Default = GroupName + ".LearningResource";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Publish = Default + ".Publish";
            public const string Feature = Default + ".Feature";
        }

        // Idea Permissions
        public static class Idea
        {
            public const string Default = GroupName + ".Idea";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Evaluate = Default + ".Evaluate";
            public const string ChangeStatus = Default + ".ChangeStatus";
        }

        // Decision Record Permissions
        public static class DecisionRecord
        {
            public const string Default = GroupName + ".DecisionRecord";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string MakeDecision = Default + ".MakeDecision";
            public const string Implement = Default + ".Implement";
            public const string Evaluate = Default + ".Evaluate";
        }

        // Mentor Profile Permissions
        public static class MentorProfile
        {
            public const string Default = GroupName + ".MentorProfile";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManageRelationships = Default + ".ManageRelationships";
            public const string AddAdvice = Default + ".AddAdvice";
        }
    }
}

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Permissions
{
    /// <summary>
    /// Permission definitions for Personal Productivity Intelligence module
    /// </summary>
    public static class PersonalProductivityIntelligencePermissions
    {
        /// <summary>
        /// Root permission group name
        /// </summary>
        public const string GroupName = "PersonalProductivityIntelligence";
        
        /// <summary>
        /// User Productivity Profile permissions
        /// </summary>
        public static class UserProfile
        {
            /// <summary>
            /// Base permission for user profiles
            /// </summary>
            public const string Default = GroupName + ".UserProfile";
            
            /// <summary>
            /// Permission to view user profiles
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to update cognitive load
            /// </summary>
            public const string UpdateCognitiveLoad = Default + ".UpdateCognitiveLoad";
            
            /// <summary>
            /// Permission to record task completions
            /// </summary>
            public const string RecordTaskCompletion = Default + ".RecordTaskCompletion";
            
            /// <summary>
            /// Permission to update user context
            /// </summary>
            public const string UpdateContext = Default + ".UpdateContext";
            
            /// <summary>
            /// Permission to set preferences
            /// </summary>
            public const string SetPreference = Default + ".SetPreference";
            
            /// <summary>
            /// Permission to analyze work patterns
            /// </summary>
            public const string AnalyzeWorkPattern = Default + ".AnalyzeWorkPattern";
        }
        
        /// <summary>
        /// Prediction permissions
        /// </summary>
        public static class Prediction
        {
            /// <summary>
            /// Base permission for predictions
            /// </summary>
            public const string Default = GroupName + ".Prediction";
            
            /// <summary>
            /// Permission to view predictions
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to create predictions
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to record actual outcomes
            /// </summary>
            public const string RecordOutcome = Default + ".RecordOutcome";
        }
        
        /// <summary>
        /// Recommendation permissions
        /// </summary>
        public static class Recommendation
        {
            /// <summary>
            /// Base permission for recommendations
            /// </summary>
            public const string Default = GroupName + ".Recommendation";
            
            /// <summary>
            /// Permission to view recommendations
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to generate recommendations
            /// </summary>
            public const string Generate = Default + ".Generate";
            
            /// <summary>
            /// Permission to mark recommendations
            /// </summary>
            public const string Mark = Default + ".Mark";
            
            /// <summary>
            /// Permission to rate recommendations
            /// </summary>
            public const string Rate = Default + ".Rate";
        }
        
        /// <summary>
        /// Personal Automation permissions
        /// </summary>
        public static class Automation
        {
            /// <summary>
            /// Base permission for automations
            /// </summary>
            public const string Default = GroupName + ".Automation";
            
            /// <summary>
            /// Permission to view automations
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to create automations
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to edit automations
            /// </summary>
            public const string Edit = Default + ".Edit";
            
            /// <summary>
            /// Permission to delete automations
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to execute automations
            /// </summary>
            public const string Execute = Default + ".Execute";
        }
        
        /// <summary>
        /// Performance Tracking permissions
        /// </summary>
        public static class Performance
        {
            /// <summary>
            /// Base permission for performance tracking
            /// </summary>
            public const string Default = GroupName + ".Performance";
            
            /// <summary>
            /// Permission to view performance data
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to create performance snapshots
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to generate insights
            /// </summary>
            public const string GenerateInsights = Default + ".GenerateInsights";
            
            /// <summary>
            /// Permission to view trends
            /// </summary>
            public const string ViewTrends = Default + ".ViewTrends";
        }
    }
}

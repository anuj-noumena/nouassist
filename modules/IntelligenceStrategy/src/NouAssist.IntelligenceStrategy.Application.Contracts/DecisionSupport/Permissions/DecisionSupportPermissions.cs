namespace NouAssist.IntelligenceStrategy.DecisionSupport.Permissions
{
    /// <summary>
    /// Permission definitions for the Decision Support module
    /// </summary>
    public static class DecisionSupportPermissions
    {
        /// <summary>
        /// Root permission group name for Decision Support module
        /// </summary>
        public const string GroupName = "DecisionSupport";

        /// <summary>
        /// Strategic Initiative permissions
        /// </summary>
        public static class StrategicInitiative
        {
            /// <summary>
            /// Base permission for strategic initiatives
            /// </summary>
            public const string Default = GroupName + ".StrategicInitiative";
            
            /// <summary>
            /// Permission to create strategic initiatives
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to view strategic initiatives
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to update strategic initiatives
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete strategic initiatives
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to approve strategic initiatives
            /// </summary>
            public const string Approve = Default + ".Approve";
            
            /// <summary>
            /// Permission to reject strategic initiatives
            /// </summary>
            public const string Reject = Default + ".Reject";
        }

        /// <summary>
        /// Decision Model permissions
        /// </summary>
        public static class DecisionModel
        {
            /// <summary>
            /// Base permission for decision models
            /// </summary>
            public const string Default = GroupName + ".DecisionModel";
            
            /// <summary>
            /// Permission to create decision models
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to view decision models
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to update decision models
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete decision models
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to finalize decision models
            /// </summary>
            public const string Finalize = Default + ".Finalize";
        }
    }
}

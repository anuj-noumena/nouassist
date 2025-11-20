using Volo.Abp.Reflection;

namespace NouAssist.BusinessOperations.ProjectExecution.Permissions
{
    /// <summary>
    /// Defines the permissions for the ProjectExecution module
    /// </summary>
    public class ProjectExecutionPermissions
    {
        public const string GroupName = "ProjectExecution";
        
        public static class Projects
        {
            public const string Default = GroupName + ".Projects";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ManageStatus = Default + ".ManageStatus";
            public const string ManageMembers = Default + ".ManageMembers";
            public const string ManagePhases = Default + ".ManagePhases";
            public const string ManageMilestones = Default + ".ManageMilestones";
        }
        
        public static class Tasks
        {
            public const string Default = GroupName + ".Tasks";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ManageStatus = Default + ".ManageStatus";
            public const string ManageAssignments = Default + ".ManageAssignments";
            public const string ManageDependencies = Default + ".ManageDependencies";
        }
        
        public static class TimeLogs
        {
            public const string Default = GroupName + ".TimeLogs";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ViewOthers = Default + ".ViewOthers";
        }
        
        public static class Reports
        {
            public const string Default = GroupName + ".Reports";
            public const string ViewProjectReports = Default + ".ViewProjectReports";
            public const string ViewResourceReports = Default + ".ViewResourceReports";
        }
        
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ProjectExecutionPermissions));
        }
    }
}
namespace NouAssist.BusinessOperations.ProjectExecution.Permissions
{
    /// <summary>
    /// Defines permissions for the Project Execution module
    /// </summary>
    public static class ProjectExecutionPermissions
    {
        public const string GroupName = "ProjectExecution";

        /// <summary>
        /// Permissions for project management
        /// </summary>
        public static class Projects
        {
            public const string Default = GroupName + ".Projects";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ManagePhases = Default + ".ManagePhases";
            public const string ManageMilestones = Default + ".ManageMilestones";
            public const string ManageMembers = Default + ".ManageMembers";
        }

        /// <summary>
        /// Permissions for task management
        /// </summary>
        public static class Tasks
        {
            public const string Default = GroupName + ".Tasks";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string Assign = Default + ".Assign";
            public const string ManageDependencies = Default + ".ManageDependencies";
        }

        /// <summary>
        /// Permissions for time logging
        /// </summary>
        public static class TimeLogs
        {
            public const string Default = GroupName + ".TimeLogs";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
            public const string ViewOthers = Default + ".ViewOthers";
        }
    }
}

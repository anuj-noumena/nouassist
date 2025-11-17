using Volo.Abp.Reflection;

namespace NouAssist.WorkManagement.Permissions;

public class WorkManagementPermissions
{
    public const string GroupName = "WorkManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(WorkManagementPermissions));
    }

    // Approval Permissions
    public static class Approval
    {
        public const string GroupName = "Approval";

        public static class Workflows
        {
            public const string Default = GroupName + ".Workflows";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Activate = Default + ".Activate";
            public const string Deactivate = Default + ".Deactivate";
        }

        public static class Requests
        {
            public const string Default = GroupName + ".Requests";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string View = Default + ".View";
            public const string Approve = Default + ".Approve";
            public const string Reject = Default + ".Reject";
            public const string Cancel = Default + ".Cancel";
        }
        
        public static class Analytics
        {
            public const string Default = GroupName + ".Analytics";
            public const string View = Default + ".View";
            public const string ViewAll = Default + ".ViewAll";
            public const string Export = Default + ".Export";
        }
    }

    // GTD Permissions
    public static class GTD
    {
        public const string GroupName = "GTD";

        public static class Horizons
        {
            public const string Default = GroupName + ".Horizons";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Review = Default + ".Review";
            public const string Publish = Default + ".Publish";
        }

        public static class Ticklers
        {
            public const string Default = GroupName + ".Ticklers";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Surface = Default + ".Surface";
            public const string Process = Default + ".Process";
        }

        public static class SomedayMaybe
        {
            public const string Default = GroupName + ".SomedayMaybe";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Promote = Default + ".Promote";
            public const string Review = Default + ".Review";
        }

        public static class WaitingFor
        {
            public const string Default = GroupName + ".WaitingFor";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Resolve = Default + ".Resolve";
        }
    }

    // Project Management Permissions
    public static class ProjectManagement
    {
        public const string GroupName = "Projects";

        public static class Projects
        {
            public const string Default = GroupName + ".Projects";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Complete = Default + ".Complete";
            public const string Cancel = Default + ".Cancel";
            public const string Archive = Default + ".Archive";
        }

        public static class Requirements
        {
            public const string Default = GroupName + ".Requirements";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class Sprints
        {
            public const string Default = GroupName + ".Sprints";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Start = Default + ".Start";
            public const string Complete = Default + ".Complete";
        }

        public static class Dependencies
        {
            public const string Default = GroupName + ".Dependencies";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Resolve = Default + ".Resolve";
        }

        public static class SuccessCriteria
        {
            public const string Default = GroupName + ".SuccessCriteria";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string MarkAsMet = Default + ".MarkAsMet";
        }

        public static class Events
        {
            public const string Default = GroupName + ".Events";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }

    // Reviews Permissions
    public static class Reviews
    {
        public const string GroupName = "Reviews";

        public static class ReviewManagement
        {
            public const string Default = GroupName + ".Reviews";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Complete = Default + ".Complete";
            public const string ViewOwn = Default + ".ViewOwn";
        }

        public static class Templates
        {
            public const string Default = GroupName + ".Templates";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ViewSystem = Default + ".ViewSystem";
        }

        public static class Analytics
        {
            public const string Default = GroupName + ".Analytics";
            public const string ViewOwn = Default + ".ViewOwn";
        }
    }

    // Task Management Permissions
    public static class TaskManagement
    {
        public const string GroupName = "Tasks";

        public static class Tasks
        {
            public const string Default = GroupName + ".Tasks";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageActionSteps = Default + ".ManageActionSteps";
            public const string ManageChecklistItems = Default + ".ManageChecklistItems";
            public const string ManageCriteria = Default + ".ManageCriteria";
        }

        public static class TaskInstances
        {
            public const string Default = GroupName + ".TaskInstances";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Start = Default + ".Start";
            public const string Complete = Default + ".Complete";
            public const string Delegate = Default + ".Delegate";
            public const string Block = Default + ".Block";
            public const string UpdateProgress = Default + ".UpdateProgress";
        }

        public static class TaskTypes
        {
            public const string Default = GroupName + ".TaskTypes";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManageCustomFields = Default + ".ManageCustomFields";
        }
    }

    // Workflow Permissions
    public static class Workflow
    {
        public const string GroupName = "Workflow";

        public static class Workflows
        {
            public const string Default = GroupName + ".Workflows";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Activate = Default + ".Activate";
            public const string Deactivate = Default + ".Deactivate";
        }
    }
}

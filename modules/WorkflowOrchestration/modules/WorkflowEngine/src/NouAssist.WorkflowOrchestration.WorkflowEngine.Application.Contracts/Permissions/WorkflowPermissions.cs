using Volo.Abp.Reflection;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Permissions;

public class WorkflowPermissions
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

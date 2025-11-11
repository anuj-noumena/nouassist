namespace NouAssist.WorkManagement.Workflow.Enums
{
    /// <summary>
    /// Defines the scope at which a transition permission policy applies.
    /// </summary>
    public enum PermissionPolicyScope

    {
        /// <summary>
        /// Global policy that applies to all projects using this workflow.
        /// Evaluated when no project-specific policy exists.
        /// </summary>
        Global = 0,

        /// <summary>
        /// Project-specific policy that applies only to a specific project.
        /// Takes precedence over global policies.
        /// </summary>
        Project = 1,

        /// <summary>
        /// Custom context-based policy for specialized scenarios.
        /// Future extension point for complex permission requirements.
        /// </summary>
        Custom = 2
    }
}
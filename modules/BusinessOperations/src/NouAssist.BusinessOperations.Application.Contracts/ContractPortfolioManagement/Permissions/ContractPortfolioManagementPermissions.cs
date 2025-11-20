namespace NouAssist.ContractPortfolioManagement.Permissions;

/// <summary>
/// Defines permission constants for Contract Portfolio Management module
/// </summary>
public static class ContractPortfolioManagementPermissions
{
    public const string GroupName = "ContractPortfolioManagement";

    /// <summary>
    /// Contract management permissions
    /// </summary>
    public static class Contract
    {
        public const string Default = GroupName + ".Contract";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage"; // Special permission for all operations
        public const string ChangeStatus = Default + ".ChangeStatus";
        public const string CreateVersion = Default + ".CreateVersion";
        public const string AddAmendment = Default + ".AddAmendment";
    }

    /// <summary>
    /// Portfolio management permissions
    /// </summary>
    public static class Portfolio
    {
        public const string Default = GroupName + ".Portfolio";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage"; // Special permission for all operations
        public const string AddItem = Default + ".AddItem";
        public const string RemoveItem = Default + ".RemoveItem";
        public const string Share = Default + ".Share";
    }

    /// <summary>
    /// Portfolio roadmap permissions
    /// </summary>
    public static class PortfolioRoadmap
    {
        public const string Default = GroupName + ".PortfolioRoadmap";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
        public const string Manage = Default + ".Manage"; // Special permission for all operations
        public const string AddMilestone = Default + ".AddMilestone";
        public const string ManageDependencies = Default + ".ManageDependencies";
    }
}

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Enums
{
    /// <summary>
    /// Defines the type category of a resource
    /// </summary>
    public enum ResourceCategory
    {
        /// <summary>
        /// Human resource (employee, contractor, etc.)
        /// </summary>
        HumanResource = 1,

        /// <summary>
        /// Equipment resource (hardware, tool, vehicle, etc.)
        /// </summary>
        Equipment = 2,

        /// <summary>
        /// Facility resource (room, building, space, etc.)
        /// </summary>
        Facility = 3,

        /// <summary>
        /// Digital resource (license, account, subscription, etc.)
        /// </summary>
        Digital = 4,

        /// <summary>
        /// Other type of resource
        /// </summary>
        Other = 99
    }
}

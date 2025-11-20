namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Permissions
{
    /// <summary>
    /// Permission definitions for the RIM module
    /// </summary>
    public static class RIMPermissions
    {
        /// <summary>
        /// Group name for RIM permissions
        /// </summary>
        public const string GroupName = "RIM";

        /// <summary>
        /// Permissions for ResourceType management
        /// </summary>
        public static class ResourceType
        {
            /// <summary>
            /// Default group permission
            /// </summary>
            public const string Default = GroupName + ".ResourceType";
            
            /// <summary>
            /// Permission to create resource types
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update resource types
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete resource types
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view resource types
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to manage resource type attributes
            /// </summary>
            public const string ManageAttributes = Default + ".ManageAttributes";
        }

        /// <summary>
        /// Permissions for ResourceInstance management
        /// </summary>
        public static class ResourceInstance
        {
            /// <summary>
            /// Default group permission
            /// </summary>
            public const string Default = GroupName + ".ResourceInstance";
            
            /// <summary>
            /// Permission to create resource instances
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update resource instances
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete resource instances
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view resource instances
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to allocate resources
            /// </summary>
            public const string Allocate = Default + ".Allocate";
            
            /// <summary>
            /// Permission to log resource usage
            /// </summary>
            public const string LogUsage = Default + ".LogUsage";
        }

        /// <summary>
        /// Permissions for InventoryItem management
        /// </summary>
        public static class InventoryItem
        {
            /// <summary>
            /// Default group permission
            /// </summary>
            public const string Default = GroupName + ".InventoryItem";
            
            /// <summary>
            /// Permission to create inventory items
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update inventory items
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete inventory items
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view inventory items
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to adjust stock levels
            /// </summary>
            public const string AdjustStock = Default + ".AdjustStock";
            
            /// <summary>
            /// Permission to transfer inventory
            /// </summary>
            public const string Transfer = Default + ".Transfer";
        }

        /// <summary>
        /// Permissions for ResourceBooking management
        /// </summary>
        public static class ResourceBooking
        {
            /// <summary>
            /// Default group permission
            /// </summary>
            public const string Default = GroupName + ".ResourceBooking";
            
            /// <summary>
            /// Permission to create bookings
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update bookings
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete bookings
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view bookings
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to confirm/reject bookings
            /// </summary>
            public const string Approve = Default + ".Approve";
            
            /// <summary>
            /// Permission to check-in/check-out resources
            /// </summary>
            public const string CheckInOut = Default + ".CheckInOut";
        }

        /// <summary>
        /// Permissions for StorageLocation management
        /// </summary>
        public static class StorageLocation
        {
            /// <summary>
            /// Default group permission
            /// </summary>
            public const string Default = GroupName + ".StorageLocation";
            
            /// <summary>
            /// Permission to create storage locations
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update storage locations
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete storage locations
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view storage locations
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to manage location hierarchy
            /// </summary>
            public const string ManageHierarchy = Default + ".ManageHierarchy";
        }

        /// <summary>
        /// Permissions for InventoryAudit management
        /// </summary>
        public static class InventoryAudit
        {
            /// <summary>
            /// Default group permission
            /// </summary>
            public const string Default = GroupName + ".InventoryAudit";
            
            /// <summary>
            /// Permission to create audit sessions
            /// </summary>
            public const string Create = Default + ".Create";
            
            /// <summary>
            /// Permission to update audit sessions
            /// </summary>
            public const string Update = Default + ".Update";
            
            /// <summary>
            /// Permission to delete audit sessions
            /// </summary>
            public const string Delete = Default + ".Delete";
            
            /// <summary>
            /// Permission to view audit sessions
            /// </summary>
            public const string View = Default + ".View";
            
            /// <summary>
            /// Permission to record audit discrepancies
            /// </summary>
            public const string RecordDiscrepancies = Default + ".RecordDiscrepancies";
            
            /// <summary>
            /// Permission to reconcile audit findings
            /// </summary>
            public const string Reconcile = Default + ".Reconcile";
        }

        /// <summary>
        /// Permissions for Reports and Analytics
        /// </summary>
        public static class Reports
        {
            /// <summary>
            /// Default group permission
            /// </summary>
            public const string Default = GroupName + ".Reports";
            
            /// <summary>
            /// Permission to view resource utilization reports
            /// </summary>
            public const string ResourceUtilization = Default + ".ResourceUtilization";
            
            /// <summary>
            /// Permission to view inventory reports
            /// </summary>
            public const string Inventory = Default + ".Inventory";
            
            /// <summary>
            /// Permission to view booking reports
            /// </summary>
            public const string Bookings = Default + ".Bookings";
            
            /// <summary>
            /// Permission to view audit reports
            /// </summary>
            public const string Audits = Default + ".Audits";
        }
    }
}

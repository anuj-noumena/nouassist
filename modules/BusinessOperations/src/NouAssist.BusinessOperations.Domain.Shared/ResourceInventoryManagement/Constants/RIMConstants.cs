namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Constants
{
    /// <summary>
    /// Contains constant values used throughout the Resource and Inventory Management module.
    /// </summary>
    public static class RIMConstants
    {
        /// <summary>
        /// The name of the RIM module
        /// </summary>
        public const string ModuleName = "RIM";

        /// <summary>
        /// Constants related to resource types
        /// </summary>
        public static class ResourceTypes
        {
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 512;
            public const int MaxCodeLength = 32;
        }

        /// <summary>
        /// Constants related to resource instances
        /// </summary>
        public static class ResourceInstances
        {
            public const int MaxIdentifierLength = 64;
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 512;
        }

        /// <summary>
        /// Constants related to inventory items
        /// </summary>
        public static class InventoryItems
        {
            public const int MaxSkuLength = 32;
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 512;
            public const int MaxBarcodeLength = 64;
        }

        /// <summary>
        /// Constants related to storage locations
        /// </summary>
        public static class StorageLocations
        {
            public const int MaxCodeLength = 32;
            public const int MaxNameLength = 128;
            public const int MaxDescriptionLength = 512;
        }

        /// <summary>
        /// Constants related to resource bookings
        /// </summary>
        public static class ResourceBookings
        {
            public const int MaxBookingCodeLength = 32;
            public const int MaxPurposeLength = 256;
        }

        /// <summary>
        /// Constants related to attribute definitions
        /// </summary>
        public static class Attributes
        {
            public const int MaxNameLength = 64;
            public const int MaxDescriptionLength = 256;
            public const int MaxValueLength = 512;
        }
    }
}

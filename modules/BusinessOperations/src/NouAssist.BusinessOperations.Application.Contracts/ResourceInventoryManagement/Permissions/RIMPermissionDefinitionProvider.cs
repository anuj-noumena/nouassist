using NouAssist.BusinessOperations.ResourceInventoryManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Permissions
{
    /// <summary>
    /// Permission definition provider for the RIM module
    /// </summary>
    public class RIMPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        /// <summary>
        /// Defines permissions for the RIM module
        /// </summary>
        /// <param name="context">Permission definition context</param>
        public override void Define(IPermissionDefinitionContext context)
        {
            var rimGroup = context.AddGroup(RIMPermissions.GroupName, L("Permission:RIM"));

            // Resource Type permissions
            var resourceTypePermission = rimGroup.AddPermission(
                RIMPermissions.ResourceType.Default,
                L("Permission:ResourceType"));
            
            resourceTypePermission.AddChild(
                RIMPermissions.ResourceType.Create,
                L("Permission:Create"));
            
            resourceTypePermission.AddChild(
                RIMPermissions.ResourceType.Update,
                L("Permission:Update"));
            
            resourceTypePermission.AddChild(
                RIMPermissions.ResourceType.Delete,
                L("Permission:Delete"));
            
            resourceTypePermission.AddChild(
                RIMPermissions.ResourceType.View,
                L("Permission:View"));
            
            resourceTypePermission.AddChild(
                RIMPermissions.ResourceType.ManageAttributes,
                L("Permission:ManageAttributes"));

            // Resource Instance permissions
            var resourceInstancePermission = rimGroup.AddPermission(
                RIMPermissions.ResourceInstance.Default,
                L("Permission:ResourceInstance"));
            
            resourceInstancePermission.AddChild(
                RIMPermissions.ResourceInstance.Create,
                L("Permission:Create"));
            
            resourceInstancePermission.AddChild(
                RIMPermissions.ResourceInstance.Update,
                L("Permission:Update"));
            
            resourceInstancePermission.AddChild(
                RIMPermissions.ResourceInstance.Delete,
                L("Permission:Delete"));
            
            resourceInstancePermission.AddChild(
                RIMPermissions.ResourceInstance.View,
                L("Permission:View"));
            
            resourceInstancePermission.AddChild(
                RIMPermissions.ResourceInstance.Allocate,
                L("Permission:Allocate"));
            
            resourceInstancePermission.AddChild(
                RIMPermissions.ResourceInstance.LogUsage,
                L("Permission:LogUsage"));

            // Inventory Item permissions
            var inventoryItemPermission = rimGroup.AddPermission(
                RIMPermissions.InventoryItem.Default,
                L("Permission:InventoryItem"));
            
            inventoryItemPermission.AddChild(
                RIMPermissions.InventoryItem.Create,
                L("Permission:Create"));
            
            inventoryItemPermission.AddChild(
                RIMPermissions.InventoryItem.Update,
                L("Permission:Update"));
            
            inventoryItemPermission.AddChild(
                RIMPermissions.InventoryItem.Delete,
                L("Permission:Delete"));
            
            inventoryItemPermission.AddChild(
                RIMPermissions.InventoryItem.View,
                L("Permission:View"));
            
            inventoryItemPermission.AddChild(
                RIMPermissions.InventoryItem.AdjustStock,
                L("Permission:AdjustStock"));
            
            inventoryItemPermission.AddChild(
                RIMPermissions.InventoryItem.Transfer,
                L("Permission:Transfer"));

            // Resource Booking permissions
            var resourceBookingPermission = rimGroup.AddPermission(
                RIMPermissions.ResourceBooking.Default,
                L("Permission:ResourceBooking"));
            
            resourceBookingPermission.AddChild(
                RIMPermissions.ResourceBooking.Create,
                L("Permission:Create"));
            
            resourceBookingPermission.AddChild(
                RIMPermissions.ResourceBooking.Update,
                L("Permission:Update"));
            
            resourceBookingPermission.AddChild(
                RIMPermissions.ResourceBooking.Delete,
                L("Permission:Delete"));
            
            resourceBookingPermission.AddChild(
                RIMPermissions.ResourceBooking.View,
                L("Permission:View"));
            
            resourceBookingPermission.AddChild(
                RIMPermissions.ResourceBooking.Approve,
                L("Permission:Approve"));
            
            resourceBookingPermission.AddChild(
                RIMPermissions.ResourceBooking.CheckInOut,
                L("Permission:CheckInOut"));

            // Storage Location permissions
            var storageLocationPermission = rimGroup.AddPermission(
                RIMPermissions.StorageLocation.Default,
                L("Permission:StorageLocation"));
            
            storageLocationPermission.AddChild(
                RIMPermissions.StorageLocation.Create,
                L("Permission:Create"));
            
            storageLocationPermission.AddChild(
                RIMPermissions.StorageLocation.Update,
                L("Permission:Update"));
            
            storageLocationPermission.AddChild(
                RIMPermissions.StorageLocation.Delete,
                L("Permission:Delete"));
            
            storageLocationPermission.AddChild(
                RIMPermissions.StorageLocation.View,
                L("Permission:View"));
            
            storageLocationPermission.AddChild(
                RIMPermissions.StorageLocation.ManageHierarchy,
                L("Permission:ManageHierarchy"));

            // Inventory Audit permissions
            var inventoryAuditPermission = rimGroup.AddPermission(
                RIMPermissions.InventoryAudit.Default,
                L("Permission:InventoryAudit"));
            
            inventoryAuditPermission.AddChild(
                RIMPermissions.InventoryAudit.Create,
                L("Permission:Create"));
            
            inventoryAuditPermission.AddChild(
                RIMPermissions.InventoryAudit.Update,
                L("Permission:Update"));
            
            inventoryAuditPermission.AddChild(
                RIMPermissions.InventoryAudit.Delete,
                L("Permission:Delete"));
            
            inventoryAuditPermission.AddChild(
                RIMPermissions.InventoryAudit.View,
                L("Permission:View"));
            
            inventoryAuditPermission.AddChild(
                RIMPermissions.InventoryAudit.RecordDiscrepancies,
                L("Permission:RecordDiscrepancies"));
            
            inventoryAuditPermission.AddChild(
                RIMPermissions.InventoryAudit.Reconcile,
                L("Permission:Reconcile"));

            // Reports and Analytics permissions
            var reportsPermission = rimGroup.AddPermission(
                RIMPermissions.Reports.Default,
                L("Permission:Reports"));
            
            reportsPermission.AddChild(
                RIMPermissions.Reports.ResourceUtilization,
                L("Permission:ResourceUtilization"));
            
            reportsPermission.AddChild(
                RIMPermissions.Reports.Inventory,
                L("Permission:InventoryReports"));
            
            reportsPermission.AddChild(
                RIMPermissions.Reports.Bookings,
                L("Permission:BookingReports"));
            
            reportsPermission.AddChild(
                RIMPermissions.Reports.Audits,
                L("Permission:AuditReports"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<RIMResource>(name);
        }
    }
}

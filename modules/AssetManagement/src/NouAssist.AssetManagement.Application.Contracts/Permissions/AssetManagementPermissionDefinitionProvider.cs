using NouAssist.AssetManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.AssetManagement.Permissions;

public class AssetManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var assetManagementGroup = context.AddGroup(AssetManagementPermissions.GroupName, L("Permission:AssetManagement"));

        // Resources Management Permissions
        var resourcesPermission = assetManagementGroup.AddPermission(
            AssetManagementPermissions.Resources.Resource.Default,
            L("Permission:Resources.Resources"));
        resourcesPermission.AddChild(
            AssetManagementPermissions.Resources.Resource.Upload,
            L("Permission:Resources.Resources.Upload"));
        resourcesPermission.AddChild(
            AssetManagementPermissions.Resources.Resource.Download,
            L("Permission:Resources.Resources.Download"));
        resourcesPermission.AddChild(
            AssetManagementPermissions.Resources.Resource.Update,
            L("Permission:Resources.Resources.Update"));
        resourcesPermission.AddChild(
            AssetManagementPermissions.Resources.Resource.Delete,
            L("Permission:Resources.Resources.Delete"));
        resourcesPermission.AddChild(
            AssetManagementPermissions.Resources.Resource.ManageVersions,
            L("Permission:Resources.Resources.ManageVersions"));
        resourcesPermission.AddChild(
            AssetManagementPermissions.Resources.Resource.ManagePermissions,
            L("Permission:Resources.Resources.ManagePermissions"));

        // Books Permissions
        var booksPermission = assetManagementGroup.AddPermission(
            AssetManagementPermissions.Resources.Books.Default,
            L("Permission:Resources.Books"));
        booksPermission.AddChild(
            AssetManagementPermissions.Resources.Books.Create,
            L("Permission:Resources.Books.Create"));
        booksPermission.AddChild(
            AssetManagementPermissions.Resources.Books.Update,
            L("Permission:Resources.Books.Update"));
        booksPermission.AddChild(
            AssetManagementPermissions.Resources.Books.Delete,
            L("Permission:Resources.Books.Delete"));
        booksPermission.AddChild(
            AssetManagementPermissions.Resources.Books.UpdateReadingProgress,
            L("Permission:Resources.Books.UpdateReadingProgress"));
        booksPermission.AddChild(
            AssetManagementPermissions.Resources.Books.Rate,
            L("Permission:Resources.Books.Rate"));

        // Collections Permissions
        var collectionsPermission = assetManagementGroup.AddPermission(
            AssetManagementPermissions.Resources.Collections.Default,
            L("Permission:Resources.Collections"));
        collectionsPermission.AddChild(
            AssetManagementPermissions.Resources.Collections.Create,
            L("Permission:Resources.Collections.Create"));
        collectionsPermission.AddChild(
            AssetManagementPermissions.Resources.Collections.Update,
            L("Permission:Resources.Collections.Update"));
        collectionsPermission.AddChild(
            AssetManagementPermissions.Resources.Collections.Delete,
            L("Permission:Resources.Collections.Delete"));

        // Inventory Permissions
        var inventoryPermission = assetManagementGroup.AddPermission(
            AssetManagementPermissions.Resources.Inventory.Default,
            L("Permission:Resources.Inventory"));
        inventoryPermission.AddChild(
            AssetManagementPermissions.Resources.Inventory.Create,
            L("Permission:Resources.Inventory.Create"));
        inventoryPermission.AddChild(
            AssetManagementPermissions.Resources.Inventory.Update,
            L("Permission:Resources.Inventory.Update"));
        inventoryPermission.AddChild(
            AssetManagementPermissions.Resources.Inventory.Delete,
            L("Permission:Resources.Inventory.Delete"));
        inventoryPermission.AddChild(
            AssetManagementPermissions.Resources.Inventory.CheckOut,
            L("Permission:Resources.Inventory.CheckOut"));
        inventoryPermission.AddChild(
            AssetManagementPermissions.Resources.Inventory.CheckIn,
            L("Permission:Resources.Inventory.CheckIn"));
        inventoryPermission.AddChild(
            AssetManagementPermissions.Resources.Inventory.ScheduleMaintenance,
            L("Permission:Resources.Inventory.ScheduleMaintenance"));
        inventoryPermission.AddChild(
            AssetManagementPermissions.Resources.Inventory.ViewAll,
            L("Permission:Resources.Inventory.ViewAll"));

        // Storage Permissions
        var storagePermission = assetManagementGroup.AddPermission(
            AssetManagementPermissions.Resources.Storage.Default,
            L("Permission:Resources.Storage"));
        storagePermission.AddChild(
            AssetManagementPermissions.Resources.Storage.Manage,
            L("Permission:Resources.Storage.Manage"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AssetManagementResource>(name);
    }
}

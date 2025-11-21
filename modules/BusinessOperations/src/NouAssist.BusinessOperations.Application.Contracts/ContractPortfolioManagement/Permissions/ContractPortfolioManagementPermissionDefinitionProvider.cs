using NouAssist.BusinessOperations.ContractPortfolioManagement.Localization;
using NouAssist.ContractPortfolioManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ContractPortfolioManagement.Permissions;

/// <summary>
/// Permission definition provider for Contract Portfolio Management module
/// </summary>
public class ContractPortfolioManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        // Define main module group
        var contractPortfolioGroup = context.AddGroup(
            ContractPortfolioManagementPermissions.GroupName,
            L("Permission:ContractPortfolioManagement"));

        // Contract permissions
        var contractPermission = contractPortfolioGroup.AddPermission(
            ContractPortfolioManagementPermissions.Contract.Default,
            L("Permission:Contracts"));

        contractPermission.AddChild(
            ContractPortfolioManagementPermissions.Contract.Create,
            L("Permission:Contracts.Create"));
        contractPermission.AddChild(
            ContractPortfolioManagementPermissions.Contract.Update,
            L("Permission:Contracts.Update"));
        contractPermission.AddChild(
            ContractPortfolioManagementPermissions.Contract.Delete,
            L("Permission:Contracts.Delete"));
        contractPermission.AddChild(
            ContractPortfolioManagementPermissions.Contract.View,
            L("Permission:Contracts.View"));
        contractPermission.AddChild(
            ContractPortfolioManagementPermissions.Contract.Manage,
            L("Permission:Contracts.Manage"));
        contractPermission.AddChild(
            ContractPortfolioManagementPermissions.Contract.ChangeStatus,
            L("Permission:Contracts.ChangeStatus"));
        contractPermission.AddChild(
            ContractPortfolioManagementPermissions.Contract.CreateVersion,
            L("Permission:Contracts.CreateVersion"));
        contractPermission.AddChild(
            ContractPortfolioManagementPermissions.Contract.AddAmendment,
            L("Permission:Contracts.AddAmendment"));

        // Portfolio permissions
        var portfolioPermission = contractPortfolioGroup.AddPermission(
            ContractPortfolioManagementPermissions.Portfolio.Default,
            L("Permission:Portfolios"));

        portfolioPermission.AddChild(
            ContractPortfolioManagementPermissions.Portfolio.Create,
            L("Permission:Portfolios.Create"));
        portfolioPermission.AddChild(
            ContractPortfolioManagementPermissions.Portfolio.Update,
            L("Permission:Portfolios.Update"));
        portfolioPermission.AddChild(
            ContractPortfolioManagementPermissions.Portfolio.Delete,
            L("Permission:Portfolios.Delete"));
        portfolioPermission.AddChild(
            ContractPortfolioManagementPermissions.Portfolio.View,
            L("Permission:Portfolios.View"));
        portfolioPermission.AddChild(
            ContractPortfolioManagementPermissions.Portfolio.Manage,
            L("Permission:Portfolios.Manage"));
        portfolioPermission.AddChild(
            ContractPortfolioManagementPermissions.Portfolio.AddItem,
            L("Permission:Portfolios.AddItem"));
        portfolioPermission.AddChild(
            ContractPortfolioManagementPermissions.Portfolio.RemoveItem,
            L("Permission:Portfolios.RemoveItem"));
        portfolioPermission.AddChild(
            ContractPortfolioManagementPermissions.Portfolio.Share,
            L("Permission:Portfolios.Share"));

        // Portfolio Roadmap permissions
        var roadmapPermission = contractPortfolioGroup.AddPermission(
            ContractPortfolioManagementPermissions.PortfolioRoadmap.Default,
            L("Permission:PortfolioRoadmaps"));

        roadmapPermission.AddChild(
            ContractPortfolioManagementPermissions.PortfolioRoadmap.Create,
            L("Permission:PortfolioRoadmaps.Create"));
        roadmapPermission.AddChild(
            ContractPortfolioManagementPermissions.PortfolioRoadmap.Update,
            L("Permission:PortfolioRoadmaps.Update"));
        roadmapPermission.AddChild(
            ContractPortfolioManagementPermissions.PortfolioRoadmap.Delete,
            L("Permission:PortfolioRoadmaps.Delete"));
        roadmapPermission.AddChild(
            ContractPortfolioManagementPermissions.PortfolioRoadmap.View,
            L("Permission:PortfolioRoadmaps.View"));
        roadmapPermission.AddChild(
            ContractPortfolioManagementPermissions.PortfolioRoadmap.Manage,
            L("Permission:PortfolioRoadmaps.Manage"));
        roadmapPermission.AddChild(
            ContractPortfolioManagementPermissions.PortfolioRoadmap.AddMilestone,
            L("Permission:PortfolioRoadmaps.AddMilestone"));
        roadmapPermission.AddChild(
            ContractPortfolioManagementPermissions.PortfolioRoadmap.ManageDependencies,
            L("Permission:PortfolioRoadmaps.ManageDependencies"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ContractPortfolioManagementResource>(name);
    }
}

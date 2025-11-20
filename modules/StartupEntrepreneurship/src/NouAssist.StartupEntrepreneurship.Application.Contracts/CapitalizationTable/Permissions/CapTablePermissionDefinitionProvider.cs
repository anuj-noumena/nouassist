using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Localization;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Permissions
{
    /// <summary>
    /// Permission definition provider for CapTable module
    /// </summary>
    public class CapTablePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // Define a top-level group for the module
            var capTableGroup = context.AddGroup(CapTablePermissions.GroupName, L("Permission:CapTable"));

            // Define permissions for CapTables
            var capTablesPermission = capTableGroup.AddPermission(
                CapTablePermissions.CapTables.Default, L("Permission:CapTables"));
            capTablesPermission.AddChild(
                CapTablePermissions.CapTables.Create, L("Permission:CapTables.Create"));
            capTablesPermission.AddChild(
                CapTablePermissions.CapTables.Update, L("Permission:CapTables.Update"));
            capTablesPermission.AddChild(
                CapTablePermissions.CapTables.Delete, L("Permission:CapTables.Delete"));
            capTablesPermission.AddChild(
                CapTablePermissions.CapTables.View, L("Permission:CapTables.View"));
            capTablesPermission.AddChild(
                CapTablePermissions.CapTables.ManageValuation, L("Permission:CapTables.ManageValuation"));

            // Define permissions for SecurityHolders
            var securityHoldersPermission = capTableGroup.AddPermission(
                CapTablePermissions.SecurityHolders.Default, L("Permission:SecurityHolders"));
            securityHoldersPermission.AddChild(
                CapTablePermissions.SecurityHolders.Create, L("Permission:SecurityHolders.Create"));
            securityHoldersPermission.AddChild(
                CapTablePermissions.SecurityHolders.Update, L("Permission:SecurityHolders.Update"));
            securityHoldersPermission.AddChild(
                CapTablePermissions.SecurityHolders.Delete, L("Permission:SecurityHolders.Delete"));
            securityHoldersPermission.AddChild(
                CapTablePermissions.SecurityHolders.View, L("Permission:SecurityHolders.View"));
            securityHoldersPermission.AddChild(
                CapTablePermissions.SecurityHolders.ManageVesting, L("Permission:SecurityHolders.ManageVesting"));

            // Define permissions for SecurityIssuances
            var securityIssuancesPermission = capTableGroup.AddPermission(
                CapTablePermissions.SecurityIssuances.Default, L("Permission:SecurityIssuances"));
            securityIssuancesPermission.AddChild(
                CapTablePermissions.SecurityIssuances.Create, L("Permission:SecurityIssuances.Create"));
            securityIssuancesPermission.AddChild(
                CapTablePermissions.SecurityIssuances.Update, L("Permission:SecurityIssuances.Update"));
            securityIssuancesPermission.AddChild(
                CapTablePermissions.SecurityIssuances.Delete, L("Permission:SecurityIssuances.Delete"));
            securityIssuancesPermission.AddChild(
                CapTablePermissions.SecurityIssuances.View, L("Permission:SecurityIssuances.View"));
            securityIssuancesPermission.AddChild(
                CapTablePermissions.SecurityIssuances.IssueSecurities, L("Permission:SecurityIssuances.IssueSecurities"));

            // Define permissions for FundingRounds
            var fundingRoundsPermission = capTableGroup.AddPermission(
                CapTablePermissions.FundingRounds.Default, L("Permission:FundingRounds"));
            fundingRoundsPermission.AddChild(
                CapTablePermissions.FundingRounds.Create, L("Permission:FundingRounds.Create"));
            fundingRoundsPermission.AddChild(
                CapTablePermissions.FundingRounds.Update, L("Permission:FundingRounds.Update"));
            fundingRoundsPermission.AddChild(
                CapTablePermissions.FundingRounds.Delete, L("Permission:FundingRounds.Delete"));
            fundingRoundsPermission.AddChild(
                CapTablePermissions.FundingRounds.View, L("Permission:FundingRounds.View"));
            fundingRoundsPermission.AddChild(
                CapTablePermissions.FundingRounds.Close, L("Permission:FundingRounds.Close"));

            // Define permissions for OptionPools
            var optionPoolsPermission = capTableGroup.AddPermission(
                CapTablePermissions.OptionPools.Default, L("Permission:OptionPools"));
            optionPoolsPermission.AddChild(
                CapTablePermissions.OptionPools.Create, L("Permission:OptionPools.Create"));
            optionPoolsPermission.AddChild(
                CapTablePermissions.OptionPools.Update, L("Permission:OptionPools.Update"));
            optionPoolsPermission.AddChild(
                CapTablePermissions.OptionPools.Delete, L("Permission:OptionPools.Delete"));
            optionPoolsPermission.AddChild(
                CapTablePermissions.OptionPools.View, L("Permission:OptionPools.View"));
            optionPoolsPermission.AddChild(
                CapTablePermissions.OptionPools.GrantOptions, L("Permission:OptionPools.GrantOptions"));

            // Define permissions for Reports
            var reportsPermission = capTableGroup.AddPermission(
                CapTablePermissions.Reports.Default, L("Permission:Reports"));
            reportsPermission.AddChild(
                CapTablePermissions.Reports.ViewOwnership, L("Permission:Reports.ViewOwnership"));
            reportsPermission.AddChild(
                CapTablePermissions.Reports.ViewValuation, L("Permission:Reports.ViewValuation"));
            reportsPermission.AddChild(
                CapTablePermissions.Reports.ViewDilution, L("Permission:Reports.ViewDilution"));
            reportsPermission.AddChild(
                CapTablePermissions.Reports.ViewScenarios, L("Permission:Reports.ViewScenarios"));
        }

        // Helper to create LocalizableString using the CapTable resource
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CapTableResource>(name);
        }
    }
}

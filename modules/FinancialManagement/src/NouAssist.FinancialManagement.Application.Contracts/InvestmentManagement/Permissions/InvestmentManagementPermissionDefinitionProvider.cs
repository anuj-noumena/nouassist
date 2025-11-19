using NouAssist.FinancialManagement.InvestmentManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.FinancialManagement.InvestmentManagement.Permissions
{
    public class InvestmentManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var investmentGroup = context.AddGroup(
                InvestmentManagementPermissions.GroupName,
                L("Permission:InvestmentManagement"));

            var portfolios = investmentGroup.AddPermission(
                InvestmentManagementPermissions.Portfolios.Default,
                L("Permission:Portfolios"));
            portfolios.AddChild(InvestmentManagementPermissions.Portfolios.Create, L("Permission:Create"));
            portfolios.AddChild(InvestmentManagementPermissions.Portfolios.Update, L("Permission:Update"));
            portfolios.AddChild(InvestmentManagementPermissions.Portfolios.Delete, L("Permission:Delete"));
            portfolios.AddChild(InvestmentManagementPermissions.Portfolios.Trade, L("Permission:Trade"));
            portfolios.AddChild(InvestmentManagementPermissions.Portfolios.Rebalance, L("Permission:Rebalance"));

            var goals = investmentGroup.AddPermission(
                InvestmentManagementPermissions.InvestmentGoals.Default,
                L("Permission:InvestmentGoals"));
            goals.AddChild(InvestmentManagementPermissions.InvestmentGoals.Create, L("Permission:Create"));
            goals.AddChild(InvestmentManagementPermissions.InvestmentGoals.Update, L("Permission:Update"));
            goals.AddChild(InvestmentManagementPermissions.InvestmentGoals.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<InvestmentManagementResource>(name);
        }
    }
}

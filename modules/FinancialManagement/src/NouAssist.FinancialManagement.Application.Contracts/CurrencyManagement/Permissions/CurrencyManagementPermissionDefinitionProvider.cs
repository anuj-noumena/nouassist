using NouAssist.FinancialManagement.CurrencyManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.FinancialManagement.CurrencyManagement.Permissions
{
    public class CurrencyManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var currencyGroup = context.AddGroup(
                CurrencyManagementPermissions.GroupName,
                L("Permission:CurrencyManagement"));

            var currenciesPermission = currencyGroup.AddPermission(
                CurrencyManagementPermissions.Currencies.Default,
                L("Permission:Currencies"));

            currenciesPermission.AddChild(
                CurrencyManagementPermissions.Currencies.View,
                L("Permission:Currencies.View"));

            currenciesPermission.AddChild(
                CurrencyManagementPermissions.Currencies.Create,
                L("Permission:Currencies.Create"));

            currenciesPermission.AddChild(
                CurrencyManagementPermissions.Currencies.Update,
                L("Permission:Currencies.Update"));

            currenciesPermission.AddChild(
                CurrencyManagementPermissions.Currencies.Activate,
                L("Permission:Currencies.Activate"));

            currenciesPermission.AddChild(
                CurrencyManagementPermissions.Currencies.Deactivate,
                L("Permission:Currencies.Deactivate"));

            var exchangeRatesPermission = currencyGroup.AddPermission(
                CurrencyManagementPermissions.ExchangeRates.Default,
                L("Permission:ExchangeRates"));

            exchangeRatesPermission.AddChild(
                CurrencyManagementPermissions.ExchangeRates.View,
                L("Permission:ExchangeRates.View"));

            exchangeRatesPermission.AddChild(
                CurrencyManagementPermissions.ExchangeRates.Create,
                L("Permission:ExchangeRates.Create"));

            exchangeRatesPermission.AddChild(
                CurrencyManagementPermissions.ExchangeRates.Update,
                L("Permission:ExchangeRates.Update"));

            exchangeRatesPermission.AddChild(
                CurrencyManagementPermissions.ExchangeRates.Sync,
                L("Permission:ExchangeRates.Sync"));

            exchangeRatesPermission.AddChild(
                CurrencyManagementPermissions.ExchangeRates.Convert,
                L("Permission:ExchangeRates.Convert"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CurrencyManagementResource>(name);
        }
    }
}

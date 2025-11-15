namespace NouAssist.CurrencyManagement.Permissions
{
    public static class CurrencyManagementPermissions
    {
        public const string GroupName = "CurrencyManagement";

        public static class Currencies
        {
            public const string Default = GroupName + ".Currencies";
            public const string View = Default + ".View";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Activate = Default + ".Activate";
            public const string Deactivate = Default + ".Deactivate";
        }

        public static class ExchangeRates
        {
            public const string Default = GroupName + ".ExchangeRates";
            public const string View = Default + ".View";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Sync = Default + ".Sync";
            public const string Convert = Default + ".Convert";
        }
    }
}

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants
{
    /// <summary>
    /// Constants for the CapTable module
    /// </summary>
    public static class CapTableConsts
    {
        public const int MaxNameLength = 128;
        public const int MaxDescriptionLength = 500;
        public const int MaxCompanyNameLength = 256;
        public const int MaxHolderNameLength = 256;
        public const int MaxShareClassNameLength = 128;
        public const int MaxTransactionMemoLength = 1000;
        public const int MaxFundingRoundNameLength = 128;
        public const int MaxTickerSymbolLength = 10;
        
        public const decimal MinShareQuantity = 0;
        public const decimal MaxShareQuantity = 999999999999;
        public const decimal MinPricePerShare = 0.0001m;
        public const decimal MaxPricePerShare = 999999999.99m;
        public const decimal MinOwnershipPercentage = 0;
        public const decimal MaxOwnershipPercentage = 100;

        public static class SecurityTypes
        {
            public const string CommonStock = "CommonStock";
            public const string PreferredStock = "PreferredStock";
            public const string StockOption = "StockOption";
            public const string Warrant = "Warrant";
            public const string ConvertibleNote = "ConvertibleNote";
        }

        public static class HolderTypes
        {
            public const string Founder = "Founder";
            public const string Employee = "Employee";
            public const string Investor = "Investor";
            public const string Advisor = "Advisor";
            public const string Company = "Company";
        }

        public static class TransactionTypes
        {
            public const string IssuanceOfShares = "IssuanceOfShares";
            public const string Transfer = "Transfer";
            public const string OptionGrant = "OptionGrant";
            public const string OptionExercise = "OptionExercise";
            public const string ShareRepurchase = "ShareRepurchase";
        }
    }
}

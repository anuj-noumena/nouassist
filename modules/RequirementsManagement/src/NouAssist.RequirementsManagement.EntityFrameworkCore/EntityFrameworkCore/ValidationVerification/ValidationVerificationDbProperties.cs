namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore
{
    public static class ValidationVerificationDbProperties
    {
        public static string DbTablePrefix { get; set; } = ValidationVerificationConsts.DbTablePrefix;
        public static string DbSchema { get; set; } = ValidationVerificationConsts.DbSchema;
        public const string ConnectionStringName = "ValidationVerification";
    }
}

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore
{
    public static class RequirementSpecificationManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "ReqSpec";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "RequirementSpecificationManagement";
    }
}

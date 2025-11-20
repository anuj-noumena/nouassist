namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore
{
    public static class ElicitationDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Elicitation";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "Elicitation";
    }
}

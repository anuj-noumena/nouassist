namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning
{
    public static class LearningDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Learning";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "Learning";
    }
}

namespace NouAssist.Collaboration.Comments.EntityFrameworkCore
{
    public static class CommentsDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Comments";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "Comments";
    }
}

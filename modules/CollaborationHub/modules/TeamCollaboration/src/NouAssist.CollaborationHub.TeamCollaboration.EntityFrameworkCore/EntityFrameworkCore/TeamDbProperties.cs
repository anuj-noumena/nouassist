namespace NouAssist.CollaborationHub.TeamCollaboration.EntityFrameworkCore
{
    public static class TeamDbProperties
    {
        public const string ConnectionStringName = "Team";
        
        public static string DbTablePrefix { get; set; } = "App";
        
        public static string DbSchema { get; set; } = null;
    }
}

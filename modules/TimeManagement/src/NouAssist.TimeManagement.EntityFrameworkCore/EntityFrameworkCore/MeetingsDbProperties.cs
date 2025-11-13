namespace NouAssist.TimeManagement.Meetings.EntityFrameworkCore
{
    public static class MeetingsDbProperties
    {
        public const string ConnectionStringName = "Meetings";
        
        public static string DbTablePrefix { get; set; } = "App";
        
        public static string DbSchema { get; set; } = null;
    }
}

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore
{
    public static class CalendarDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Calendar";
        public static string DbSchema { get; set; } = null;
        public const string ConnectionStringName = "Calendar";
    }
}

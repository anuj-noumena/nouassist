namespace NouAssist.Users;

public static class UsersDbProperties
{
    public static string DbTablePrefix { get; set; } = "Users";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Users";
}

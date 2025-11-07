namespace NouAssist.ReflectionAndSupport.Reviews;

public static class ReviewsDbProperties
{
    public static string DbTablePrefix { get; set; } = "Reviews";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Reviews";
}

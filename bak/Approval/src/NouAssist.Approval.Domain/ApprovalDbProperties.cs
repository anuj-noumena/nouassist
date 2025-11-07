namespace NouAssist.Approval;

public static class ApprovalDbProperties
{
    public static string DbTablePrefix { get; set; } = "Approval";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Approval";
}

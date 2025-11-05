namespace NouAssist.SharedKernel;

public static class SharedKernelDbProperties
{
    public static string DbTablePrefix { get; set; } = "SharedKernel";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "SharedKernel";
}

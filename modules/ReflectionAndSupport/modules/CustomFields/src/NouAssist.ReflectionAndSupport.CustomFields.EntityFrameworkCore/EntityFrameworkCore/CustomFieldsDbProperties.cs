namespace NouAssist.ReflectionAndSupport.CustomFields.EntityFrameworkCore
{
    public static class CustomFieldsDbProperties
    {
        public const string ConnectionStringName = "CustomFields";
        
        public static string DbTablePrefix { get; set; } = "App";
        
        public static string DbSchema { get; set; } = null;
    }
}

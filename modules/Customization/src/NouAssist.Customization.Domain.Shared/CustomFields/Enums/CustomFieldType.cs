namespace NouAssist.Customization.CustomFields.Enums
{
    public enum CustomFieldType : byte
    {
        Text = 1,
        Number = 2,
        Decimal = 3,
        Boolean = 4,
        Date = 5,
        DateTime = 6,
        Select = 7,
        MultiSelect = 8,
        Url = 9,
        Email = 10,
        Phone = 11,
        TextArea = 12,
        RichText = 13,
        File = 14,
        User = 15,
        MultiUser = 16,
        Rating = 17,
        Currency = 18,
        Percentage = 19,
        Duration = 20,
        Color = 21
    }

    public static class CustomFieldTypeExtensions
    {
        public static bool SupportsOptions(this CustomFieldType type)
        {
            return type == CustomFieldType.Select || 
                   type == CustomFieldType.MultiSelect;
        }

        public static bool IsNumeric(this CustomFieldType type)
        {
            return type == CustomFieldType.Number ||
                   type == CustomFieldType.Decimal ||
                   type == CustomFieldType.Currency ||
                   type == CustomFieldType.Percentage ||
                   type == CustomFieldType.Rating;
        }

        public static bool IsText(this CustomFieldType type)
        {
            return type == CustomFieldType.Text ||
                   type == CustomFieldType.TextArea ||
                   type == CustomFieldType.RichText ||
                   type == CustomFieldType.Email ||
                   type == CustomFieldType.Url ||
                   type == CustomFieldType.Phone;
        }

        public static bool IsDate(this CustomFieldType type)
        {
            return type == CustomFieldType.Date || 
                   type == CustomFieldType.DateTime;
        }
    }
}

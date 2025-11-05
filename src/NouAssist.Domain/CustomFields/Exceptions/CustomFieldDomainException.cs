using System;
using Volo.Abp;

namespace NouAssist.CustomFields.Exceptions
{
    public class CustomFieldException : BusinessException
    {
        public CustomFieldException(string message, string code = null) 
            : base(code ?? "CustomField:Error", message)
        {
        }
    }

    public class CustomFieldValidationException : BusinessException
    {
        public CustomFieldValidationException(string fieldName, string validationMessage) 
            : base("CustomField:ValidationFailed", 
                  $"Validation failed for field '{fieldName}': {validationMessage}")
        {
            WithData("FieldName", fieldName);
            WithData("ValidationMessage", validationMessage);
        }
    }
}

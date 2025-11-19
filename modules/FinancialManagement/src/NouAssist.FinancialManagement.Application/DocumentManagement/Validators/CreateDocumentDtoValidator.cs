using NouAssist.FinancialManagement.DocumentManagement.Dtos;

namespace NouAssist.FinancialManagement.DocumentManagement.Validators
{
    public class CreateDocumentDtoValidator : AbstractValidator<CreateDocumentDto>
    {
        private const long MaxFileSize = 52428800; // 50 MB

        public CreateDocumentDtoValidator()
        {
            RuleFor(x => x.FileContent)
                .NotNull()
                .Must(content => content != null && content.Length > 0 && content.Length <= MaxFileSize)
                .WithMessage($"File size must be between 1 byte and {MaxFileSize / 1024 / 1024} MB");

            RuleFor(x => x.FileName)
                .NotEmpty()
                .MaximumLength(512);

            RuleFor(x => x.ContentType)
                .NotEmpty()
                .MaximumLength(256)
                .Must(BeValidContentType)
                .WithMessage("Unsupported file type");

            RuleFor(x => x.Description)
                .MaximumLength(2048);

            RuleFor(x => x)
                .Must(x => string.IsNullOrEmpty(x.EntityType) == string.IsNullOrEmpty(x.EntityId))
                .WithMessage("EntityType and EntityId must both be provided or both be null");
        }

        private bool BeValidContentType(string contentType)
        {
            var allowedTypes = new[]
            {
                "application/pdf",
                "image/jpeg",
                "image/png",
                "image/tiff",
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "text/csv"
            };

            return System.Array.Exists(allowedTypes, t => t.Equals(contentType, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}

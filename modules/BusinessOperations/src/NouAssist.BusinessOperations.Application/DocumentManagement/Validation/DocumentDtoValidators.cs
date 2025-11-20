using System;
using FluentValidation;
using NouAssist.BusinessOperations.DocumentManagement.Constants;
using NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos;
using NouAssist.BusinessOperations.DocumentManagement.Enums;

namespace NouAssist.BusinessOperations.DocumentManagement.Validation
{
    public class UploadDocumentDtoValidator : AbstractValidator<UploadDocumentDto>
    {
        public UploadDocumentDtoValidator()
        {
            RuleFor(x => x.Filename)
                .NotEmpty()
                .MaximumLength(DocumentManagementConsts.Document.MaxFilenameLength)
                .WithMessage("Filename is required and cannot exceed {MaxLength} characters.");
                
            RuleFor(x => x.MimeType)
                .NotEmpty()
                .MaximumLength(DocumentManagementConsts.Document.MaxMimeTypeLength)
                .WithMessage("MIME type is required and cannot exceed {MaxLength} characters.");
                
            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Description cannot exceed {MaxLength} characters.");
        }
    }
    
    public class CreateDocumentVersionDtoValidator : AbstractValidator<CreateDocumentVersionDto>
    {
        public CreateDocumentVersionDtoValidator()
        {
            RuleFor(x => x.DocumentId)
                .NotEmpty()
                .WithMessage("Document ID is required.");
                
            RuleFor(x => x.Filename)
                .NotEmpty()
                .MaximumLength(DocumentManagementConsts.Document.MaxFilenameLength)
                .WithMessage("Filename is required and cannot exceed {MaxLength} characters.");
                
            RuleFor(x => x.MimeType)
                .NotEmpty()
                .MaximumLength(DocumentManagementConsts.Document.MaxMimeTypeLength)
                .WithMessage("MIME type is required and cannot exceed {MaxLength} characters.");
                
            RuleFor(x => x.ChangeComment)
                .NotEmpty()
                .MaximumLength(500)
                .WithMessage("Change comment is required and cannot exceed {MaxLength} characters.");
        }
    }
    
    public class GrantDocumentAccessDtoValidator : AbstractValidator<GrantDocumentAccessDto>
    {
        public GrantDocumentAccessDtoValidator()
        {
            RuleFor(x => x.DocumentId)
                .NotEmpty()
                .WithMessage("Document ID is required.");
                
            RuleFor(x => x.PrincipalId)
                .NotEmpty()
                .WithMessage("Principal ID is required.");
                
            RuleFor(x => x.PermissionLevel)
                .IsInEnum()
                .NotEqual(AccessPermissionLevel.None)
                .WithMessage("Valid permission level is required.");
        }
    }
    
    public class RevokeDocumentAccessDtoValidator : AbstractValidator<RevokeDocumentAccessDto>
    {
        public RevokeDocumentAccessDtoValidator()
        {
            RuleFor(x => x.DocumentId)
                .NotEmpty()
                .WithMessage("Document ID is required.");
                
            RuleFor(x => x.PrincipalId)
                .NotEmpty()
                .WithMessage("Principal ID is required.");
        }
    }
    
    public class GetDocumentDownloadLinkDtoValidator : AbstractValidator<GetDocumentDownloadLinkDto>
    {
        public GetDocumentDownloadLinkDtoValidator()
        {
            RuleFor(x => x.DocumentId)
                .NotEmpty()
                .WithMessage("Document ID is required.");
                
            RuleFor(x => x.ExpirationMinutes)
                .GreaterThan(0)
                .LessThanOrEqualTo(1440) // 24 hours
                .WithMessage("Expiration time must be between 1 minute and 24 hours.");
        }
    }
}

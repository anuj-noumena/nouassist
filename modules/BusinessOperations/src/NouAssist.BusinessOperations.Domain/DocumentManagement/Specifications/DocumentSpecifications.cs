using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.BusinessOperations.DocumentManagement.Specifications
{
    /// <summary>
    /// Base specification class for Document aggregate
    /// </summary>
    public abstract class DocumentSpecification : Specification<Document>
    {
    }

    /// <summary>
    /// Specification that identifies active documents
    /// </summary>
    public class ActiveDocumentSpecification : DocumentSpecification
    {
        public override Expression<Func<Document, bool>> ToExpression()
        {
            return document => document.Status == DocumentStatus.Active;
        }
    }

    /// <summary>
    /// Specification that identifies documents a user has access to
    /// </summary>
    public class UserAccessDocumentSpecification : DocumentSpecification
    {
        private readonly Guid _userId;
        private readonly AccessPermissionLevel _requiredLevel;

        public UserAccessDocumentSpecification(Guid userId, AccessPermissionLevel requiredLevel)
        {
            _userId = userId;
            _requiredLevel = requiredLevel;
        }

        public override Expression<Func<Document, bool>> ToExpression()
        {
            return document => document.AccessGrants.Any(g => 
                g.PrincipalId == _userId && 
                !g.IsRole && 
                g.PermissionLevel >= _requiredLevel);
        }
    }

    /// <summary>
    /// Specification that identifies documents matching a filename pattern
    /// </summary>
    public class FilenameLikeSpecification : DocumentSpecification
    {
        private readonly string _pattern;

        public FilenameLikeSpecification(string pattern)
        {
            _pattern = pattern;
        }

        public override Expression<Func<Document, bool>> ToExpression()
        {
            return document => document.Metadata.Filename.Contains(_pattern);
        }
    }

    /// <summary>
    /// Specification that identifies documents with a specific mime type
    /// </summary>
    public class MimeTypeSpecification : DocumentSpecification
    {
        private readonly string _mimeType;

        public MimeTypeSpecification(string mimeType)
        {
            _mimeType = mimeType;
        }

        public override Expression<Func<Document, bool>> ToExpression()
        {
            return document => document.Metadata.MimeType == _mimeType;
        }
    }

    /// <summary>
    /// Specification that identifies documents uploaded by a specific user
    /// </summary>
    public class UploadedBySpecification : DocumentSpecification
    {
        private readonly Guid _userId;

        public UploadedBySpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<Document, bool>> ToExpression()
        {
            return document => document.Metadata.UploadedByUserId == _userId;
        }
    }

    /// <summary>
    /// Specification that identifies documents uploaded within a date range
    /// </summary>
    public class UploadDateRangeSpecification : DocumentSpecification
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public UploadDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<Document, bool>> ToExpression()
        {
            return document => document.Metadata.UploadedAt >= _startDate &&
                              document.Metadata.UploadedAt <= _endDate;
        }
    }
}

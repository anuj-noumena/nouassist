using System;
using System.Linq.Expressions;
using NouAssist.DocumentManagement.Enums;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.DocumentManagement.Specifications
{
    public class DocumentsByTypeSpecification : Specification<Document>
    {
        private readonly DocumentType _documentType;

        public DocumentsByTypeSpecification(DocumentType documentType)
        {
            _documentType = documentType;
        }

        public override Expression<Func<Document, bool>> ToExpression()
        {
            return doc => doc.Type == _documentType;
        }
    }

    public class DocumentsByStatusSpecification : Specification<Document>
    {
        private readonly DocumentStatus _status;

        public DocumentsByStatusSpecification(DocumentStatus status)
        {
            _status = status;
        }

        public override Expression<Func<Document, bool>> ToExpression()
        {
            return doc => doc.Status == _status;
        }
    }

    public class DocumentsByEntityReferenceSpecification : Specification<Document>
    {
        private readonly string _entityType;
        private readonly string _entityId;

        public DocumentsByEntityReferenceSpecification(string entityType, string entityId = null)
        {
            _entityType = entityType;
            _entityId = entityId;
        }

        public override Expression<Func<Document, bool>> ToExpression()
        {
            if (string.IsNullOrEmpty(_entityId))
            {
                return doc => doc.EntityReference.EntityType == _entityType;
            }

            return doc => doc.EntityReference.EntityType == _entityType &&
                         doc.EntityReference.EntityId == _entityId;
        }
    }

    public class DocumentsWithOCRSpecification : Specification<Document>
    {
        private readonly bool _hasOCR;

        public DocumentsWithOCRSpecification(bool hasOCR)
        {
            _hasOCR = hasOCR;
        }

        public override Expression<Func<Document, bool>> ToExpression()
        {
            return _hasOCR
                ? doc => doc.OCRResult != null
                : doc => doc.OCRResult == null;
        }
    }

    public class DocumentsPendingOCRSpecification : Specification<Document>
    {
        public override Expression<Func<Document, bool>> ToExpression()
        {
            return doc => 
                doc.Status == DocumentStatus.Available &&
                doc.OCRResult == null &&
                (doc.Metadata.ContentType == "application/pdf" ||
                 doc.Metadata.ContentType.StartsWith("image/"));
        }
    }
}

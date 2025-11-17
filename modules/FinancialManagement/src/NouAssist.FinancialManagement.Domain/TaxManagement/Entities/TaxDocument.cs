using System;
using NouAssist.TaxManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.FinancialManagement.TaxManagement.Entities
{
    public class TaxDocument : CreationAuditedEntity<Guid>
    {
        public Guid TaxReturnId { get; private set; }
        public TaxFormType FormType { get; private set; }
        public string DocumentName { get; private set; }
        public Guid FileId { get; private set; }
        public TaxDocumentStatus Status { get; private set; }
        public string? RejectionReason { get; private set; }

        protected TaxDocument() { /* ORM */ }

        public TaxDocument(
            Guid id,
            Guid taxReturnId,
            TaxFormType formType,
            string documentName,
            Guid fileId)
            : base(id)
        {
            TaxReturnId = taxReturnId;
            FormType = formType;
            DocumentName = Check.NotNullOrWhiteSpace(documentName, nameof(documentName));
            FileId = fileId;
            Status = TaxDocumentStatus.Uploaded;
        }

        public void Verify()
        {
            if (Status != TaxDocumentStatus.Uploaded)
            {
                throw new BusinessException("TaxManagement:DocumentNotInUploadedState");
            }
            Status = TaxDocumentStatus.Verified;
        }

        public void Reject(string reason)
        {
            Status = TaxDocumentStatus.Rejected;
            RejectionReason = Check.NotNullOrWhiteSpace(reason, nameof(reason));
        }
    }
}

using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.FinancialPlanning.Entities
{
    public class ExpenseAttachment : Entity<Guid>
    {
        public Guid ExpenseId { get; private set; }
        public Guid DocumentId { get; private set; }
        public string FileName { get; private set; }
        public string FileType { get; private set; }
        public long FileSize { get; private set; }
        public DateTime UploadedAt { get; private set; }

        protected ExpenseAttachment() { }

        public ExpenseAttachment(
            Guid id,
            Guid expenseId,
            Guid documentId,
            string fileName,
            string fileType,
            long fileSize) : base(id)
        {
            ExpenseId = expenseId;
            DocumentId = documentId;
            FileName = Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
            FileType = Check.NotNullOrWhiteSpace(fileType, nameof(fileType));
            FileSize = fileSize;
            UploadedAt = Clock.Now;
        }
    }
}

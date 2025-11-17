using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AuditTrail.ValueObjects
{
    public class RetentionPolicy : ValueObject
    {
        public int RetentionDays { get; init; }
        public DateTime ExpirationDate { get; init; }
        public bool IsArchivable { get; init; }
        public string ArchiveLocation { get; init; }

        protected RetentionPolicy() { }

        public RetentionPolicy(
            int retentionDays, 
            DateTime createdAt,
            bool isArchivable = true,
            string archiveLocation = null)
        {
            if (retentionDays < 1 || retentionDays > 3650) // Max 10 years
            {
                throw new BusinessException("AuditTrail:InvalidRetentionPeriod");
            }

            RetentionDays = retentionDays;
            ExpirationDate = createdAt.AddDays(retentionDays);
            IsArchivable = isArchivable;
            ArchiveLocation = archiveLocation;
        }

        public bool IsExpired(DateTime currentDate)
        {
            return currentDate >= ExpirationDate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return RetentionDays;
            yield return ExpirationDate;
            yield return IsArchivable;
            yield return ArchiveLocation ?? string.Empty;
        }
    }
}

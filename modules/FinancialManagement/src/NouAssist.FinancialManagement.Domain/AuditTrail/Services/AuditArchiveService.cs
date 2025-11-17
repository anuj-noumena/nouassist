using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AuditTrail.Data;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.AuditTrail.Services
{
    public class AuditArchiveService : DomainService
    {
        private readonly IAuditEntryRepository _auditRepository;

        public AuditArchiveService(IAuditEntryRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<int> ArchiveOldEntriesAsync(string archiveLocation)
        {
            var currentDate = Clock.Now;
            var entriesToArchive = await _auditRepository.GetEntriesForArchivalAsync(currentDate);

            foreach (var entry in entriesToArchive)
            {
                entry.Archive(archiveLocation);
            }

            await _auditRepository.UpdateManyAsync(entriesToArchive);
            
            return entriesToArchive.Count;
        }

        public async Task<int> DeleteExpiredEntriesAsync()
        {
            var currentDate = Clock.Now;
            var expiredEntries = await _auditRepository.GetExpiredEntriesAsync(currentDate);

            // Before deletion, ensure they are archived
            var unarchivedExpired = expiredEntries.Where(e => !e.IsArchived).ToList();
            if (unarchivedExpired.Any())
            {
                throw new InvalidOperationException(
                    $"Cannot delete {unarchivedExpired.Count} unarchived expired entries. Archive them first.");
            }

            await _auditRepository.DeleteManyAsync(expiredEntries);
            
            return expiredEntries.Count;
        }
    }
}

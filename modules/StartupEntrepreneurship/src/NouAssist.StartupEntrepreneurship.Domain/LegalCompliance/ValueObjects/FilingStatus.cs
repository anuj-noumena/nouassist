using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects
{
    /// <summary>
    /// Value object representing the filing status of a legal document
    /// </summary>
    public class FilingStatus : ValueObject
    {
        public FilingStatusType Status { get; private set; }
        public DateTime StatusDate { get; private set; }
        public string StatusNotes { get; private set; }

        protected FilingStatus() { }

        public FilingStatus(FilingStatusType status, DateTime statusDate, string statusNotes = null)
        {
            Status = status;
            StatusDate = statusDate;
            StatusNotes = statusNotes?.Length > LegalComplianceConsts.MaxNotesLength
                ? statusNotes.Substring(0, LegalComplianceConsts.MaxNotesLength)
                : statusNotes;
        }

        public FilingStatus UpdateStatus(FilingStatusType newStatus, DateTime statusDate, string notes = null)
        {
            return new FilingStatus(newStatus, statusDate, notes);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Status;
            yield return StatusDate;
            yield return StatusNotes;
        }
    }
}

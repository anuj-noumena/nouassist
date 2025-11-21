using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects
{
    /// <summary>
    /// Value object representing the type of corporate record with its metadata
    /// </summary>
    public class RecordType : ValueObject
    {
        public CorporateRecordType Type { get; private set; }
        public string Description { get; private set; }
        public bool RequiresSignature { get; private set; }
        public bool RequiresNotarization { get; private set; }
        public bool RequiresFiling { get; private set; }

        protected RecordType() { }

        public RecordType(CorporateRecordType type)
        {
            Type = type;
            SetMetadataByType(type);
        }

        private void SetMetadataByType(CorporateRecordType type)
        {
            switch (type)
            {
                case CorporateRecordType.BoardMeetingMinutes:
                    Description = "Minutes from a meeting of the Board of Directors";
                    RequiresSignature = true;
                    RequiresNotarization = false;
                    RequiresFiling = false;
                    break;
                case CorporateRecordType.ShareholderMeetingMinutes:
                    Description = "Minutes from a meeting of the Shareholders";
                    RequiresSignature = true;
                    RequiresNotarization = false;
                    RequiresFiling = false;
                    break;
                case CorporateRecordType.BoardResolution:
                    Description = "Resolution approved by the Board of Directors";
                    RequiresSignature = true;
                    RequiresNotarization = false;
                    RequiresFiling = false;
                    break;
                case CorporateRecordType.ShareholderResolution:
                    Description = "Resolution approved by the Shareholders";
                    RequiresSignature = true;
                    RequiresNotarization = false;
                    RequiresFiling = false;
                    break;
                case CorporateRecordType.CertificateOfIncorporation:
                    Description = "Certificate of Incorporation";
                    RequiresSignature = true;
                    RequiresNotarization = true;
                    RequiresFiling = true;
                    break;
                case CorporateRecordType.Bylaws:
                    Description = "Company Bylaws";
                    RequiresSignature = true;
                    RequiresNotarization = false;
                    RequiresFiling = false;
                    break;
                case CorporateRecordType.AnnualReport:
                    Description = "Annual Report";
                    RequiresSignature = true;
                    RequiresNotarization = false;
                    RequiresFiling = true;
                    break;
                case CorporateRecordType.StockCertificate:
                    Description = "Stock Certificate";
                    RequiresSignature = true;
                    RequiresNotarization = false;
                    RequiresFiling = false;
                    break;
                case CorporateRecordType.EquityIssuance:
                    Description = "Equity Issuance Documentation";
                    RequiresSignature = true;
                    RequiresNotarization = false;
                    RequiresFiling = false;
                    break;
                default:
                    Description = "Other Corporate Record";
                    RequiresSignature = false;
                    RequiresNotarization = false;
                    RequiresFiling = false;
                    break;
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Description;
            yield return RequiresSignature;
            yield return RequiresNotarization;
            yield return RequiresFiling;
        }
    }
}

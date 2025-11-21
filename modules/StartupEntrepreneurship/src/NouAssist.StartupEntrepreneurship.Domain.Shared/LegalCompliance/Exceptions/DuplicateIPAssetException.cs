using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a duplicate IP asset
    /// </summary>
    public class DuplicateIPAssetException : LegalComplianceDomainException
    {
        public string IPIdentifier { get; }
        public IntellectualPropertyType IPType { get; }

        public DuplicateIPAssetException(string ipIdentifier, IntellectualPropertyType ipType)
            : base("LegalCompliance:ErrorMessage:DuplicateIPAsset")
        {
            IPIdentifier = ipIdentifier;
            IPType = ipType;
            WithData("ipIdentifier", ipIdentifier);
            WithData("ipType", ipType);
        }
    }
}

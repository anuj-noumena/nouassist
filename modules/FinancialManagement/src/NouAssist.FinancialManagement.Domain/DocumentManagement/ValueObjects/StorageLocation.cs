using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.DocumentManagement.ValueObjects
{
    public class StorageLocation : ValueObject
    {
        public string Container { get; init; }
        public string BlobPath { get; init; }
        public string FullPath { get; init; }
        public string StorageProvider { get; init; }

        protected StorageLocation() { }

        public StorageLocation(
            string container,
            string blobPath,
            string storageProvider = "AzureBlobStorage")
        {
            Container = Check.NotNullOrWhiteSpace(container, nameof(container));
            BlobPath = Check.NotNullOrWhiteSpace(blobPath, nameof(blobPath));
            StorageProvider = storageProvider;
            FullPath = $"{container}/{blobPath}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Container;
            yield return BlobPath;
            yield return StorageProvider;
        }
    }
}

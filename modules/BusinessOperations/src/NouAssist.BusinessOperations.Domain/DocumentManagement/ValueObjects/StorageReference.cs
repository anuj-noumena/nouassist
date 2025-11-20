using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.DocumentManagement.ValueObjects
{
    /// <summary>
    /// Represents a reference to the physical storage location of a document
    /// </summary>
    public class StorageReference : ValueObject
    {
        public string StorageProvider { get; private set; }
        public string ContainerName { get; private set; }
        public string BlobPath { get; private set; }

        private StorageReference() { }

        public StorageReference(
            string storageProvider,
            string containerName,
            string blobPath)
        {
            StorageProvider = Check.NotNullOrWhiteSpace(storageProvider, nameof(storageProvider));
            ContainerName = Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
            BlobPath = Check.NotNullOrWhiteSpace(blobPath, nameof(blobPath));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StorageProvider;
            yield return ContainerName;
            yield return BlobPath;
        }
    }
}

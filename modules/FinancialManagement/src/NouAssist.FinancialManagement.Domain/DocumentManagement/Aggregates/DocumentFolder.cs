using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.DocumentManagement.Aggregates
{
    public class DocumentFolder : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public Guid? ParentFolderId { get; private set; }
        public string Path { get; private set; }
        public int Level { get; private set; }
        
        private readonly List<Guid> _documentIds = new();
        public IReadOnlyCollection<Guid> DocumentIds => _documentIds.AsReadOnly();

        protected DocumentFolder() { }

        public DocumentFolder(
            Guid id,
            Guid? tenantId,
            string name,
            Guid? parentFolderId = null,
            string parentPath = null,
            string description = null) : base(id)
        {
            TenantId = tenantId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ParentFolderId = parentFolderId;
            Description = description;

            if (parentFolderId.HasValue && !string.IsNullOrEmpty(parentPath))
            {
                Path = $"{parentPath}/{name}";
                Level = parentPath.Count(c => c == '/') + 1;
            }
            else
            {
                Path = name;
                Level = 0;
            }
        }

        public void Rename(string newName)
        {
            Name = Check.NotNullOrWhiteSpace(newName, nameof(newName));
            
            // Update path
            var pathParts = Path.Split('/');
            pathParts[^1] = newName;
            Path = string.Join("/", pathParts);
        }

        public void AddDocument(Guid documentId)
        {
            if (!_documentIds.Contains(documentId))
            {
                _documentIds.Add(documentId);
            }
        }

        public void RemoveDocument(Guid documentId)
        {
            _documentIds.Remove(documentId);
        }

        public void Move(Guid? newParentFolderId, string newParentPath)
        {
            ParentFolderId = newParentFolderId;

            if (newParentFolderId.HasValue && !string.IsNullOrEmpty(newParentPath))
            {
                Path = $"{newParentPath}/{Name}";
                Level = newParentPath.Count(c => c == '/') + 1;
            }
            else
            {
                Path = Name;
                Level = 0;
            }
        }
    }
}

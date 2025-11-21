using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.VersioningBaselining.Constants;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using NouAssist.RequirementsManagement.VersioningBaselining.Events;
using NouAssist.RequirementsManagement.VersioningBaselining.Exceptions;
using NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Aggregates
{
    /// <summary>
    /// Aggregate root representing a baseline definition
    /// </summary>
    public class BaselineDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// ID of the tenant this baseline belongs to (if multi-tenant)
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the baseline
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the baseline
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Status of the baseline
        /// </summary>
        public BaselineStatus Status { get; private set; }
        
        private readonly List<BaselineItem> _items;
        
        /// <summary>
        /// Collection of items included in this baseline
        /// </summary>
        public IReadOnlyCollection<BaselineItem> Items => _items.AsReadOnly();

        protected BaselineDefinition()
        {
            _items = new List<BaselineItem>();
        }

        /// <summary>
        /// Creates a new BaselineDefinition
        /// </summary>
        public static BaselineDefinition Create(
            Guid id,
            string name,
            string description,
            Guid? tenantId)
        {
            return new BaselineDefinition(id, name, description, tenantId);
        }

        private BaselineDefinition(
            Guid id,
            string name,
            string description,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            TenantId = tenantId;
            Status = BaselineStatus.Draft;
            _items = new List<BaselineItem>();
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: VersionControlConsts.MaxBaselineNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = description.IsNullOrWhiteSpace()
                ? null
                : Check.Length(
                    description,
                    nameof(description),
                    maxLength: VersionControlConsts.MaxBaselineDescriptionLength
                );
        }

        /// <summary>
        /// Updates the baseline properties
        /// </summary>
        public void Update(string name, string description)
        {
            EnsureCanBeModified();
            
            SetName(name);
            SetDescription(description);
        }

        /// <summary>
        /// Adds a requirement version to the baseline
        /// </summary>
        public BaselineItem AddItem(Guid requirementId, VersionIdentifier version, DateTime addedAt)
        {
            EnsureCanBeModified();
            
            if (_items.Any(i => i.RequirementId == requirementId))
            {
                throw new BusinessException("VersionControl:ErrorMessage:RequirementAlreadyInBaseline")
                    .WithData("requirementId", requirementId)
                    .WithData("baselineId", Id);
            }
            
            var item = new BaselineItem(Id, requirementId, version, addedAt);
            _items.Add(item);
            
            return item;
        }

        /// <summary>
        /// Removes a requirement from the baseline
        /// </summary>
        public void RemoveItem(Guid requirementId)
        {
            EnsureCanBeModified();
            
            var item = _items.FirstOrDefault(i => i.RequirementId == requirementId);
            if (item == null)
            {
                throw new BusinessException("VersionControl:ErrorMessage:RequirementNotInBaseline")
                    .WithData("requirementId", requirementId)
                    .WithData("baselineId", Id);
            }
            
            _items.Remove(item);
        }

        /// <summary>
        /// Updates a requirement's version in the baseline
        /// </summary>
        public void UpdateItem(Guid requirementId, VersionIdentifier newVersion, DateTime updatedAt)
        {
            EnsureCanBeModified();
            
            var existingItemIndex = _items.FindIndex(i => i.RequirementId == requirementId);
            if (existingItemIndex < 0)
            {
                throw new BusinessException("VersionControl:ErrorMessage:RequirementNotInBaseline")
                    .WithData("requirementId", requirementId)
                    .WithData("baselineId", Id);
            }
            
            // Remove the existing item and add a new one with the updated version
            _items.RemoveAt(existingItemIndex);
            
            var updatedItem = new BaselineItem(Id, requirementId, newVersion, updatedAt);
            _items.Add(updatedItem);
        }

        /// <summary>
        /// Activates the baseline
        /// </summary>
        public void Activate(DateTime modificationTime)
        {
            if (Status != BaselineStatus.Draft)
            {
                throw new InvalidBaselineStatusTransitionException(Id, Status, BaselineStatus.Active);
            }
            
            if (!_items.Any())
            {
                throw new BusinessException("VersionControl:ErrorMessage:CannotActivateEmptyBaseline")
                    .WithData("baselineId", Id);
            }
            
            var previousStatus = Status;
            Status = BaselineStatus.Active;
            
            AddDistributedEvent(new BaselineStatusChangedEto
            {
                TenantId = TenantId,
                BaselineId = Id,
                Name = Name,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ModifierId = LastModifierId.GetValueOrDefault(),
                ModificationTime = modificationTime
            });
        }

        /// <summary>
        /// Archives the baseline
        /// </summary>
        public void Archive(DateTime modificationTime)
        {
            if (Status != BaselineStatus.Active)
            {
                throw new InvalidBaselineStatusTransitionException(Id, Status, BaselineStatus.Archived);
            }
            
            var previousStatus = Status;
            Status = BaselineStatus.Archived;
            
            AddDistributedEvent(new BaselineStatusChangedEto
            {
                TenantId = TenantId,
                BaselineId = Id,
                Name = Name,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ModifierId = LastModifierId.GetValueOrDefault(),
                ModificationTime = modificationTime
            });
        }

        private void EnsureCanBeModified()
        {
            if (Status == BaselineStatus.Active)
            {
                throw new BusinessException("VersionControl:ErrorMessage:CannotModifyActiveBaseline")
                    .WithData("baselineId", Id);
            }
            
            if (Status == BaselineStatus.Archived)
            {
                throw new BusinessException("VersionControl:ErrorMessage:CannotModifyArchivedBaseline")
                    .WithData("baselineId", Id);
            }
        }
    }
}

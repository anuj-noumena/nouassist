using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.ResourceType;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceType
{
    /// <summary>
    /// Represents a resource type definition in the system.
    /// Resource types define the classes of resources (e.g., "HumanResource", "Equipment").
    /// </summary>
    public class ResourceTypeAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID, if this resource type belongs to a specific tenant
        /// </summary>
        public Guid? TenantId { get; protected set; }

        /// <summary>
        /// Code uniquely identifying this resource type
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Name of the resource type
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the resource type
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Category of the resource type
        /// </summary>
        public ResourceCategory Category { get; private set; }

        /// <summary>
        /// Path in the category hierarchy
        /// </summary>
        public CategoryPath CategoryPath { get; private set; }

        /// <summary>
        /// Indicates if this resource type is active
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Icons to represent this resource type (optional)
        /// </summary>
        public string Icon { get; private set; }

        /// <summary>
        /// Color to represent this resource type (optional)
        /// </summary>
        public string Color { get; private set; }

        private readonly List<ResourceAttributeDefinition> _attributeDefinitions;

        /// <summary>
        /// List of attribute definitions for this resource type
        /// </summary>
        public IReadOnlyCollection<ResourceAttributeDefinition> AttributeDefinitions => _attributeDefinitions.AsReadOnly();

        protected ResourceTypeAggregate()
        {
            _attributeDefinitions = new List<ResourceAttributeDefinition>();
        }

        /// <summary>
        /// Creates a new resource type
        /// </summary>
        /// <param name="id">Resource type ID</param>
        /// <param name="code">Unique code</param>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="category">Resource category</param>
        /// <param name="categoryPath">Category path (optional)</param>
        /// <param name="tenantId">Tenant ID (optional)</param>
        public ResourceTypeAggregate(
            Guid id,
            string code,
            string name,
            string description,
            ResourceCategory category,
            CategoryPath categoryPath = null,
            Guid? tenantId = null) : base(id)
        {
            SetCode(code);
            SetName(name);
            SetDescription(description);
            Category = category;
            CategoryPath = categoryPath ?? new CategoryPath(category.ToString());
            IsActive = true;
            TenantId = tenantId;
            _attributeDefinitions = new List<ResourceAttributeDefinition>();

            AddDistributedEvent(new ResourceTypeDefinedEto
            {
                Id = Id,
                Code = Code,
                Name = Name,
                Category = Category.ToString(),
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the resource type information
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="category">Resource category</param>
        /// <param name="categoryPath">Category path</param>
        /// <param name="icon">Icon</param>
        /// <param name="color">Color</param>
        public void Update(
            string name,
            string description,
            ResourceCategory category,
            CategoryPath categoryPath = null,
            string icon = null,
            string color = null)
        {
            SetName(name);
            SetDescription(description);
            Category = category;
            CategoryPath = categoryPath ?? new CategoryPath(category.ToString());
            Icon = icon;
            Color = color;

            AddDistributedEvent(new ResourceTypeUpdatedEto
            {
                Id = Id,
                Code = Code,
                Name = Name,
                Category = Category.ToString(),
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Deactivates this resource type
        /// </summary>
        public void Deactivate()
        {
            if (!IsActive)
            {
                return;
            }

            IsActive = false;
        }

        /// <summary>
        /// Activates this resource type
        /// </summary>
        public void Activate()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
        }

        /// <summary>
        /// Adds an attribute definition to this resource type
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="dataType">Attribute data type</param>
        /// <param name="description">Attribute description (optional)</param>
        /// <param name="isRequired">Whether the attribute is required (optional)</param>
        /// <param name="defaultValue">Default value (optional)</param>
        /// <param name="possibleValues">Possible values for enum type (optional)</param>
        /// <returns>The created attribute definition</returns>
        public ResourceAttributeDefinition AddAttributeDefinition(
            string name,
            AttributeDataType dataType,
            string description = null,
            bool isRequired = false,
            string defaultValue = null,
            List<string> possibleValues = null)
        {
            if (_attributeDefinitions.Any(a => a.Name == name))
            {
                throw new RIMDomainException("RIM:ErrorMessage:AttributeAlreadyExists")
                    .WithData("name", name);
            }

            var attributeDefinition = new ResourceAttributeDefinition(
                Id,
                name,
                dataType,
                description,
                isRequired,
                defaultValue,
                possibleValues);

            _attributeDefinitions.Add(attributeDefinition);

            return attributeDefinition;
        }

        /// <summary>
        /// Updates an existing attribute definition
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="description">Updated description (optional)</param>
        /// <param name="isRequired">Updated required flag (optional)</param>
        /// <param name="defaultValue">Updated default value (optional)</param>
        /// <param name="possibleValues">Updated possible values for enum type (optional)</param>
        /// <returns>The updated attribute definition</returns>
        public ResourceAttributeDefinition UpdateAttributeDefinition(
            string name,
            string description = null,
            bool? isRequired = null,
            string defaultValue = null,
            List<string> possibleValues = null)
        {
            var attributeDefinition = _attributeDefinitions.FirstOrDefault(a => a.Name == name);
            if (attributeDefinition == null)
            {
                throw new RIMDomainException("RIM:ErrorMessage:AttributeNotFound")
                    .WithData("attributeName", name);
            }

            attributeDefinition.Update(
                description ?? attributeDefinition.Description,
                isRequired ?? attributeDefinition.IsRequired,
                defaultValue,
                possibleValues);

            return attributeDefinition;
        }

        /// <summary>
        /// Removes an attribute definition
        /// </summary>
        /// <param name="name">Attribute name</param>
        public void RemoveAttributeDefinition(string name)
        {
            var attributeDefinition = _attributeDefinitions.FirstOrDefault(a => a.Name == name);
            if (attributeDefinition == null)
            {
                throw new RIMDomainException("RIM:ErrorMessage:AttributeNotFound")
                    .WithData("attributeName", name);
            }

            _attributeDefinitions.Remove(attributeDefinition);
        }

        /// <summary>
        /// Gets an attribute definition by name
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <returns>The attribute definition</returns>
        public ResourceAttributeDefinition GetAttributeDefinition(string name)
        {
            var attributeDefinition = _attributeDefinitions.FirstOrDefault(a => a.Name == name);
            if (attributeDefinition == null)
            {
                throw new RIMDomainException("RIM:ErrorMessage:AttributeNotFound")
                    .WithData("attributeName", name);
            }

            return attributeDefinition;
        }

        private void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(
                code,
                nameof(code),
                maxLength: Constants.RIMConstants.ResourceTypes.MaxCodeLength
            );
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: Constants.RIMConstants.ResourceTypes.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: Constants.RIMConstants.ResourceTypes.MaxDescriptionLength
            );
        }
    }
}

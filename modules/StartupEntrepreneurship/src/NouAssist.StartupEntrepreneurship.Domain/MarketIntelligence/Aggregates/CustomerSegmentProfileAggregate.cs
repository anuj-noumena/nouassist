using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates
{
    /// <summary>
    /// Aggregate root representing a customer segment profile for strategic planning
    /// </summary>
    public class CustomerSegmentProfile : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public SegmentType Type { get; private set; }
        public Demographics Demographics { get; private set; }
        public Psychographics Psychographics { get; private set; }
        public decimal EstimatedSize { get; private set; }
        public decimal EstimatedValue { get; private set; }
        public string PrimaryNeeds { get; private set; }
        public string PainPoints { get; private set; }
        public string PreferredChannels { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation properties for entities within the aggregate
        private readonly List<SegmentCharacteristics> _characteristics;
        public IReadOnlyCollection<SegmentCharacteristics> Characteristics => _characteristics.AsReadOnly();

        protected CustomerSegmentProfile()
        {
            _characteristics = new List<SegmentCharacteristics>();
        }

        /// <summary>
        /// Creates a new customer segment profile
        /// </summary>
        public static CustomerSegmentProfile Create(
            Guid id,
            string name,
            string description,
            SegmentType type,
            Demographics demographics,
            Psychographics psychographics,
            string primaryNeeds,
            Guid? tenantId = null)
        {
            return new CustomerSegmentProfile(id, name, description, type, demographics, psychographics, primaryNeeds, tenantId);
        }

        private CustomerSegmentProfile(
            Guid id,
            string name,
            string description,
            SegmentType type,
            Demographics demographics,
            Psychographics psychographics,
            string primaryNeeds,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            Type = type;
            Demographics = demographics ?? throw new ArgumentNullException(nameof(demographics));
            Psychographics = psychographics ?? throw new ArgumentNullException(nameof(psychographics));
            SetPrimaryNeeds(primaryNeeds);
            TenantId = tenantId;
            IsActive = true;
            EstimatedSize = 0;
            EstimatedValue = 0;
            _characteristics = new List<SegmentCharacteristics>();
        }

        /// <summary>
        /// Updates segment profile information
        /// </summary>
        public void UpdateProfile(
            string name,
            string description,
            Demographics demographics,
            Psychographics psychographics,
            string primaryNeeds)
        {
            SetName(name);
            SetDescription(description);
            Demographics = demographics ?? throw new ArgumentNullException(nameof(demographics));
            Psychographics = psychographics ?? throw new ArgumentNullException(nameof(psychographics));
            SetPrimaryNeeds(primaryNeeds);
        }

        /// <summary>
        /// Updates market sizing information
        /// </summary>
        public void UpdateMarketSizing(decimal estimatedSize, decimal estimatedValue)
        {
            if (estimatedSize < 0)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidValue")
                    .WithData("propertyName", "Estimated Size");
            }

            if (estimatedValue < 0)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidValue")
                    .WithData("propertyName", "Estimated Value");
            }

            EstimatedSize = estimatedSize;
            EstimatedValue = estimatedValue;
        }

        /// <summary>
        /// Sets customer pain points
        /// </summary>
        public void SetPainPoints(string painPoints)
        {
            if (!string.IsNullOrEmpty(painPoints) && painPoints.Length > MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Pain Points")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength);
            }

            PainPoints = painPoints?.Trim();
        }

        /// <summary>
        /// Sets preferred communication channels
        /// </summary>
        public void SetPreferredChannels(string preferredChannels)
        {
            if (!string.IsNullOrEmpty(preferredChannels) && preferredChannels.Length > MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Preferred Channels")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength);
            }

            PreferredChannels = preferredChannels?.Trim();
        }

        /// <summary>
        /// Adds a custom characteristic to the segment
        /// </summary>
        public void AddCharacteristic(
            string characteristicName,
            string characteristicValue,
            string category,
            decimal importance = 0.5m,
            string notes = null)
        {
            // Check if characteristic already exists
            if (_characteristics.Any(c => c.CharacteristicName.Equals(characteristicName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:DuplicateCharacteristic")
                    .WithData("characteristicName", characteristicName);
            }

            var characteristic = new SegmentCharacteristics(
                GuidGenerator.Create(),
                Id,
                characteristicName,
                characteristicValue,
                category,
                importance,
                notes
            );

            _characteristics.Add(characteristic);
        }

        /// <summary>
        /// Updates an existing characteristic
        /// </summary>
        public void UpdateCharacteristic(
            Guid characteristicId,
            string characteristicName,
            string characteristicValue,
            string category,
            decimal importance,
            string notes = null)
        {
            var characteristic = _characteristics.FirstOrDefault(c => c.Id == characteristicId);
            
            if (characteristic == null)
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:CharacteristicNotFound")
                    .WithData("characteristicId", characteristicId);
            }

            // Check if new name conflicts with existing characteristics (excluding current one)
            if (_characteristics.Any(c => c.Id != characteristicId && 
                                         c.CharacteristicName.Equals(characteristicName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:DuplicateCharacteristic")
                    .WithData("characteristicName", characteristicName);
            }

            characteristic.UpdateCharacteristic(characteristicName, characteristicValue, category, importance, notes);
        }

        /// <summary>
        /// Removes a characteristic
        /// </summary>
        public void RemoveCharacteristic(Guid characteristicId)
        {
            var characteristic = _characteristics.FirstOrDefault(c => c.Id == characteristicId);
            
            if (characteristic != null)
            {
                _characteristics.Remove(characteristic);
            }
        }

        /// <summary>
        /// Changes segment type
        /// </summary>
        public void ChangeType(SegmentType newType)
        {
            Type = newType;
        }

        /// <summary>
        /// Deactivates the segment profile
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Reactivates the segment profile
        /// </summary>
        public void Reactivate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Gets characteristics by category
        /// </summary>
        public IReadOnlyList<SegmentCharacteristics> GetCharacteristicsByCategory(string category)
        {
            return _characteristics
                .Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.CharacteristicName)
                .ToList();
        }

        /// <summary>
        /// Gets high-priority characteristics
        /// </summary>
        public IReadOnlyList<SegmentCharacteristics> GetHighPriorityCharacteristics()
        {
            return _characteristics
                .Where(c => c.Importance >= 0.7m)
                .OrderByDescending(c => c.Importance)
                .ThenBy(c => c.CharacteristicName)
                .ToList();
        }

        /// <summary>
        /// Calculates segment attractiveness score
        /// </summary>
        public decimal CalculateAttractiveness()
        {
            // Base score from estimated size and value
            decimal baseScore = 0.3m;
            
            if (EstimatedSize > 0)
            {
                baseScore += 0.2m;
            }
            
            if (EstimatedValue > 0)
            {
                baseScore += 0.2m;
            }

            // Additional score from high-priority characteristics
            var highPriorityCount = _characteristics.Count(c => c.Importance >= 0.7m);
            var characteristicScore = Math.Min(0.3m, highPriorityCount * 0.1m);

            return Math.Min(1.0m, baseScore + characteristicScore);
        }

        /// <summary>
        /// Checks if segment has sufficient profiling data
        /// </summary>
        public bool HasSufficientProfileData()
        {
            return !string.IsNullOrWhiteSpace(Demographics.AgeRange) &&
                   !string.IsNullOrWhiteSpace(Demographics.Income) &&
                   !string.IsNullOrWhiteSpace(Psychographics.Interests) &&
                   !string.IsNullOrWhiteSpace(PrimaryNeeds) &&
                   _characteristics.Count >= 3;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: MarketIntelligenceConsts.CustomerSegment.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: MarketIntelligenceConsts.CustomerSegment.MaxDescriptionLength
            );
        }

        private void SetPrimaryNeeds(string primaryNeeds)
        {
            PrimaryNeeds = Check.NotNullOrWhiteSpace(
                primaryNeeds,
                nameof(primaryNeeds),
                maxLength: MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength
            );
        }
    }
}

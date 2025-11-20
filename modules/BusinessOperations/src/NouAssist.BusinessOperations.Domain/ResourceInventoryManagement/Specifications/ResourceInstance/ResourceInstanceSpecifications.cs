using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Specifications.Base;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Specifications.ResourceInstance
{
    /// <summary>
    /// Specifications for resource instances
    /// </summary>
    public static class ResourceInstanceSpecifications
    {
        /// <summary>
        /// Specification for available resources
        /// </summary>
        public class AvailableResourceSpecification : Specification<ResourceInstanceAggregate>
        {
            public override Expression<Func<ResourceInstanceAggregate, bool>> ToExpression()
            {
                return resource => resource.Status == ResourceStatus.Available;
            }
        }

        /// <summary>
        /// Specification for allocated resources
        /// </summary>
        public class AllocatedResourceSpecification : Specification<ResourceInstanceAggregate>
        {
            public override Expression<Func<ResourceInstanceAggregate, bool>> ToExpression()
            {
                return resource => resource.Status == ResourceStatus.Allocated;
            }
        }

        /// <summary>
        /// Specification for resources of a specific type
        /// </summary>
        public class ResourceTypeSpecification : Specification<ResourceInstanceAggregate>
        {
            private readonly Guid _resourceTypeId;

            public ResourceTypeSpecification(Guid resourceTypeId)
            {
                _resourceTypeId = resourceTypeId;
            }

            public override Expression<Func<ResourceInstanceAggregate, bool>> ToExpression()
            {
                return resource => resource.ResourceTypeId == _resourceTypeId;
            }
        }

        /// <summary>
        /// Specification for resources with specific attribute values
        /// </summary>
        public class ResourceAttributesSpecification : Specification<ResourceInstanceAggregate>
        {
            private readonly Dictionary<string, string> _attributeValues;

            public ResourceAttributesSpecification(Dictionary<string, string> attributeValues)
            {
                _attributeValues = attributeValues ?? new Dictionary<string, string>();
            }

            public override Expression<Func<ResourceInstanceAggregate, bool>> ToExpression()
            {
                // This is simplified because LINQ to Entities can't process complex predicates on collections
                // In the actual implementation, this would need to be handled differently, possibly at the repository level
                return resource => true;
            }

            // Instead, we'll do in-memory filtering
            public new bool IsSatisfiedBy(ResourceInstanceAggregate resource)
            {
                if (_attributeValues.Count == 0)
                {
                    return true;
                }

                foreach (var attribute in _attributeValues)
                {
                    var attributeValue = resource.GetAttributeValue(attribute.Key);
                    if (attributeValue == null || attributeValue.StringValue != attribute.Value)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Specification for resources available during a specific time period
        /// </summary>
        public class AvailableDuringTimeRangeSpecification : Specification<ResourceInstanceAggregate>
        {
            private readonly DateTime _startTime;
            private readonly DateTime _endTime;

            public AvailableDuringTimeRangeSpecification(DateTime startTime, DateTime endTime)
            {
                _startTime = startTime;
                _endTime = endTime;
            }

            public override Expression<Func<ResourceInstanceAggregate, bool>> ToExpression()
            {
                // This is simplified because time range availability needs to check allocations
                // In the actual implementation, this would need to be handled at the repository level
                return resource => resource.Status == ResourceStatus.Available;
            }

            // Instead, we'll do in-memory filtering
            public new bool IsSatisfiedBy(ResourceInstanceAggregate resource)
            {
                if (resource.Status == ResourceStatus.Retired || resource.Status == ResourceStatus.Unavailable)
                {
                    return false;
                }

                if (resource.Status == ResourceStatus.Available)
                {
                    return true;
                }

                // Check if the current allocation overlaps with the requested time range
                var currentAllocation = resource.CurrentAllocation;
                if (currentAllocation == null)
                {
                    return true;
                }

                // If allocation has no end time, it's open-ended and not available
                if (!currentAllocation.EndTime.HasValue)
                {
                    return false;
                }

                // Check if allocation end time is before the requested start time
                return currentAllocation.EndTime.Value <= _startTime;
            }
        }

        /// <summary>
        /// Specification for human resources
        /// </summary>
        public class HumanResourceSpecification : Specification<ResourceInstanceAggregate>
        {
            private readonly Guid? _userId;

            public HumanResourceSpecification(Guid? userId = null)
            {
                _userId = userId;
            }

            public override Expression<Func<ResourceInstanceAggregate, bool>> ToExpression()
            {
                if (_userId.HasValue)
                {
                    return resource => resource.UserId != null && resource.UserId.Value == _userId.Value;
                }
                
                return resource => resource.UserId != null;
            }
        }
    }
}

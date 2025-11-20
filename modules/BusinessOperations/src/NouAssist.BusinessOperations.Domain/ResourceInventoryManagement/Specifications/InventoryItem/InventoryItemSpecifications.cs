using System;
using System.Linq.Expressions;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Specifications.Base;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Specifications.InventoryItem
{
    /// <summary>
    /// Specifications for inventory items
    /// </summary>
    public static class InventoryItemSpecifications
    {
        /// <summary>
        /// Specification for active inventory items
        /// </summary>
        public class ActiveInventoryItemSpecification : Specification<InventoryItemDefinitionAggregate>
        {
            public override Expression<Func<InventoryItemDefinitionAggregate, bool>> ToExpression()
            {
                return item => item.IsActive;
            }
        }

        /// <summary>
        /// Specification for inventory items below minimum stock level
        /// </summary>
        public class BelowMinimumStockLevelSpecification : Specification<InventoryItemDefinitionAggregate>
        {
            public override Expression<Func<InventoryItemDefinitionAggregate, bool>> ToExpression()
            {
                // This is simplified because checking across all locations needs more complex logic
                // In the actual implementation, this would need to be handled at the repository level
                return item => true;
            }

            // Instead, we'll do in-memory filtering
            public new bool IsSatisfiedBy(InventoryItemDefinitionAggregate item)
            {
                return item.IsBelowMinimumStockLevel();
            }
        }

        /// <summary>
        /// Specification for inventory items in a specific category
        /// </summary>
        public class CategoryPathSpecification : Specification<InventoryItemDefinitionAggregate>
        {
            private readonly string _categoryPath;
            private readonly bool _includeSubcategories;

            public CategoryPathSpecification(string categoryPath, bool includeSubcategories = false)
            {
                _categoryPath = categoryPath;
                _includeSubcategories = includeSubcategories;
            }

            public override Expression<Func<InventoryItemDefinitionAggregate, bool>> ToExpression()
            {
                if (_includeSubcategories)
                {
                    return item => item.CategoryPath.Path.StartsWith(_categoryPath);
                }

                return item => item.CategoryPath.Path == _categoryPath;
            }
        }

        /// <summary>
        /// Specification for inventory items at a specific location
        /// </summary>
        public class AtLocationSpecification : Specification<InventoryItemDefinitionAggregate>
        {
            private readonly Guid _locationId;
            private readonly bool _onlyWithStock;

            public AtLocationSpecification(Guid locationId, bool onlyWithStock = false)
            {
                _locationId = locationId;
                _onlyWithStock = onlyWithStock;
            }

            public override Expression<Func<InventoryItemDefinitionAggregate, bool>> ToExpression()
            {
                // This is simplified because checking stock levels needs more complex logic
                // In the actual implementation, this would need to be handled at the repository level
                return item => true;
            }

            // Instead, we'll do in-memory filtering
            public new bool IsSatisfiedBy(InventoryItemDefinitionAggregate item)
            {
                var stockLevel = item.GetStockLevel(_locationId);
                
                if (stockLevel == null)
                {
                    return false;
                }

                if (_onlyWithStock)
                {
                    return stockLevel.Quantity > 0;
                }

                return true;
            }
        }

        /// <summary>
        /// Specification for inventory items below minimum stock level at a specific location
        /// </summary>
        public class BelowMinimumStockLevelAtLocationSpecification : Specification<InventoryItemDefinitionAggregate>
        {
            private readonly Guid _locationId;
            private readonly decimal? _locationSpecificMinimum;

            public BelowMinimumStockLevelAtLocationSpecification(Guid locationId, decimal? locationSpecificMinimum = null)
            {
                _locationId = locationId;
                _locationSpecificMinimum = locationSpecificMinimum;
            }

            public override Expression<Func<InventoryItemDefinitionAggregate, bool>> ToExpression()
            {
                // This is simplified because checking minimum levels needs more complex logic
                // In the actual implementation, this would need to be handled at the repository level
                return item => true;
            }

            // Instead, we'll do in-memory filtering
            public new bool IsSatisfiedBy(InventoryItemDefinitionAggregate item)
            {
                var stockLevel = item.GetStockLevel(_locationId);
                
                if (stockLevel == null)
                {
                    return false;
                }

                var minimumLevel = _locationSpecificMinimum ?? item.MinimumStockLevel;
                return stockLevel.Quantity < minimumLevel;
            }
        }
    }
}

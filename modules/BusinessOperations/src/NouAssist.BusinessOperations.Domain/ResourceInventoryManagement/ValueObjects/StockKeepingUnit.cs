using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects
{
    /// <summary>
    /// Represents a Stock Keeping Unit (SKU) for inventory items
    /// </summary>
    public class StockKeepingUnit : ValueObject
    {
        /// <summary>
        /// The SKU value
        /// </summary>
        public string Value { get; private set; }

        private StockKeepingUnit()
        {
        }

        /// <summary>
        /// Creates a new SKU value object
        /// </summary>
        /// <param name="value">SKU string value</param>
        public StockKeepingUnit(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value, 
                nameof(value), 
                maxLength: Constants.RIMConstants.InventoryItems.MaxSkuLength
            );
        }

        /// <summary>
        /// Gets the atomic values for value comparison
        /// </summary>
        /// <returns>Collection of atomic values</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        /// <summary>
        /// Returns the string representation of the SKU
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}

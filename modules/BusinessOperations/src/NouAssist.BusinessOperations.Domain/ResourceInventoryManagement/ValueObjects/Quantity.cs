using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects
{
    /// <summary>
    /// Represents a quantity amount with unit of measure
    /// </summary>
    public class Quantity : ValueObject
    {
        /// <summary>
        /// The amount value
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// The unit of measure
        /// </summary>
        public string UnitOfMeasure { get; private set; }

        private Quantity()
        {
        }

        /// <summary>
        /// Creates a new quantity value object
        /// </summary>
        /// <param name="amount">Amount value</param>
        /// <param name="unitOfMeasure">Unit of measure (e.g., "ea", "kg", "m")</param>
        public Quantity(decimal amount, string unitOfMeasure)
        {
            if (amount < 0)
            {
                throw new RIMDomainException("RIM:Validation:QuantityMustBePositive");
            }

            Amount = amount;
            UnitOfMeasure = unitOfMeasure ?? "ea"; // Default to "each" if not specified
        }

        /// <summary>
        /// Adds the specified quantity to this quantity
        /// </summary>
        /// <param name="other">Quantity to add</param>
        /// <returns>New quantity object with the sum</returns>
        public Quantity Add(Quantity other)
        {
            if (other == null)
            {
                return new Quantity(Amount, UnitOfMeasure);
            }

            if (UnitOfMeasure != other.UnitOfMeasure)
            {
                throw new RIMDomainException("RIM:ErrorMessage:IncompatibleUnits")
                    .WithData("unit1", UnitOfMeasure)
                    .WithData("unit2", other.UnitOfMeasure);
            }

            return new Quantity(Amount + other.Amount, UnitOfMeasure);
        }

        /// <summary>
        /// Subtracts the specified quantity from this quantity
        /// </summary>
        /// <param name="other">Quantity to subtract</param>
        /// <returns>New quantity object with the difference</returns>
        public Quantity Subtract(Quantity other)
        {
            if (other == null)
            {
                return new Quantity(Amount, UnitOfMeasure);
            }

            if (UnitOfMeasure != other.UnitOfMeasure)
            {
                throw new RIMDomainException("RIM:ErrorMessage:IncompatibleUnits")
                    .WithData("unit1", UnitOfMeasure)
                    .WithData("unit2", other.UnitOfMeasure);
            }

            var result = Amount - other.Amount;
            if (result < 0)
            {
                throw new RIMDomainException("RIM:ErrorMessage:NegativeQuantityResult")
                    .WithData("amount1", Amount)
                    .WithData("amount2", other.Amount)
                    .WithData("unit", UnitOfMeasure);
            }

            return new Quantity(result, UnitOfMeasure);
        }

        /// <summary>
        /// Gets the atomic values for value comparison
        /// </summary>
        /// <returns>Collection of atomic values</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return UnitOfMeasure;
        }

        /// <summary>
        /// Returns the string representation of the quantity
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"{Amount} {UnitOfMeasure}";
        }
    }
}

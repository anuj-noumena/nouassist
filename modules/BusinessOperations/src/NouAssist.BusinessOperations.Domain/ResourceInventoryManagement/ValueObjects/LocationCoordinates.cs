using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects
{
    /// <summary>
    /// Represents geographic coordinates for a location
    /// </summary>
    public class LocationCoordinates : ValueObject
    {
        /// <summary>
        /// Latitude in decimal degrees
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// Longitude in decimal degrees
        /// </summary>
        public double Longitude { get; private set; }

        private LocationCoordinates()
        {
        }

        /// <summary>
        /// Creates a new location coordinates value object
        /// </summary>
        /// <param name="latitude">Latitude (-90 to 90)</param>
        /// <param name="longitude">Longitude (-180 to 180)</param>
        public LocationCoordinates(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Calculates the distance to another location in kilometers
        /// </summary>
        /// <param name="other">Other location coordinates</param>
        /// <returns>Distance in kilometers</returns>
        public double DistanceTo(LocationCoordinates other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            // Haversine formula for calculating great-circle distance
            const double earthRadius = 6371; // in kilometers
            var dLat = ToRadians(other.Latitude - Latitude);
            var dLon = ToRadians(other.Longitude - Longitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(Latitude)) * Math.Cos(ToRadians(other.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadius * c;
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        /// <summary>
        /// Gets the atomic values for value comparison
        /// </summary>
        /// <returns>Collection of atomic values</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Latitude;
            yield return Longitude;
        }

        /// <summary>
        /// Returns the string representation of the coordinates
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"{Latitude:F6}, {Longitude:F6}";
        }
    }
}

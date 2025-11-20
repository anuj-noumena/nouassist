namespace NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos
{
    /// <summary>
    /// DTO for geographic coordinates
    /// </summary>
    public class LocationCoordinatesDto
    {
        /// <summary>
        /// Latitude in decimal degrees
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude in decimal degrees
        /// </summary>
        public double Longitude { get; set; }
    }
}

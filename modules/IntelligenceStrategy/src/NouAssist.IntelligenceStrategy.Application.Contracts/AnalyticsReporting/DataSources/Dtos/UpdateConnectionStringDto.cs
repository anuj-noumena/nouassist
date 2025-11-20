namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for updating connection string
    /// </summary>
    public class UpdateConnectionStringDto
    {
        /// <summary>
        /// The actual connection string value
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// Type of connection (e.g., SQL, MongoDB, etc.)
        /// </summary>
        public string ConnectionType { get; set; }
        
        /// <summary>
        /// Whether the connection should be encrypted
        /// </summary>
        public bool IsEncrypted { get; set; }
    }
}

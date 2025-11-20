using System.Collections.Generic;

namespace NouAssist.Analytics.DTOs.DataSource
{
    /// <summary>
    /// DTO for data source credentials information
    /// </summary>
    public class DataSourceCredentialsDto
    {
        public string Username { get; set; }
        
        public string EncryptedPassword { get; set; }
        
        public string ApiKey { get; set; }
        
        public string TokenType { get; set; }
        
        public Dictionary<string, string> AdditionalSecrets { get; set; }
    }
}

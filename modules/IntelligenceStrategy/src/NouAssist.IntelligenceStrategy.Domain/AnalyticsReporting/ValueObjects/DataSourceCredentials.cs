using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Encrypted credentials for data source access
    /// </summary>
    public class DataSourceCredentials : ValueObject
    {
        /// <summary>
        /// The encrypted credentials JSON
        /// </summary>
        public string EncryptedCredentials { get; private set; }
        
        /// <summary>
        /// When the credentials were last updated
        /// </summary>
        public DateTime LastUpdated { get; private set; }
        
        /// <summary>
        /// Whether the credentials are valid
        /// </summary>
        public bool IsValid { get; private set; }
        
        private DataSourceCredentials()
        {
        }
        
        public DataSourceCredentials(string encryptedCredentials, DateTime lastUpdated, bool isValid = true)
        {
            EncryptedCredentials = Check.NotNullOrWhiteSpace(
                encryptedCredentials,
                nameof(encryptedCredentials),
                AnalyticsReportingConsts.MaxEncryptedCredentialsLength
            );
            LastUpdated = lastUpdated;
            IsValid = isValid;
        }
        
        /// <summary>
        /// Mark credentials as invalid
        /// </summary>
        public void Invalidate()
        {
            IsValid = false;
        }
        
        /// <summary>
        /// Update credentials
        /// </summary>
        public void Update(string encryptedCredentials, DateTime updateTime)
        {
            EncryptedCredentials = Check.NotNullOrWhiteSpace(
                encryptedCredentials,
                nameof(encryptedCredentials),
                AnalyticsReportingConsts.MaxEncryptedCredentialsLength
            );
            LastUpdated = updateTime;
            IsValid = true;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EncryptedCredentials;
            yield return LastUpdated;
            yield return IsValid;
        }
    }
}

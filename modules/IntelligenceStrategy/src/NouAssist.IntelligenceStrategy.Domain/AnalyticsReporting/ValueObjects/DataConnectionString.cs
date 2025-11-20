using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Connection string to an internal data store
    /// </summary>
    public class DataConnectionString : ValueObject
    {
        /// <summary>
        /// The actual connection string value
        /// </summary>
        public string Value { get; private set; }
        
        /// <summary>
        /// Type of connection (e.g., SQL, MongoDB, etc.)
        /// </summary>
        public string ConnectionType { get; private set; }
        
        /// <summary>
        /// Whether the connection is encrypted
        /// </summary>
        public bool IsEncrypted { get; private set; }
        
        private DataConnectionString()
        {
        }
        
        public DataConnectionString(string value, string connectionType, bool isEncrypted = true)
        {
            Value = Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                AnalyticsReportingConsts.MaxConnectionStringLength
            );
            ConnectionType = Check.NotNullOrWhiteSpace(connectionType, nameof(connectionType));
            IsEncrypted = isEncrypted;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return ConnectionType;
            yield return IsEncrypted;
        }
    }
}

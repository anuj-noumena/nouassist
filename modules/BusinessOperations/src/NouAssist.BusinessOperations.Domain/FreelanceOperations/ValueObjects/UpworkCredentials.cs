using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing Upwork API credentials
    /// </summary>
    public class UpworkCredentials : ValueObject
    {
        /// <summary>
        /// API key or client ID for Upwork API
        /// </summary>
        public string ApiKey { get; private set; }
        
        /// <summary>
        /// API secret for Upwork API
        /// </summary>
        public string ApiSecret { get; private set; }
        
        /// <summary>
        /// OAuth access token for authenticated requests
        /// </summary>
        public string AccessToken { get; private set; }
        
        /// <summary>
        /// OAuth refresh token
        /// </summary>
        public string RefreshToken { get; private set; }
        
        /// <summary>
        /// When the access token expires
        /// </summary>
        public DateTime? TokenExpiration { get; private set; }

        protected UpworkCredentials()
        {
        }

        public UpworkCredentials(
            string apiKey, 
            string apiSecret,
            string accessToken = null,
            string refreshToken = null,
            DateTime? tokenExpiration = null)
        {
            ApiKey = Check.NotNullOrWhiteSpace(
                apiKey, 
                nameof(apiKey), 
                maxLength: FreelanceOperationsConsts.Upwork.MaxCredentialsLength
            );
            
            ApiSecret = Check.NotNullOrWhiteSpace(
                apiSecret, 
                nameof(apiSecret), 
                maxLength: FreelanceOperationsConsts.Upwork.MaxCredentialsLength
            );
            
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            TokenExpiration = tokenExpiration;
        }

        /// <summary>
        /// Update the OAuth tokens
        /// </summary>
        public void UpdateTokens(string accessToken, string refreshToken, DateTime tokenExpiration)
        {
            AccessToken = Check.NotNullOrWhiteSpace(accessToken, nameof(accessToken));
            RefreshToken = Check.NotNullOrWhiteSpace(refreshToken, nameof(refreshToken));
            TokenExpiration = tokenExpiration;
        }

        /// <summary>
        /// Check if the access token is expired
        /// </summary>
        public bool IsTokenExpired()
        {
            return !TokenExpiration.HasValue || DateTime.UtcNow > TokenExpiration.Value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ApiKey;
            yield return ApiSecret;
            yield return AccessToken ?? string.Empty;
            yield return RefreshToken ?? string.Empty;
            yield return TokenExpiration;
        }
    }
}

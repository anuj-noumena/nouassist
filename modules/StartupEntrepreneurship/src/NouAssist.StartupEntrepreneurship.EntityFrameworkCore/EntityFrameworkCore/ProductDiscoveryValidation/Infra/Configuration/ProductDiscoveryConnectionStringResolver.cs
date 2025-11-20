namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Configuration
{
    /// <summary>
    /// Custom connection string resolver for Product Discovery module
    /// </summary>
    public class ProductDiscoveryConnectionStringResolver : IConnectionStringResolver
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionStringResolver _defaultResolver;

        public ProductDiscoveryConnectionStringResolver(
            IConfiguration configuration,
            IConnectionStringResolver defaultResolver)
        {
            _configuration = configuration;
            _defaultResolver = defaultResolver;
        }

        public Task<string> ResolveAsync(string? connectionStringName = null)
        {
            // Try Product Discovery specific connection string first
            if (connectionStringName == ProductDiscoveryDbProperties.ConnectionStringName)
            {
                var connectionString = _configuration.GetConnectionString(connectionStringName);
                if (!string.IsNullOrEmpty(connectionString))
                {
                    return Task.FromResult(connectionString);
                }
            }

            // Fallback to default resolver
            return _defaultResolver.ResolveAsync(connectionStringName);
        }
    }
}

// appsettings.json configuration example
/*
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=MyApp;Uid=root;Pwd=password;",
    "ProductDiscovery": "Server=localhost;Database=MyApp_ProductDiscovery;Uid=root;Pwd=password;"
  },
  "ProductDiscovery": {
    "Database": {
      "TablePrefix": "ProductDiscovery",
      "Schema": null,
      "EnableSensitiveDataLogging": false,
      "CommandTimeout": 30,
      "MaxRetryCount": 3
    },
    "Performance": {
      "EnableConnectionPooling": true,
      "MaxPoolSize": 100,
      "MinPoolSize": 5,
      "ConnectionLifetime": 300
    }
  }
}
*/

using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore
{
    /// <summary>
    /// Factory class for creating RIMDbContext instances during design-time operations (migrations)
    /// </summary>
    public class RIMDbContextFactory : IDesignTimeDbContextFactory<RIMDbContext>
    {
        /// <summary>
        /// Creates a new instance of RIMDbContext with configuration from appsettings.json
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        /// <returns>RIMDbContext instance</returns>
        public RIMDbContext CreateDbContext(string[] args)
        {
            // Build configuration from appsettings.json
            var configuration = BuildConfiguration();

            // Get connection string
            var connectionString = configuration.GetConnectionString(RIMDbProperties.ConnectionStringName);

            // Create DbContextOptions
            var builder = new DbContextOptionsBuilder<RIMDbContext>()
                .UseNpgsql(connectionString);

            return new RIMDbContext(builder.Options);
        }

        /// <summary>
        /// Builds the configuration from appsettings.json
        /// </summary>
        /// <returns>Configuration</returns>
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../NouAssist.Host"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}

<!-- src/NouAssist.BusinessOperations.ResourceInventoryManagement.Application.Contracts/NouAssist.BusinessOperations.ResourceInventoryManagement.Application.Contracts.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>net9.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <RootNamespace>NouAssist.BusinessOperations.ResourceInventoryManagement</RootNamespace>
    </PropertyGroup>

    <ItemGroup>
        <ProjectReference Include="..\NouAssist.BusinessOperations.ResourceInventoryManagement.Domain.Shared\NouAssist.BusinessOperations.ResourceInventoryManagement.Domain.Shared.csproj" />
    </ItemGroup>

    <ItemGroup>
        <PackageReference Include="Volo.Abp.Ddd.Application.Contracts" Version="9.1.0" />
        <PackageReference Include="Volo.Abp.Authorization" Version="9.1.0" />
        <PackageReference Include="Volo.Abp.Validation" Version="9.1.0" />
    </ItemGroup>
</Project>

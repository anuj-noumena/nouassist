using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Services
{
    /// <summary>
    /// Domain service for barcode generation and scanning operations
    /// </summary>
    public class BarcodeService : DomainService
    {
        private readonly IInventoryItemRepository _inventoryRepository;
        private readonly ILogger<BarcodeService> _logger;

        /// <summary>
        /// Creates a new instance of BarcodeService
        /// </summary>
        /// <param name="inventoryRepository">Inventory item repository</param>
        /// <param name="logger">Logger</param>
        public BarcodeService(
            IInventoryItemRepository inventoryRepository,
            ILogger<BarcodeService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }

        /// <summary>
        /// Generates a barcode for an inventory item
        /// </summary>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="barcodeFormat">Barcode format (default is CODE128)</param>
        /// <returns>Generated barcode value</returns>
        public async Task<string> GenerateBarcodeAsync(
            Guid inventoryItemId, 
            BarcodeFormat barcodeFormat = BarcodeFormat.CODE128)
        {
            var inventoryItem = await _inventoryRepository.GetAsync(inventoryItemId);
            if (inventoryItem == null)
            {
                throw new ArgumentException($"Inventory item with ID {inventoryItemId} not found", nameof(inventoryItemId));
            }

            // Generate barcode string based on format
            string barcode;

            switch (barcodeFormat)
            {
                case BarcodeFormat.EAN13:
                    barcode = GenerateEan13Barcode(inventoryItem);
                    break;
                case BarcodeFormat.UPC:
                    barcode = GenerateUpcBarcode(inventoryItem);
                    break;
                case BarcodeFormat.QR:
                    barcode = GenerateQrCode(inventoryItem);
                    break;
                case BarcodeFormat.CODE128:
                default:
                    barcode = GenerateCode128Barcode(inventoryItem);
                    break;
            }

            // Update inventory item with barcode
            inventoryItem.Barcode = barcode;
            await _inventoryRepository.UpdateAsync(inventoryItem);

            _logger.LogInformation(
                "Generated {Format} barcode {Barcode} for inventory item {ItemId}",
                barcodeFormat, barcode, inventoryItemId);

            return barcode;
        }

        /// <summary>
        /// Finds inventory items by barcode
        /// </summary>
        /// <param name="barcode">Barcode value</param>
        /// <returns>List of matching inventory items</returns>
        public async Task<List<InventoryItemDefinitionAggregate>> FindItemsByBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                throw new ArgumentException("Barcode cannot be empty", nameof(barcode));
            }

            return await _inventoryRepository.GetListByBarcodeAsync(barcode);
        }

        private string GenerateCode128Barcode(InventoryItemDefinitionAggregate item)
        {
            // For CODE128, use a combination of item ID and SKU
            // In a real implementation, this would follow proper CODE128 format with checksums
            return $"RIM{item.Id.ToString("N").Substring(0, 8)}{item.Sku.Value.Replace("-", "")}";
        }

        private string GenerateEan13Barcode(InventoryItemDefinitionAggregate item)
        {
            // For EAN-13, create a 13-digit number (12 digits + 1 check digit)
            // In a real implementation, this would follow proper EAN-13 format with checksums
            var prefix = "200"; // Example company prefix
            var itemNumber = item.Id.ToString("N").Substring(0, 9);
            
            // Return with check digit (simplified)
            return prefix + itemNumber + "0";
        }

        private string GenerateUpcBarcode(InventoryItemDefinitionAggregate item)
        {
            // For UPC-A, create a 12-digit number
            // In a real implementation, this would follow proper UPC-A format with checksums
            var prefix = "1"; // Example number system digit
            var manufacturerCode = "12345"; // Example manufacturer code
            var itemNumber = item.Id.ToString("N").Substring(0, 5);
            
            // Return with check digit (simplified)
            return prefix + manufacturerCode + itemNumber + "0";
        }

        private string GenerateQrCode(InventoryItemDefinitionAggregate item)
        {
            // For QR Code, create a JSON-like string with item details
            // In a real implementation, this would be properly formatted
            return $"RIM:ITEM:{item.Id}:SKU:{item.Sku.Value}:NAME:{item.Name}";
        }
    }

    /// <summary>
    /// Barcode format options
    /// </summary>
    public enum BarcodeFormat
    {
        /// <summary>
        /// CODE128 format
        /// </summary>
        CODE128 = 0,

        /// <summary>
        /// EAN-13 format
        /// </summary>
        EAN13 = 1,

        /// <summary>
        /// UPC-A format
        /// </summary>
        UPC = 2,

        /// <summary>
        /// QR Code format
        /// </summary>
        QR = 3
    }
}

<!-- src/NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore/NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <RootNamespace>NouAssist.BusinessOperations.ResourceInventoryManagement</RootNamespace>
    </PropertyGroup>

    <ItemGroup>
        <ProjectReference Include="..\NouAssist.BusinessOperations.ResourceInventoryManagement.Domain\NouAssist.BusinessOperations.ResourceInventoryManagement.Domain.csproj" />
    </ItemGroup>

    <ItemGroup>
        <PackageReference Include="Volo.Abp.EntityFrameworkCore" Version="9.1.0" />
        <PackageReference Include="Volo.Abp.EntityFrameworkCore.PostgreSQL" Version="9.1.0" />
        <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.2" />
    </ItemGroup>
</Project>

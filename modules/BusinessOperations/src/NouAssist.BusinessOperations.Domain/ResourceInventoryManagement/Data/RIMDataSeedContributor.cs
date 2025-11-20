using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceType;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.StorageLocation;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Data
{
    /// <summary>
    /// Data seed contributor for the RIM module
    /// </summary>
    public class RIMDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IResourceTypeRepository _resourceTypeRepository;
        private readonly IStorageLocationRepository _locationRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly ILogger<RIMDataSeedContributor> _logger;

        /// <summary>
        /// Creates a new instance of RIMDataSeedContributor
        /// </summary>
        /// <param name="resourceTypeRepository">Resource type repository</param>
        /// <param name="locationRepository">Storage location repository</param>
        /// <param name="guidGenerator">GUID generator</param>
        /// <param name="currentTenant">Current tenant</param>
        /// <param name="logger">Logger</param>
        public RIMDataSeedContributor(
            IResourceTypeRepository resourceTypeRepository,
            IStorageLocationRepository locationRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            ILogger<RIMDataSeedContributor> logger)
        {
            _resourceTypeRepository = resourceTypeRepository;
            _locationRepository = locationRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _logger = logger;
        }

        /// <summary>
        /// Seeds initial data for the RIM module
        /// </summary>
        /// <param name="context">Data seeding context</param>
        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                if (await _resourceTypeRepository.GetCountAsync() > 0)
                {
                    // Skip if data already exists
                    return;
                }

                _logger.LogInformation("Seeding RIM data...");

                await SeedDefaultResourceTypesAsync();
                await SeedDefaultStorageLocationsAsync();

                _logger.LogInformation("RIM data seeding completed.");
            }
        }

        private async Task SeedDefaultResourceTypesAsync()
        {
            // Seed default resource types
            var resourceTypes = new List<ResourceTypeAggregate>
            {
                // Human Resource
                CreateResourceType(
                    "HR-EMPLOYEE",
                    "Employee",
                    "Regular full-time or part-time employee",
                    ResourceCategory.HumanResource,
                    new CategoryPath("Human Resources/Employee")),

                // Equipment
                CreateResourceType(
                    "EQ-COMPUTER",
                    "Computer",
                    "Desktop or laptop computer",
                    ResourceCategory.Equipment,
                    new CategoryPath("Equipment/IT/Computer")),
                
                CreateResourceType(
                    "EQ-VEHICLE",
                    "Vehicle",
                    "Car, truck, or other vehicle",
                    ResourceCategory.Equipment,
                    new CategoryPath("Equipment/Transportation/Vehicle")),

                // Facility
                CreateResourceType(
                    "FAC-MEETING-ROOM",
                    "Meeting Room",
                    "Conference or meeting room",
                    ResourceCategory.Facility,
                    new CategoryPath("Facility/Meeting Room")),
                
                // Digital
                CreateResourceType(
                    "DIG-LICENSE",
                    "Software License",
                    "Software license or subscription",
                    ResourceCategory.Digital,
                    new CategoryPath("Digital/Software License"))
            };

            // Add default attributes to resource types
            AddDefaultAttributesToResourceTypes(resourceTypes);

            // Save resource types
            foreach (var resourceType in resourceTypes)
            {
                await _resourceTypeRepository.InsertAsync(resourceType);
            }

            _logger.LogInformation("Seeded {Count} default resource types", resourceTypes.Count);
        }

        private ResourceTypeAggregate CreateResourceType(
            string code,
            string name,
            string description,
            ResourceCategory category,
            CategoryPath categoryPath)
        {
            return new ResourceTypeAggregate(
                _guidGenerator.Create(),
                code,
                name,
                description,
                category,
                categoryPath,
                _currentTenant.Id);
        }

        private void AddDefaultAttributesToResourceTypes(List<ResourceTypeAggregate> resourceTypes)
        {
            // Find resource types by code
            var employeeType = resourceTypes.Find(rt => rt.Code == "HR-EMPLOYEE");
            var computerType = resourceTypes.Find(rt => rt.Code == "EQ-COMPUTER");
            var vehicleType = resourceTypes.Find(rt => rt.Code == "EQ-VEHICLE");
            var meetingRoomType = resourceTypes.Find(rt => rt.Code == "FAC-MEETING-ROOM");
            var licenseType = resourceTypes.Find(rt => rt.Code == "DIG-LICENSE");

            // Add attributes for employee
            if (employeeType != null)
            {
                employeeType.AddAttributeDefinition("Department", AttributeDataType.Text, "Employee's department");
                employeeType.AddAttributeDefinition("JobTitle", AttributeDataType.Text, "Employee's job title");
                employeeType.AddAttributeDefinition("SkillSet", AttributeDataType.Text, "Employee's skills (comma-separated)");
                employeeType.AddAttributeDefinition("HireDate", AttributeDataType.Date, "Date employee was hired");
                employeeType.AddAttributeDefinition("IsManager", AttributeDataType.Boolean, "Whether employee is a manager", false, "false");
            }

            // Add attributes for computer
            if (computerType != null)
            {
                computerType.AddAttributeDefinition("Model", AttributeDataType.Text, "Computer model");
                computerType.AddAttributeDefinition("SerialNumber", AttributeDataType.Text, "Serial number", true);
                computerType.AddAttributeDefinition("PurchaseDate", AttributeDataType.Date, "Date of purchase");
                computerType.AddAttributeDefinition("Specifications", AttributeDataType.Text, "Technical specifications");
                computerType.AddAttributeDefinition("OperatingSystem", AttributeDataType.Text, "Operating system");
            }

            // Add attributes for vehicle
            if (vehicleType != null)
            {
                vehicleType.AddAttributeDefinition("Make", AttributeDataType.Text, "Vehicle manufacturer", true);
                vehicleType.AddAttributeDefinition("Model", AttributeDataType.Text, "Vehicle model", true);
                vehicleType.AddAttributeDefinition("Year", AttributeDataType.Text, "Model year");
                vehicleType.AddAttributeDefinition("LicensePlate", AttributeDataType.Text, "License plate number", true);
                vehicleType.AddAttributeDefinition("VIN", AttributeDataType.Text, "Vehicle identification number", true);
                vehicleType.AddAttributeDefinition("Color", AttributeDataType.Text, "Vehicle color");
            }

            // Add attributes for meeting room
            if (meetingRoomType != null)
            {
                meetingRoomType.AddAttributeDefinition("Capacity", AttributeDataType.Number, "Room capacity (people)");
                meetingRoomType.AddAttributeDefinition("HasVideoConference", AttributeDataType.Boolean, "Has video conferencing equipment", false, "false");
                meetingRoomType.AddAttributeDefinition("Floor", AttributeDataType.Text, "Floor number or name");
                meetingRoomType.AddAttributeDefinition("Building", AttributeDataType.Text, "Building name or code");
                meetingRoomType.AddAttributeDefinition("Features", AttributeDataType.Text, "Room features (comma-separated)");
            }

            // Add attributes for software license
            if (licenseType != null)
            {
                licenseType.AddAttributeDefinition("Software", AttributeDataType.Text, "Software name", true);
                licenseType.AddAttributeDefinition("Version", AttributeDataType.Text, "Software version");
                licenseType.AddAttributeDefinition("LicenseKey", AttributeDataType.Text, "License key", true);
                licenseType.AddAttributeDefinition("ExpirationDate", AttributeDataType.Date, "License expiration date");
                licenseType.AddAttributeDefinition("IsSubscription", AttributeDataType.Boolean, "Is subscription-based", false, "false");
                licenseType.AddAttributeDefinition("SeatsAllowed", AttributeDataType.Number, "Number of seats/users allowed");
            }
        }

        private async Task SeedDefaultStorageLocationsAsync()
        {
            // Seed default storage locations
            var locations = new List<StorageLocationAggregate>
            {
                // Warehouses
                CreateStorageLocation(
                    "WH-MAIN",
                    "Main Warehouse",
                    "Main warehouse for storing inventory",
                    "Warehouse"),
                
                CreateStorageLocation(
                    "WH-SECONDARY",
                    "Secondary Warehouse",
                    "Secondary warehouse for overflow inventory",
                    "Warehouse")
            };

            // Save locations
            foreach (var location in locations)
            {
                await _locationRepository.InsertAsync(location);
            }

            // Get saved locations
            var mainWarehouse = await _locationRepository.FindByCodeAsync("WH-MAIN");
            var secondaryWarehouse = await _locationRepository.FindByCodeAsync("WH-SECONDARY");

            // Create child locations for main warehouse
            if (mainWarehouse != null)
            {
                var mainWarehouseChildren = new List<StorageLocationAggregate>
                {
                    CreateStorageLocation(
                        "WH-MAIN-A",
                        "Zone A",
                        "Zone A of main warehouse",
                        "Zone",
                        mainWarehouse.Id),
                    
                    CreateStorageLocation(
                        "WH-MAIN-B",
                        "Zone B",
                        "Zone B of main warehouse",
                        "Zone",
                        mainWarehouse.Id),
                    
                    CreateStorageLocation(
                        "WH-MAIN-C",
                        "Zone C",
                        "Zone C of main warehouse",
                        "Zone",
                        mainWarehouse.Id)
                };

                // Save child locations
                foreach (var childLocation in mainWarehouseChildren)
                {
                    await _locationRepository.InsertAsync(childLocation);
                    mainWarehouse.AddChildLocation(childLocation.Id, childLocation.Code, childLocation.Name);
                }

                await _locationRepository.UpdateAsync(mainWarehouse);
            }

            _logger.LogInformation("Seeded default storage locations");
        }

        private StorageLocationAggregate CreateStorageLocation(
            string code,
            string name,
            string description,
            string locationType,
            Guid? parentLocationId = null)
        {
            return new StorageLocationAggregate(
                _guidGenerator.Create(),
                code,
                name,
                description,
                locationType,
                parentLocationId,
                null,
                null,
                null,
                null,
                null,
                _currentTenant.Id);
        }
    }
}

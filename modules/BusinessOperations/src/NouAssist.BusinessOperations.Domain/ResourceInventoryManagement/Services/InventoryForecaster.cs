using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Services
{
    /// <summary>
    /// Domain service for inventory forecasting operations
    /// </summary>
    public class InventoryForecaster : DomainService
    {
        private readonly IInventoryItemRepository _inventoryRepository;
        private readonly ILogger<InventoryForecaster> _logger;

        /// <summary>
        /// Creates a new instance of InventoryForecaster
        /// </summary>
        /// <param name="inventoryRepository">Inventory item repository</param>
        /// <param name="logger">Logger</param>
        public InventoryForecaster(
            IInventoryItemRepository inventoryRepository,
            ILogger<InventoryForecaster> logger)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }

        /// <summary>
        /// Forecasts future inventory levels based on historical consumption
        /// </summary>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="locationId">Location ID</param>
        /// <param name="daysToForecast">Number of days to forecast</param>
        /// <returns>Forecasted inventory levels for each future day</returns>
        public async Task<InventoryForecast> ForecastInventoryLevelsAsync(
            Guid inventoryItemId,
            Guid locationId,
            int daysToForecast)
        {
            if (daysToForecast <= 0)
            {
                throw new ArgumentException("Days to forecast must be positive", nameof(daysToForecast));
            }

            // Get inventory item with full transaction history
            var inventoryItem = await _inventoryRepository.GetAsync(inventoryItemId, true);
            if (inventoryItem == null)
            {
                throw new ArgumentException($"Inventory item with ID {inventoryItemId} not found", nameof(inventoryItemId));
            }

            // Get current stock level
            var stockLevel = inventoryItem.GetStockLevel(locationId);
            if (stockLevel == null)
            {
                // No stock at this location
                return new InventoryForecast
                {
                    InventoryItemId = inventoryItemId,
                    LocationId = locationId,
                    CurrentLevel = 0,
                    DailyForecast = Enumerable.Range(1, daysToForecast)
                        .Select(day => new DailyForecast { Day = day, Level = 0 })
                        .ToList()
                };
            }

            // Get historical transactions for this item at this location
            var transactions = inventoryItem.Transactions
                .Where(t => t.LocationId == locationId)
                .Where(t => t.TransactionType == InventoryTransactionType.Issuance)
                .OrderBy(t => t.Timestamp)
                .ToList();

            // Calculate daily consumption rate
            decimal dailyConsumptionRate = CalculateDailyConsumptionRate(transactions);

            // Create forecast
            var forecast = new InventoryForecast
            {
                InventoryItemId = inventoryItemId,
                LocationId = locationId,
                CurrentLevel = stockLevel.Quantity,
                DailyConsumptionRate = dailyConsumptionRate,
                DailyForecast = new List<DailyForecast>()
            };

            decimal previousLevel = stockLevel.Quantity;
            for (int day = 1; day <= daysToForecast; day++)
            {
                decimal estimatedLevel = Math.Max(0, previousLevel - dailyConsumptionRate);
                
                forecast.DailyForecast.Add(new DailyForecast
                {
                    Day = day,
                    Date = DateTime.Now.Date.AddDays(day),
                    Level = estimatedLevel
                });
                
                previousLevel = estimatedLevel;
            }

            // Calculate stock out date if applicable
            if (dailyConsumptionRate > 0)
            {
                int daysUntilStockout = stockLevel.Quantity > 0 
                    ? (int)Math.Ceiling(stockLevel.Quantity / dailyConsumptionRate) 
                    : 0;
                
                if (daysUntilStockout > 0)
                {
                    forecast.EstimatedStockoutDate = DateTime.Now.Date.AddDays(daysUntilStockout);
                }
            }

            return forecast;
        }

        /// <summary>
        /// Generates purchase recommendations based on forecasted inventory levels
        /// </summary>
        /// <param name="inventoryItemIds">Inventory item IDs (if empty, checks all items)</param>
        /// <param name="locationId">Location ID (optional)</param>
        /// <param name="daysToConsider">Number of days to consider in forecast</param>
        /// <returns>List of purchase recommendations</returns>
        public async Task<List<PurchaseRecommendation>> GeneratePurchaseRecommendationsAsync(
            List<Guid> inventoryItemIds = null,
            Guid? locationId = null,
            int daysToConsider = 30)
        {
            List<InventoryItemDefinitionAggregate> inventoryItems;

            // Get inventory items
            if (inventoryItemIds != null && inventoryItemIds.Count > 0)
            {
                inventoryItems = new List<InventoryItemDefinitionAggregate>();
                foreach (var id in inventoryItemIds)
                {
                    var item = await _inventoryRepository.GetAsync(id, true);
                    if (item != null)
                    {
                        inventoryItems.Add(item);
                    }
                }
            }
            else
            {
                inventoryItems = await _inventoryRepository.GetActiveListAsync(true);
            }

            var recommendations = new List<PurchaseRecommendation>();

            foreach (var item in inventoryItems)
            {
                if (locationId.HasValue)
                {
                    var forecast = await ForecastInventoryLevelsAsync(item.Id, locationId.Value, daysToConsider);
                    var recommendation = EvaluateForPurchase(item, forecast);
                    
                    if (recommendation != null)
                    {
                        recommendations.Add(recommendation);
                    }
                }
                else
                {
                    // Check all locations with stock
                    foreach (var stockLevel in item.StockLevels)
                    {
                        var forecast = await ForecastInventoryLevelsAsync(item.Id, stockLevel.LocationId, daysToConsider);
                        var recommendation = EvaluateForPurchase(item, forecast);
                        
                        if (recommendation != null)
                        {
                            recommendations.Add(recommendation);
                        }
                    }
                }
            }

            return recommendations;
        }

        private decimal CalculateDailyConsumptionRate(List<InventoryTransaction> transactions)
        {
            if (transactions.Count < 2)
            {
                return 0;
            }

            // Get date range of transactions
            var firstTransaction = transactions.First();
            var lastTransaction = transactions.Last();
            var dateRange = (lastTransaction.Timestamp - firstTransaction.Timestamp).TotalDays;

            if (dateRange <= 0)
            {
                return 0;
            }

            // Sum all issuance amounts
            decimal totalIssuanceAmount = 0;
            foreach (var transaction in transactions)
            {
                totalIssuanceAmount += Math.Abs(transaction.AdjustmentAmount);
            }

            // Calculate daily rate
            return totalIssuanceAmount / (decimal)dateRange;
        }

        private PurchaseRecommendation EvaluateForPurchase(
            InventoryItemDefinitionAggregate item,
            InventoryForecast forecast)
        {
            // Skip if not below minimum yet
            if (forecast.CurrentLevel >= item.MinimumStockLevel)
            {
                // Check if we'll be below minimum within the forecast period
                var belowMinimumForecast = forecast.DailyForecast
                    .FirstOrDefault(f => f.Level < item.MinimumStockLevel);
                
                if (belowMinimumForecast == null)
                {
                    return null;
                }
            }

            // Calculate order quantity (target level - current level)
            decimal orderQuantity = item.TargetStockLevel - forecast.CurrentLevel;
            
            if (orderQuantity <= 0)
            {
                return null;
            }

            return new PurchaseRecommendation
            {
                InventoryItemId = item.Id,
                InventoryItemName = item.Name,
                Sku = item.Sku.Value,
                LocationId = forecast.LocationId,
                CurrentLevel = forecast.CurrentLevel,
                MinimumLevel = item.MinimumStockLevel,
                TargetLevel = item.TargetStockLevel,
                RecommendedOrderQuantity = orderQuantity,
                UnitOfMeasure = item.UnitOfMeasure,
                EstimatedStockoutDate = forecast.EstimatedStockoutDate,
                DailyConsumptionRate = forecast.DailyConsumptionRate,
                Priority = forecast.EstimatedStockoutDate.HasValue 
                    ? CalculatePriority(forecast.CurrentLevel, item.MinimumStockLevel, forecast.EstimatedStockoutDate.Value) 
                    : PurchasePriority.Low
            };
        }

        private PurchasePriority CalculatePriority(
            decimal currentLevel, 
            decimal minimumLevel, 
            DateTime estimatedStockoutDate)
        {
            // Critical if already below minimum
            if (currentLevel < minimumLevel)
            {
                return PurchasePriority.Critical;
            }

            // High if stock out within 7 days
            var daysUntilStockout = (estimatedStockoutDate - DateTime.Now.Date).TotalDays;
            if (daysUntilStockout <= 7)
            {
                return PurchasePriority.High;
            }

            // Medium if stock out within 14 days
            if (daysUntilStockout <= 14)
            {
                return PurchasePriority.Medium;
            }

            // Otherwise low
            return PurchasePriority.Low;
        }
    }

    /// <summary>
    /// Inventory forecast result
    /// </summary>
    public class InventoryForecast
    {
        /// <summary>
        /// Inventory item ID
        /// </summary>
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// Location ID
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Current inventory level
        /// </summary>
        public decimal CurrentLevel { get; set; }

        /// <summary>
        /// Estimated daily consumption rate
        /// </summary>
        public decimal DailyConsumptionRate { get; set; }

        /// <summary>
        /// Estimated date when stock will run out
        /// </summary>
        public DateTime? EstimatedStockoutDate { get; set; }

        /// <summary>
        /// Daily forecast for future inventory levels
        /// </summary>
        public List<DailyForecast> DailyForecast { get; set; } = new List<DailyForecast>();
    }

    /// <summary>
    /// Daily forecast information
    /// </summary>
    public class DailyForecast
    {
        /// <summary>
        /// Day number (1 is tomorrow)
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Calendar date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Forecasted inventory level
        /// </summary>
        public decimal Level { get; set; }
    }

    /// <summary>
    /// Purchase recommendation result
    /// </summary>
    public class PurchaseRecommendation
    {
        /// <summary>
        /// Inventory item ID
        /// </summary>
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// Inventory item name
        /// </summary>
        public string InventoryItemName { get; set; }

        /// <summary>
        /// Stock keeping unit (SKU)
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Location ID
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Current inventory level
        /// </summary>
        public decimal CurrentLevel { get; set; }

        /// <summary>
        /// Minimum inventory level
        /// </summary>
        public decimal MinimumLevel { get; set; }

        /// <summary>
        /// Target inventory level
        /// </summary>
        public decimal TargetLevel { get; set; }

        /// <summary>
        /// Recommended order quantity
        /// </summary>
        public decimal RecommendedOrderQuantity { get; set; }

        /// <summary>
        /// Unit of measure
        /// </summary>
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Estimated date when stock will run out
        /// </summary>
        public DateTime? EstimatedStockoutDate { get; set; }

        /// <summary>
        /// Daily consumption rate
        /// </summary>
        public decimal DailyConsumptionRate { get; set; }

        /// <summary>
        /// Purchase priority
        /// </summary>
        public PurchasePriority Priority { get; set; }
    }

    /// <summary>
    /// Purchase priority levels
    /// </summary>
    public enum PurchasePriority
    {
        /// <summary>
        /// Low priority
        /// </summary>
        Low = 0,

        /// <summary>
        /// Medium priority
        /// </summary>
        Medium = 1,

        /// <summary>
        /// High priority
        /// </summary>
        High = 2,

        /// <summary>
        /// Critical priority
        /// </summary>
        Critical = 3
    }
}

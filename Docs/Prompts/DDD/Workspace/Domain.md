# ABP Framework Domain Layer Development Guide

This guide provides comprehensive, production-quality instructions and code for generating a domain layer following Domain-Driven Design (DDD) principles and **SOLID design principles** within the ABP Framework v9+ ecosystem. All code is fully defined, documented, and organized in a modular structure with complete file paths specified.

## SOLID Principles Integration Checklist

**In addition to the existing DDD checklist, ensure:**

-   [x] **Single Responsibility Principle (SRP):** Each class has a single, well-defined responsibility
-   [x] **Open/Closed Principle (OCP):** Use interfaces, abstract classes, and strategy patterns for extensibility
-   [x] **Liskov Substitution Principle (LSP):** Derived classes are substitutable for their base classes
-   [x] **Interface Segregation Principle (ISP):** Create focused, client-specific interfaces
-   [x] **Dependency Inversion Principle (DIP):** Depend on abstractions, not concrete implementations
-   [x] Use factory patterns for complex object creation
-   [x] Implement strategy patterns for varying algorithms
-   [x] Apply decorator pattern for cross-cutting concerns
-   [x] Use composition over inheritance where appropriate

---

## SOLID-Compliant Domain Model Implementation

### 1. Interface Segregation Principle (ISP) - Focused Interfaces

Break down large interfaces into smaller, client-specific interfaces.

```csharp
// src/NouAssist.Orders.Domain/Contracts/IOrderValidator.cs
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.ValueObjects;

namespace NouAssist.Orders.Contracts
{
    // ISP: Focused interface for order validation only
    public interface IOrderValidator
    {
        ValidationResult ValidateForConfirmation(Order order);
        ValidationResult ValidateForProcessing(Order order);
        ValidationResult ValidateForCompletion(Order order);
    }
}

// src/NouAssist.Orders.Domain/Contracts/IPriceCalculator.cs
using System.Collections.Generic;
using NouAssist.Orders.ValueObjects;

namespace NouAssist.Orders.Contracts
{
    // ISP: Focused interface for price calculations only
    public interface IPriceCalculator
    {
        PriceCalculationResult CalculateLinePrice(PriceCalculationRequest request);
        PriceCalculationResult CalculateOrderTotal(IEnumerable<PriceCalculationResult> lineResults);
    }
}

// src/NouAssist.Orders.Domain/Contracts/IDiscountStrategy.cs
using NouAssist.Customers.Aggregates;

namespace NouAssist.Orders.Contracts
{
    // ISP: Focused interface for discount calculations
    public interface IDiscountStrategy
    {
        bool IsApplicable(Customer customer, decimal amount);
        decimal CalculateDiscount(decimal amount, Customer customer);
        string GetDiscountDescription();
    }
}

// src/NouAssist.Orders.Domain/Contracts/ITaxCalculator.cs
using NouAssist.Customers.Aggregates;

namespace NouAssist.Orders.Contracts
{
    // ISP: Focused interface for tax calculations
    public interface ITaxCalculator
    {
        decimal CalculateTax(decimal amount, Customer customer, string productCategory);
    }
}

// src/NouAssist.Orders.Domain/Contracts/IOrderFactory.cs
using System;
using NouAssist.Orders.Aggregates;

namespace NouAssist.Orders.Contracts
{
    // ISP: Focused factory interface
    public interface IOrderFactory
    {
        Order CreateOrder(Guid customerId, DateTime orderDate, Guid? workspaceId = null);
        Order CreateOrderWithLines(Guid customerId, DateTime orderDate, 
            IEnumerable<OrderLineRequest> lines, Guid? workspaceId = null);
    }

    public record OrderLineRequest(Guid ProductId, string VariantSku, decimal Price, int Quantity);
}
```

### 2. Single Responsibility Principle (SRP) - Focused Classes

Each class should have only one reason to change.

```csharp
// src/NouAssist.Orders.Domain/Services/OrderValidator.cs
using System.Linq;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Contracts;
using NouAssist.Orders.Domain.Shared.Enums;
using NouAssist.Orders.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.Orders.Services
{
    // SRP: Only responsible for order validation logic
    public class OrderValidator : DomainService, IOrderValidator
    {
        public ValidationResult ValidateForConfirmation(Order order)
        {
            var result = new ValidationResult();
            
            if (order.Status != OrderStatus.Pending)
            {
                result.AddError("Only pending orders can be confirmed.");
            }
            
            if (!order.OrderLines.Any())
            {
                result.AddError("Cannot confirm an order without order lines.");
            }
            
            return result;
        }

        public ValidationResult ValidateForProcessing(Order order)
        {
            var result = new ValidationResult();
            
            if (order.Status != OrderStatus.Confirmed)
            {
                result.AddError("Only confirmed orders can be processed.");
            }
            
            return result;
        }

        public ValidationResult ValidateForCompletion(Order order)
        {
            var result = new ValidationResult();
            
            if (order.Status != OrderStatus.Processing)
            {
                result.AddError("Only processing orders can be completed.");
            }
            
            return result;
        }
    }
}

// src/NouAssist.Orders.Domain/Services/PriceCalculator.cs
using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Orders.Contracts;
using NouAssist.Orders.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.Orders.Services
{
    // SRP: Only responsible for price calculations
    public class PriceCalculator : DomainService, IPriceCalculator
    {
        private readonly IEnumerable<IDiscountStrategy> _discountStrategies;
        private readonly ITaxCalculator _taxCalculator;

        public PriceCalculator(
            IEnumerable<IDiscountStrategy> discountStrategies,
            ITaxCalculator taxCalculator)
        {
            _discountStrategies = discountStrategies;
            _taxCalculator = taxCalculator;
        }

        public PriceCalculationResult CalculateLinePrice(PriceCalculationRequest request)
        {
            var basePrice = request.UnitPrice * request.Quantity;
            var discountAmount = CalculateApplicableDiscounts(basePrice, request);
            var discountedPrice = basePrice - discountAmount;
            var taxAmount = _taxCalculator.CalculateTax(discountedPrice, request.Customer, request.ProductCategory);
            var finalPrice = discountedPrice + taxAmount;

            return new PriceCalculationResult(
                basePrice, discountAmount, taxAmount, finalPrice, request.CalculationDate);
        }

        public PriceCalculationResult CalculateOrderTotal(IEnumerable<PriceCalculationResult> lineResults)
        {
            var results = lineResults.ToList();
            var totalBase = results.Sum(r => r.BasePrice);
            var totalDiscount = results.Sum(r => r.DiscountAmount);
            var totalTax = results.Sum(r => r.TaxAmount);
            var totalFinal = results.Sum(r => r.FinalPrice);
            var latestCalculation = results.Max(r => r.CalculationDate);

            return new PriceCalculationResult(totalBase, totalDiscount, totalTax, totalFinal, latestCalculation);
        }

        private decimal CalculateApplicableDiscounts(decimal amount, PriceCalculationRequest request)
        {
            return _discountStrategies
                .Where(strategy => strategy.IsApplicable(request.Customer, amount))
                .Sum(strategy => strategy.CalculateDiscount(amount, request.Customer));
        }
    }
}

// src/NouAssist.Orders.Domain/ValueObjects/PriceCalculationRequest.cs
using System;
using System.Collections.Generic;
using NouAssist.Customers.Aggregates;
using Volo.Abp.Domain.Values;

namespace NouAssist.Orders.ValueObjects
{
    public class PriceCalculationRequest : ValueObject
    {
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
        public Customer Customer { get; init; }
        public string ProductCategory { get; init; }
        public DateTime CalculationDate { get; init; }

        public PriceCalculationRequest(
            decimal unitPrice, int quantity, Customer customer, 
            string productCategory, DateTime calculationDate)
        {
            UnitPrice = unitPrice;
            Quantity = quantity;
            Customer = customer;
            ProductCategory = productCategory;
            CalculationDate = calculationDate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UnitPrice;
            yield return Quantity;
            yield return Customer.Id;
            yield return ProductCategory;
            yield return CalculationDate;
        }
    }
}
```

### 3. Open/Closed Principle (OCP) - Strategy Patterns

Use strategies to make the system open for extension but closed for modification.

```csharp
// src/NouAssist.Orders.Domain/Strategies/PremiumCustomerDiscountStrategy.cs
using NouAssist.Customers.Aggregates;
using NouAssist.Customers.Domain.Shared.Enums;
using NouAssist.Orders.Contracts;

namespace NouAssist.Orders.Strategies
{
    // OCP: New discount strategies can be added without modifying existing code
    public class PremiumCustomerDiscountStrategy : IDiscountStrategy
    {
        private const decimal PremiumDiscountPercentage = 0.15m; // 15%
        private const decimal MinimumOrderAmount = 100m;

        public bool IsApplicable(Customer customer, decimal amount)
        {
            return customer.Type == CustomerType.Premium && amount >= MinimumOrderAmount;
        }

        public decimal CalculateDiscount(decimal amount, Customer customer)
        {
            return amount * PremiumDiscountPercentage;
        }

        public string GetDiscountDescription()
        {
            return $"Premium Customer Discount: {PremiumDiscountPercentage:P0}";
        }
    }
}

// src/NouAssist.Orders.Domain/Strategies/BulkOrderDiscountStrategy.cs
using NouAssist.Customers.Aggregates;
using NouAssist.Orders.Contracts;

namespace NouAssist.Orders.Strategies
{
    public class BulkOrderDiscountStrategy : IDiscountStrategy
    {
        private const decimal BulkDiscountPercentage = 0.10m; // 10%
        private const decimal BulkOrderThreshold = 500m;

        public bool IsApplicable(Customer customer, decimal amount)
        {
            return amount >= BulkOrderThreshold;
        }

        public decimal CalculateDiscount(decimal amount, Customer customer)
        {
            return amount * BulkDiscountPercentage;
        }

        public string GetDiscountDescription()
        {
            return $"Bulk Order Discount: {BulkDiscountPercentage:P0} for orders over ${BulkOrderThreshold:F2}";
        }
    }
}

// src/NouAssist.Orders.Domain/Strategies/StandardTaxCalculator.cs
using NouAssist.Customers.Aggregates;
using NouAssist.Orders.Contracts;

namespace NouAssist.Orders.Strategies
{
    // OCP: New tax calculation strategies can be added for different regions/rules
    public class StandardTaxCalculator : ITaxCalculator
    {
        private const decimal StandardTaxRate = 0.10m; // 10%
        private const decimal LuxuryTaxRate = 0.15m; // 15%

        public decimal CalculateTax(decimal amount, Customer customer, string productCategory)
        {
            var taxRate = IsLuxuryCategory(productCategory) ? LuxuryTaxRate : StandardTaxRate;
            return amount * taxRate;
        }

        private static bool IsLuxuryCategory(string category)
        {
            return category?.ToLowerInvariant() is "luxury" or "premium" or "jewelry";
        }
    }
}
```

### 4. Dependency Inversion Principle (DIP) - Enhanced Aggregates

Aggregates should depend on abstractions, not concrete implementations.

```csharp
// src/NouAssist.Orders.Domain/Aggregates/Order.cs
using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Orders.Contracts;
using NouAssist.Orders.Domain.Shared.Events;
using NouAssist.Orders.Domain.Shared.Enums;
using NouAssist.Orders.Domain.Shared.Exceptions;
using NouAssist.Orders.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Orders.Aggregates
{
    public class Order : FullAuditedAggregateRoot<Guid>, IMultiTenant, IWorkspaceEntity
    {
        public Guid? WorkspaceId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }

        private readonly List<OrderLine> _orderLines = new();
        public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

        protected Order() { /* Required by ORM */ }

        public Order(Guid id, Guid customerId, DateTime orderDate, Guid? workspaceId = null) : base(id)
        {
            CustomerId = Check.NotNull(customerId, nameof(customerId));
            OrderDate = orderDate;
            WorkspaceId = workspaceId;
            Status = OrderStatus.Pending;
        }

        // DIP: Depends on abstraction (IOrderValidator) not concrete implementation
        public void ConfirmOrder(DateTime confirmedAt, IOrderValidator validator)
        {
            var validation = validator.ValidateForConfirmation(this);
            if (!validation.IsValid)
            {
                throw new OrderDomainException("Orders:ValidationFailed", 
                    string.Join("; ", validation.Errors));
            }
            
            Status = OrderStatus.Confirmed;
            
            AddDistributedEvent(new OrderConfirmedEto
            {
                OrderId = Id,
                CustomerId = CustomerId,
                TenantId = TenantId,
                ConfirmedAt = confirmedAt,
                TotalAmount = GetTotalAmount()
            });
        }

        // DIP: Depends on abstraction for validation
        public void MarkAsProcessed(DateTime processedTime, IOrderValidator validator)
        {
            var validation = validator.ValidateForProcessing(this);
            if (!validation.IsValid)
            {
                throw new OrderDomainException("Orders:ValidationFailed", 
                    string.Join("; ", validation.Errors));
            }
            
            Status = OrderStatus.Processing;
        }

        // DIP: Depends on abstraction for validation
        public void Complete(DateTime completedDate, IOrderValidator validator)
        {
            var validation = validator.ValidateForCompletion(this);
            if (!validation.IsValid)
            {
                throw new OrderDomainException("Orders:ValidationFailed", 
                    string.Join("; ", validation.Errors));
            }
            
            Status = OrderStatus.Completed;
            CompletedDate = completedDate;
            
            AddDistributedEvent(new OrderCompletedEto
            {
                OrderId = Id,
                CustomerId = CustomerId,
                TenantId = TenantId,
                CompletedAt = completedDate,
                TotalAmount = GetTotalAmount()
            });
        }

        public void AddOrderLine(Guid orderLineId, Guid productId, string variantSku, decimal price, int quantity)
        {
            if (Status != OrderStatus.Pending)
            {
                throw new OrderDomainException("Orders:CanOnlyModifyPending", 
                    "Order lines can only be added to pending orders.");
            }
            
            if (_orderLines.Any(l => l.ProductId == productId && l.VariantSku == variantSku))
            {
                throw new OrderDomainException("Orders:DuplicateLine", 
                    "This product variant is already in the order.");
            }
            
            _orderLines.Add(new OrderLine(orderLineId, Id, productId, variantSku, price, quantity));
        }

        public decimal GetTotalAmount() => OrderLines.Sum(l => l.Subtotal);
    }
}
```

### 5. Factory Pattern for Complex Object Creation

```csharp
// src/NouAssist.Orders.Domain/Factories/OrderFactory.cs
using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Contracts;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.Orders.Factories
{
    // SRP: Only responsible for creating orders
    // DIP: Depends on abstraction (IGuidGenerator)
    public class OrderFactory : DomainService, IOrderFactory
    {
        private readonly IGuidGenerator _guidGenerator;

        public OrderFactory(IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
        }

        public Order CreateOrder(Guid customerId, DateTime orderDate, Guid? workspaceId = null)
        {
            var orderId = _guidGenerator.Create();
            return new Order(orderId, customerId, orderDate, workspaceId);
        }

        public Order CreateOrderWithLines(
            Guid customerId, DateTime orderDate, 
            IEnumerable<OrderLineRequest> lines, Guid? workspaceId = null)
        {
            var order = CreateOrder(customerId, orderDate, workspaceId);
            
            foreach (var lineRequest in lines)
            {
                var lineId = _guidGenerator.Create();
                order.AddOrderLine(lineId, lineRequest.ProductId, lineRequest.VariantSku, 
                    lineRequest.Price, lineRequest.Quantity);
            }
            
            return order;
        }
    }
}
```

### 6. Enhanced Domain Service with SOLID Principles

```csharp
// src/NouAssist.Orders.Domain/Services/OrderProcessingService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Customers.Aggregates;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Contracts;
using NouAssist.Orders.Domain.Shared.Events;
using NouAssist.Orders.ValueObjects;
using NouAssist.Products.Aggregates;
using NouAssist.Products.Domain.Shared.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.Orders.Services
{
    // SRP: Only responsible for coordinating order processing
    // DIP: Depends on abstractions, not concrete implementations
    public class OrderProcessingService : DomainService
    {
        private readonly IOrderValidator _orderValidator;
        private readonly IPriceCalculator _priceCalculator;

        public OrderProcessingService(
            IOrderValidator orderValidator,
            IPriceCalculator priceCalculator)
        {
            _orderValidator = orderValidator;
            _priceCalculator = priceCalculator;
        }
        
        public OrderProcessingResult ProcessOrder(
            Order order, List<Product> products, Customer customer, 
            Guid processedByUserId, DateTime processedAt)
        {
            var result = new OrderProcessingResult();
            
            // Use injected validator instead of implementing validation logic here
            var validation = _orderValidator.ValidateForProcessing(order);
            if (!validation.IsValid)
            {
                result.Errors = validation.Errors;
                return result;
            }

            // Validate business rules specific to processing
            var businessValidation = ValidateBusinessRulesForProcessing(order, products, customer);
            if (!businessValidation.IsValid)
            {
                result.Errors = businessValidation.Errors;
                return result;
            }

            // Use injected price calculator
            RecalculatePricing(order, products, customer, processedAt);
            
            // Use injected validator for state transition
            order.MarkAsProcessed(processedAt, _orderValidator);
            
            order.AddDistributedEvent(new OrderProcessedEto
            {
                OrderId = order.Id,
                TenantId = order.TenantId,
                ProcessedAt = processedAt,
                ProcessedBy = processedByUserId,
                TotalAmount = order.GetTotalAmount()
            });

            result.ProcessedOrder = order;
            result.IsSuccessful = true;
            return result;
        }

        // SRP: Separate method for business rule validation
        private ValidationResult ValidateBusinessRulesForProcessing(
            Order order, List<Product> products, Customer customer)
        {
            var result = new ValidationResult();
            
            if (!customer.IsActive)
            {
                result.AddError("Customer account is not active.");
            }

            foreach (var line in order.OrderLines)
            {
                var product = products.FirstOrDefault(p => p.Id == line.ProductId);
                if (product == null)
                {
                    result.AddError($"Product with ID {line.ProductId} not found.");
                    continue;
                }
                
                if (product.Status != ProductStatus.Published)
                {
                    result.AddError($"Product '{product.Name}' is not available for purchase.");
                }
            }
            
            return result;
        }

        // SRP: Separate method for price recalculation
        private void RecalculatePricing(Order order, List<Product> products, Customer customer, DateTime calculationTime)
        {
            foreach (var line in order.OrderLines)
            {
                var product = products.First(p => p.Id == line.ProductId);
                var variant = product.GetVariant(line.VariantSku);
                
                var request = new PriceCalculationRequest(
                    variant.Price, line.Quantity, customer, 
                    product.Category, calculationTime);
                
                var priceResult = _priceCalculator.CalculateLinePrice(request);
                line.UpdatePricing(priceResult.FinalPrice, priceResult.TaxAmount);
            }
        }
    }

    public class OrderProcessingResult
    {
        public bool IsSuccessful { get; set; }
        public Order ProcessedOrder { get; set; }
        public IReadOnlyList<string> Errors { get; set; } = new List<string>();
    }
}
```

### 7. Enhanced Product Aggregate with SOLID Principles

```csharp
// src/NouAssist.Products.Domain/Aggregates/Product.cs
using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Products.Contracts;
using NouAssist.Products.Domain.Shared.Events;
using NouAssist.Products.Domain.Shared.Enums;
using NouAssist.Products.Domain.Shared.Exceptions;
using NouAssist.Products.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Products.Aggregates
{
    public class Product : FullAuditedAggregateRoot<Guid>, IMultiTenant, IWorkspaceEntity
    {
        public Guid? WorkspaceId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public string Category { get; private set; }  // Added for pricing strategies
        public ProductStatus Status { get; private set; }
        public bool IsPremiumOnly { get; set; }
        public Guid? TenantId { get; private set; }

        private readonly List<ProductVariant> _variants = new();
        public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

        protected Product() { /* ORM */ }

        public Product(Guid id, string name, string category, ProductStatus status, Guid? workspaceId = null) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Category = Check.NotNullOrWhiteSpace(category, nameof(category));
            Status = status;
            WorkspaceId = workspaceId;
        }

        // DIP: Depends on abstraction for validation
        public void Publish(DateTime publishedAt, IProductValidator validator)
        {
            var validation = validator.ValidateForPublication(this);
            if (!validation.IsValid)
            {
                throw new ProductDomainException("Products:ValidationFailed", 
                    string.Join("; ", validation.Errors));
            }
            
            Status = ProductStatus.Published;
            
            AddDistributedEvent(new ProductPublishedEto
            {
                ProductId = Id,
                ProductName = Name,
                TenantId = TenantId,
                PublishedAt = publishedAt,
                IsPremiumOnly = IsPremiumOnly
            });
        }

        public void AddVariant(ProductVariant variant)
        {
            if (_variants.Any(v => v.Sku == variant.Sku))
            {
                throw new ProductDomainException("Products:DuplicateSku", 
                    $"A variant with SKU '{variant.Sku}' already exists.");
            }
            _variants.Add(variant);
        }

        public ProductVariant GetVariant(string sku)
        {
            var variant = _variants.FirstOrDefault(v => v.Sku == sku);
            if (variant == null)
            {
                throw new ProductDomainException("Products:VariantNotFound", 
                    $"Variant with SKU '{sku}' not found.");
            }
            return variant;
        }

        public void Archive()
        {
            if (Status == ProductStatus.Archived)
            {
                return;
            }
            
            Status = ProductStatus.Archived;
        }

        public void UpdateName(string newName)
        {
            Name = Check.NotNullOrWhiteSpace(newName, nameof(newName));
        }
    }
}

// src/NouAssist.Products.Domain/Contracts/IProductValidator.cs
using NouAssist.Orders.ValueObjects;
using NouAssist.Products.Aggregates;

namespace NouAssist.Products.Contracts
{
    // ISP: Focused interface for product validation
    public interface IProductValidator
    {
        ValidationResult ValidateForPublication(Product product);
        ValidationResult ValidateForArchival(Product product);
    }
}

// src/NouAssist.Products.Domain/Services/ProductValidator.cs
using System.Linq;
using NouAssist.Orders.ValueObjects;
using NouAssist.Products.Aggregates;
using NouAssist.Products.Contracts;
using NouAssist.Products.Domain.Shared.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.Products.Services
{
    // SRP: Only responsible for product validation
    public class ProductValidator : DomainService, IProductValidator
    {
        public ValidationResult ValidateForPublication(Product product)
        {
            var result = new ValidationResult();
            
            if (product.Status == ProductStatus.Published)
            {
                return result; // Already published, no validation needed
            }
            
            if (!product.Variants.Any())
            {
                result.AddError("Cannot publish a product without variants.");
            }
            
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                result.AddError("Product name is required for publication.");
            }
            
            return result;
        }

        public ValidationResult ValidateForArchival(Product product)
        {
            var result = new ValidationResult();
            
            if (product.Status == ProductStatus.Archived)
            {
                return result; // Already archived
            }
            
            // Add any business rules for archival validation
            
            return result;
        }
    }
}
```

### 8. Enhanced Repository Interfaces (ISP)

```csharp
// src/NouAssist.Orders.Domain/Repositories/IOrderRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Orders.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Orders.Repositories
{
    // ISP: Basic repository operations
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<Order> GetWithLinesAsync(Guid id);
    }
}

// src/NouAssist.Orders.Domain/Repositories/IOrderQueryRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Orders.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.Orders.Repositories
{
    // ISP: Separate interface for complex queries
    public interface IOrderQueryRepository
    {
        Task<List<Order>> GetListAsync(ISpecification<Order> spec);
        Task<List<Order>> GetOrdersByCustomerAsync(Guid customerId);
        Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
    }
}

// src/NouAssist.Products.Domain/Repositories/IProductRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Products.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Products.Repositories
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        Task<Product> GetWithVariantsAsync(Guid id);
        Task<List<Product>> GetByCategoryAsync(string category);
    }
}

// src/NouAssist.Customers.Domain/Repositories/ICustomerRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Customers.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Customers.Repositories
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer> GetByEmailAsync(string email);
        Task<List<Customer>> GetActiveCustomersAsync();
    }
}
```

### 9. Domain.Shared Components

```csharp
// src/NouAssist.Orders.Domain.Shared/Enums/OrderStatus.cs
namespace NouAssist.Orders.Domain.Shared.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Processing = 2,
        Completed = 3,
        Cancelled = 4
    }
}

// src/NouAssist.Products.Domain.Shared/Enums/ProductStatus.cs
namespace NouAssist.Products.Domain.Shared.Enums
{
    public enum ProductStatus
    {
        Draft = 0,
        Published = 1,
        Archived = 2
    }
}

// src/NouAssist.Customers.Domain.Shared/Enums/CustomerType.cs
namespace NouAssist.Customers.Domain.Shared.Enums
{
    public enum CustomerType
    {
        Standard = 0,
        Premium = 1,
        Enterprise = 2
    }
}

// src/NouAssist.Orders.Domain.Shared/Exceptions/OrderDomainException.cs
using Volo.Abp;

namespace NouAssist.Orders.Domain.Shared.Exceptions
{
    public class OrderDomainException : BusinessException
    {
        public OrderDomainException(string code, string message = null) : base(code, message)
        {
        }
    }
}

// src/NouAssist.Products.Domain.Shared/Exceptions/ProductDomainException.cs
using Volo.Abp;

namespace NouAssist.Products.Domain.Shared.Exceptions
{
    public class ProductDomainException : BusinessException
    {
        public ProductDomainException(string code, string message = null) : base(code, message)
        {
        }
    }
}

// src/NouAssist.Customers.Domain.Shared/Exceptions/CustomerDomainException.cs
using Volo.Abp;

namespace NouAssist.Customers.Domain.Shared.Exceptions
{
    public class CustomerDomainException : BusinessException
    {
        public CustomerDomainException(string code, string message = null) : base(code, message)
        {
        }
    }
}

// src/NouAssist.Orders.Domain.Shared/Constants/OrderConstants.cs
namespace NouAssist.Orders.Domain.Shared.Constants
{
    public static class OrderConstants
    {
        public const int MaxOrderLinesPerOrder = 100;
        public const decimal MaxOrderAmount = 50000m;
        public const int OrderNumberLength = 10;
    }
}

// src/NouAssist.Products.Domain.Shared/Constants/ProductConstants.cs
namespace NouAssist.Products.Domain.Shared.Constants
{
    public static class ProductConstants
    {
        public const int MaxNameLength = 200;
        public const int MaxDescriptionLength = 2000;
        public const int MaxCategoryLength = 100;
        public const int MaxVariantsPerProduct = 50;
    }
}

// src/NouAssist.Customers.Domain.Shared/Constants/CustomerConstants.cs
namespace NouAssist.Customers.Domain.Shared.Constants
{
    public static class CustomerConstants
    {
        public const int MaxFirstNameLength = 100;
        public const int MaxLastNameLength = 100;
        public const int MaxEmailLength = 256;
        public const int MaxPhoneLength = 20;
    }
}
```

## Module Configuration for Dependency Injection

```csharp
// src/NouAssist.Orders.Domain/OrdersDomainModule.cs
using Microsoft.Extensions.DependencyInjection;
using NouAssist.Orders.Contracts;
using NouAssist.Orders.Factories;
using NouAssist.Orders.Services;
using NouAssist.Orders.Strategies;
using Volo.Abp.Modularity;

[DependsOn(typeof(OrdersDomainSharedModule))]
public class OrdersDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        
        // Register domain services following DIP
        services.AddTransient<IOrderValidator, OrderValidator>();
        services.AddTransient<IPriceCalculator, PriceCalculator>();
        services.AddTransient<ITaxCalculator, StandardTaxCalculator>();
        services.AddTransient<IOrderFactory, OrderFactory>();
        
        // Register strategies for OCP
        services.AddTransient<IDiscountStrategy, PremiumCustomerDiscountStrategy>();
        services.AddTransient<IDiscountStrategy, BulkOrderDiscountStrategy>();
        
        // Register domain services
        services.AddTransient<OrderProcessingService>();
    }
}
```

## Key SOLID Principles Improvements

1. **Single Responsibility Principle (SRP)**:
   - `OrderValidator` only handles validation logic
   - `PriceCalculator` only handles price calculations
   - `OrderFactory` only handles object creation
   - Each strategy class has a single responsibility

2. **Open/Closed Principle (OCP)**:
   - New discount strategies can be added without modifying existing code
   - New tax calculators can be implemented without changing the price calculator
   - Strategy pattern allows for easy extension

3. **Liskov Substitution Principle (LSP)**:
   - All discount strategies are interchangeable through the `IDiscountStrategy` interface
   - Tax calculators can be substituted without breaking functionality

4. **Interface Segregation Principle (ISP)**:
   - Small, focused interfaces (`IOrderValidator`, `IPriceCalculator`, `ITaxCalculator`)
   - Separate query repository interface for complex queries
   - Clients only depend on the methods they actually use

5. **Dependency Inversion Principle (DIP)**:
   - Aggregates depend on validator abstractions, not concrete implementations
   - Domain services are injected with their dependencies
   - High-level modules don't depend on low-level modules; both depend on abstractions

## File Organization Summary

### Domain.Shared Projects
- **Enums**: All enums in `*.Domain.Shared/Enums/` folders
- **Exceptions**: All domain exceptions in `*.Domain.Shared/Exceptions/` folders  
- **Constants**: All constants in `*.Domain.Shared/Constants/` folders
- **Events**: Integration events in `*.Domain.Shared/Events/` folders

### Domain Projects
- **Aggregates**: Root entities in `*/Aggregates/` folders
- **Entities**: Child entities in `*/Entities/` folders
- **ValueObjects**: Value objects in `*/ValueObjects/` folders
- **Repositories**: Repository interfaces in `*/Repositories/` folders
- **Services**: Domain services in `*/Services/` folders
- **Contracts**: Other domain interfaces in `*/Contracts/` folders
- **Factories**: Factory classes in `*/Factories/` folders
- **Strategies**: Strategy implementations in `*/Strategies/` folders
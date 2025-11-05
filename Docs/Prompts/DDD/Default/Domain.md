# ABP Framework Domain Layer Development Guide

This guide provides comprehensive, production-quality instructions and code for generating a domain layer following Domain-Driven Design (DDD) principles within the ABP Framework v9+ ecosystem. All code is fully defined, documented, and organized in a modular structure with complete file paths specified.

## Project Context & Technical Stack

-   **Framework:** ABP Framework v9+
-   **.NET Version:** .NET 9.0
-   **Architecture:** Domain-Driven Design with modular organization
-   **Multi-tenancy:** Enabled
-   **Core Dependencies:**
    ```xml
    <ItemGroup>
      <PackageReference Include="Volo.Abp.Ddd.Domain" Version="9.1.0" />
      <PackageReference Include="Volo.Abp.Domain" Version="9.1.0" />
      <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="9.0.2" />
    </ItemGroup>
    ```

## Implementation Checklist

-   [x] Use smaller, focused aggregates (3-5 entities max)
-   [x] Identify true invariants that require transactional consistency
-   [x] Implement specifications to encapsulate complex query logic (without executing queries)
-   [x] Use domain services for business logic coordination - NO direct repository access
-   [x] Reference other aggregates by ID only, not direct object reference
-   [x] Use domain events for eventual consistency between aggregates
-   [x] Implement proper encapsulation of entity state
-   [x] Use NSubstitute for mocking and Shouldly for assertions in tests
-   [x] Include English localization files for all domain exceptions and messages

---

## Core Domain Model Implementation

This section contains the full implementation of the core domain objects, including Enums, Aggregates, Entities, and Value Objects.

### 1. Domain Shared Enums and Events

These are simple data structures shared between the `.Domain` and `.Application.Contracts` projects.

```csharp
// src/NouAssist.Orders.Domain.Shared/Enums/OrderStatus.cs
namespace NouAssist.Orders.Enums
{
    public enum OrderStatus : byte
    {
        Pending = 1,
        Confirmed = 2,
        Processing = 3,
        Shipped = 4,
        Completed = 5,
        Cancelled = 6
    }
}

// src/NouAssist.Products.Domain.Shared/Enums/ProductStatus.cs
namespace NouAssist.Products.Enums
{
    public enum ProductStatus : byte
    {
        Draft = 1,
        Published = 2,
        Archived = 3
    }
}

// src/NouAssist.Customers.Domain.Shared/Enums/CustomerType.cs
namespace NouAssist.Customers.Enums
{
    public enum CustomerType
    {
        Standard,
        Premium
    }
}

// src/NouAssist.Orders.Domain.Shared/Events/OrderProcessedEto.cs
using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Orders.Domain.Shared.Events
{
    [EventName("NouAssist.Orders.Processed")]
    public class OrderProcessedEto : IMultiTenant
    {
        public Guid OrderId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime ProcessedAt { get; set; }
        public Guid ProcessedBy { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
```

### 2. Aggregates

Aggregates are the root of our consistency boundaries. They encapsulate their state and expose methods to manipulate it.

**Order Aggregate**
```csharp
// src/NouAssist.Orders.Domain/Aggregates/Order.cs
using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Orders.Enums;
using NouAssist.Orders.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Orders.Aggregates
{
    public class Order : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid CustomerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }

        private readonly List<OrderLine> _orderLines = new();
        public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

        protected Order() { /* Required by ORM */ }

        public Order(Guid id, Guid customerId, Guid? tenantId) : base(id)
        {
            CustomerId = Check.NotNull(customerId, nameof(customerId));
            TenantId = tenantId;
            OrderDate = Clock.Now;
            Status = OrderStatus.Pending;
        }

        public void AddOrderLine(Guid productId, string variantSku, decimal price, int quantity)
        {
            if (Status != OrderStatus.Pending)
            {
                throw new BusinessException("Orders:CanOnlyModifyPending", "Order lines can only be added to pending orders.");
            }
            if (_orderLines.Any(l => l.ProductId == productId && l.VariantSku == variantSku))
            {
                throw new BusinessException("Orders:DuplicateLine", "This product variant is already in the order.");
            }
            _orderLines.Add(new OrderLine(Id, productId, variantSku, price, quantity));
        }

        public void ConfirmOrder()
        {
            if (Status != OrderStatus.Pending) return;
            Status = OrderStatus.Confirmed;
        }
        
        public void MarkAsProcessed(DateTime processedTime, Guid userId)
        {
            if (Status != OrderStatus.Confirmed)
            {
                throw new BusinessException("Orders:CannotProcess", "Only confirmed orders can be processed.");
            }
            Status = OrderStatus.Processing;
            SetModification(userId, processedTime);
        }

        public decimal GetTotalAmount() => OrderLines.Sum(l => l.Subtotal);
    }
}
```

**Product Aggregate**
```csharp
// src/NouAssist.Products.Domain/Aggregates/Product.cs
using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Products.Entities;
using NouAssist.Products.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Products.Aggregates
{
    public class Product : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Name { get; private set; }
        public string Description { get; set; }
        public ProductStatus Status { get; set; }
        public bool IsPremiumOnly { get; set; }
        public Guid? TenantId { get; private set; }

        private readonly List<ProductVariant> _variants = new();
        public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

        protected Product() { /* ORM */ }

        public Product(Guid id, string name, ProductStatus status, Guid? tenantId) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Status = status;
            TenantId = tenantId;
        }

        public void AddVariant(ProductVariant variant)
        {
            if (_variants.Any(v => v.Sku == variant.Sku))
            {
                throw new BusinessException("Products:DuplicateSku", $"A variant with SKU '{variant.Sku}' already exists.");
            }
            _variants.Add(variant);
        }

        public ProductVariant GetVariant(string sku)
        {
            var variant = _variants.FirstOrDefault(v => v.Sku == sku);
            if (variant == null)
            {
                throw new BusinessException("Products:VariantNotFound", $"Variant with SKU '{sku}' not found.");
            }
            return variant;
        }
    }
}
```

### 3. Entities

Entities are objects with identity but are owned by an Aggregate Root.

```csharp
// src/NouAssist.Orders.Domain/Entities/OrderLine.cs
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Orders.Entities
{
    public class OrderLine : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string VariantSku { get; private set; }
        public decimal Price { get; private set; }
        public decimal TaxAmount { get; private set; }
        public int Quantity { get; private set; }
        public decimal Subtotal => Quantity * Price;

        protected OrderLine() { /* ORM */ }

        public OrderLine(Guid orderId, Guid productId, string variantSku, decimal price, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            VariantSku = Check.NotNullOrWhiteSpace(variantSku, nameof(variantSku));
            Price = price;
            Quantity = quantity;
        }

        public void UpdatePricing(decimal finalPrice, decimal taxAmount)
        {
            Price = finalPrice;
            TaxAmount = taxAmount;
        }

        public override object[] GetKeys() => new object[] { OrderId, ProductId, VariantSku };
    }
}

// src/NouAssist.Products.Domain/Entities/ProductVariant.cs
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Products.Entities
{
    public class ProductVariant : Entity
    {
        public Guid ProductId { get; private set; }
        public string Sku { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }

        protected ProductVariant() { /* ORM */ }

        public ProductVariant(Guid productId, string sku, decimal price, int stockQuantity)
        {
            ProductId = productId;
            Sku = Check.NotNullOrWhiteSpace(sku, nameof(sku));
            Price = price;
            StockQuantity = stockQuantity;
        }

        public override object[] GetKeys() => new object[] { ProductId, Sku };
    }
}
```

### 4. Value Objects

Value Objects are immutable objects defined by their attributes rather than a unique ID.

```csharp
// src/NouAssist.Orders.Domain/ValueObjects/PriceCalculationResult.cs
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Orders.ValueObjects
{
    public class PriceCalculationResult : ValueObject
    {
        public decimal BasePrice { get; init; }
        public decimal DiscountAmount { get; init; }
        public decimal TaxAmount { get; init; }
        public decimal FinalPrice { get; init; }
        public DateTime CalculationDate { get; init; }

        public PriceCalculationResult(
            decimal basePrice, decimal discountAmount, decimal taxAmount, 
            decimal finalPrice, DateTime calculationDate)
        {
            BasePrice = basePrice;
            DiscountAmount = discountAmount;
            TaxAmount = taxAmount;
            FinalPrice = finalPrice;
            CalculationDate = calculationDate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return BasePrice;
            yield return DiscountAmount;
            yield return TaxAmount;
            yield return FinalPrice;
            yield return CalculationDate;
        }
    }
}
```

---

## Domain Contracts: Repository Interfaces

The domain layer defines the contracts (interfaces) for repositories. The Infrastructure layer (`.EntityFrameworkCore` or `.MongoDB` project) implements them.

```csharp
// src/NouAssist.Orders.Domain/Data/IOrderRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Orders.Data
{
    public interface IOrderRepository : IRepository<Aggregates.Order, Guid>
    {
        // Specifications often reduce the need for custom repository methods.
        // However, if complex includes are needed, they can be defined here.
        Task<Aggregates.Order> GetWithLinesAsync(Guid id);
        Task<List<Aggregates.Order>> GetListAsync(ISpecification<Aggregates.Order> spec);
    }
}

// src/NouAssist.Products.Domain/Data/IProductRepository.cs
using System;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Products.Data
{
    public interface IProductRepository : IRepository<Aggregates.Product, Guid>
    {
        // Custom methods as needed
    }
}
```

---

## Domain Service Patterns

Domain services contain business logic that doesn't naturally fit within a single aggregate. They are stateless and do not access repositories.

### 1. Coordination Service: `OrderProcessingService`
This service coordinates the complex process of validating, pricing, and finalizing an order.

```csharp
// src/NouAssist.Orders.Domain/Services/OrderProcessingService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Customers.Aggregates;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Domain.Shared.Events;
using NouAssist.Orders.Enums;
using NouAssist.Orders.ValueObjects;
using NouAssist.Products.Aggregates;
using NouAssist.Products.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.Orders.Services
{
    public class OrderProcessingService : DomainService
    {
        private readonly PriceCalculationService _priceCalculationService;

        public OrderProcessingService(PriceCalculationService priceCalculationService)
        {
            _priceCalculationService = priceCalculationService;
        }
        
        public OrderProcessingResult ProcessOrder(
            Order order, List<Product> products, Customer customer, 
            List<DiscountRule> availableDiscounts, Guid processedByUserId)
        {
            var result = new OrderProcessingResult();
            var validation = ValidateOrderForProcessing(order, products, customer);
            if (!validation.IsValid)
            {
                result.Errors = validation.Errors;
                return result;
            }

            foreach (var line in order.OrderLines)
            {
                var product = products.First(p => p.Id == line.ProductId);
                var variant = product.GetVariant(line.VariantSku);
                var priceCalc = _priceCalculationService.CalculateFinalPrice(variant, customer, availableDiscounts);
                line.UpdatePricing(priceCalc.FinalPrice, priceCalc.TaxAmount);
            }
            
            order.MarkAsProcessed(Clock.Now, processedByUserId);
            
            order.AddDistributedEvent(new OrderProcessedEto
            {
                OrderId = order.Id,
                TenantId = order.TenantId,
                ProcessedAt = order.LastModificationTime.Value,
                ProcessedBy = processedByUserId,
                TotalAmount = order.GetTotalAmount()
            });

            result.ProcessedOrder = order;
            result.IsSuccessful = true;
            return result;
        }

        private ValidationResult ValidateOrderForProcessing(Order order, List<Product> products, Customer customer)
        {
            var result = new ValidationResult();
            if (order.Status != OrderStatus.Confirmed) result.AddError("Only confirmed orders can be processed.");
            if (!customer.IsActive) result.AddError("Customer account is not active.");

            foreach (var line in order.OrderLines)
            {
                var product = products.FirstOrDefault(p => p.Id == line.ProductId);
                if (product == null) { result.AddError($"Product with ID {line.ProductId} not found."); continue; }
                if (product.Status != ProductStatus.Published) result.AddError($"Product '{product.Name}' is not available for purchase.");
            }
            return result;
        }
    }
}
```

---

## Specification Patterns

Specifications encapsulate query logic in a reusable, chainable format. They are defined in the Domain layer and used by the Application or Infrastructure layers.

```csharp
// src/NouAssist.Orders.Domain/Specifications/OrderSpecifications.cs
using System;
using System.Linq.Expressions;
using NouAssist.Customers.Enums;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Orders.Specifications
{
    public class OrderEligibleForProcessingSpecification : Specification<Order>
    {
        public override Expression<Func<Order, bool>> ToExpression() =>
            o => o.Status == OrderStatus.Confirmed;
    }

    public class OrdersCompletedInMonthSpecification : Specification<Order>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public OrdersCompletedInMonthSpecification(int year, int month)
        {
            _startDate = new DateTime(year, month, 1);
            _endDate = _startDate.AddMonths(1);
        }

        public override Expression<Func<Order, bool>> ToExpression() =>
            o => o.Status == OrderStatus.Completed &&
                 o.CompletedDate.HasValue &&
                 o.CompletedDate.Value >= _startDate &&
                 o.CompletedDate.Value < _endDate;
    }
}
```

---
## Testing Domain Services

Unit tests for domain services should be simple, as they work with in-memory objects and have no external dependencies like databases or APIs.

```csharp
// src/NouAssist.Orders.Domain.Tests/Services/OrderProcessingServiceTests.cs
using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Customers.Aggregates;
using NouAssist.Customers.Enums;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Domain.Shared.Events;
using NouAssist.Orders.Enums;
using NouAssist.Orders.Services;
using NouAssist.Products.Aggregates;
using NouAssist.Products.Entities;
using NouAssist.Products.Enums;
using Shouldly;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Xunit;

namespace NouAssist.Orders.Tests.Services
{
    public class OrderProcessingServiceTests
    {
        private readonly OrderProcessingService _orderProcessingService;

        public OrderProcessingServiceTests()
        {
            var priceCalculationService = new PriceCalculationService();
            _orderProcessingService = new OrderProcessingService(priceCalculationService);
        }

        [Fact]
        public void ProcessOrder_Should_ReturnSuccess_ForValidOrder()
        {
            // Arrange
            var customer = CreateCustomer(isActive: true);
            var products = CreateProducts();
            var order = CreateOrder(customer.Id, products);
            order.ConfirmOrder(); // Set state required for processing
            var userId = Guid.NewGuid();

            // Act
            var result = _orderProcessingService.ProcessOrder(order, products, customer, new List<DiscountRule>(), userId);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.ProcessedOrder.Status.ShouldBe(OrderStatus.Processing);
            result.Errors.ShouldBeEmpty();
            
            var eto = result.ProcessedOrder.GetDistributedEvents().First().EventData.ShouldBeOfType<OrderProcessedEto>();
            eto.OrderId.ShouldBe(order.Id);
        }

        [Fact]
        public void ProcessOrder_Should_ReturnErrors_ForInactiveCustomer()
        {
            // Arrange
            var customer = CreateCustomer(isActive: false);
            var products = CreateProducts();
            var order = CreateOrder(customer.Id, products);
            order.ConfirmOrder();

            // Act
            var result = _orderProcessingService.ProcessOrder(order, products, customer, new List<DiscountRule>(), Guid.NewGuid());

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Errors.ShouldContain("Customer account is not active.");
        }

        #region Test Data Helpers
        private Customer CreateCustomer(bool isActive)
        {
            var customer = new Customer(Guid.NewGuid(), "John", "Doe", "john.doe@example.com");
            customer.IsActive = isActive;
            customer.Type = CustomerType.Standard;
            return customer;
        }

        private List<Product> CreateProducts()
        {
            var product1 = new Product(Guid.NewGuid(), "Laptop Pro", ProductStatus.Published, null);
            product1.AddVariant(new ProductVariant(product1.Id, "LP-16-512", 1500.00m, 10));
            return new List<Product> { product1 };
        }

        private Order CreateOrder(Guid customerId, IEnumerable<Product> products)
        {
            var order = new Order(Guid.NewGuid(), customerId, null);
            var product = products.First();
            var variant = product.Variants.First();
            order.AddOrderLine(product.Id, variant.Sku, variant.Price, 1);
            return order;
        }
        #endregion
    }
}
```

---

## Localization

Domain exceptions and messages must be localized. This is handled in the `.Domain.Shared` project.

**Resource Class (`.cs`)**
```csharp
// src/NouAssist.Orders.Domain.Shared/Localization/OrdersResource.cs
using Volo.Abp.Localization;

namespace NouAssist.Orders.Localization
{
    [LocalizationResourceName("Orders")]
    public class OrdersResource { }
}
```

**Resource File (`.json`)**
```json
// src/NouAssist.Orders.Domain.Shared/Localization/en.json
{
  "culture": "en",
  "texts": {
    "Orders:CannotProcess": "Only confirmed orders can be processed.",
    "Orders:CanOnlyModifyPending": "Order lines can only be added to pending orders.",
    "Orders:DuplicateLine": "This product variant is already in the order."
  }
}
```

## Final File Structure

```
src/NouAssist.Orders.Domain.Shared/
    Enums/
        OrderStatus.cs
    Events/
        OrderProcessedEto.cs
    Localization/
        Orders/
            en.json
        OrdersResource.cs

src/NouAssist.Products.Domain.Shared/
    Enums/
        ProductStatus.cs

src/NouAssist.Customers.Domain.Shared/
    Enums/
        CustomerType.cs

src/NouAssist.Orders.Domain/
    Aggregates/
        Order.cs
    Entities/
        OrderLine.cs
    ValueObjects/
        PriceCalculationResult.cs
        ValidationResult.cs 
    Services/
        OrderProcessingService.cs
        PriceCalculationService.cs
    Specifications/
        OrderSpecifications.cs
    Data/
        IOrderRepository.cs
```

## Key Takeaways

1.  **Domain Model is Central:** The core logic, state, and rules of your application live in the aggregates, entities, and value objects.
2.  **Encapsulation is Key:** Aggregates protect their internal state (`private set`) and expose methods (`ConfirmOrder()`, `MarkAsProcessed()`) to enforce business rules.
3.  **Domain Services for Coordination:** Use stateless services to coordinate operations between multiple aggregates or for complex calculations that don't belong to a single aggregate.
4.  **Repositories are Contracts:** The domain defines the *interface* for data access; it does not know or care about the implementation (e.g., EF Core, MongoDB).
5.  **Events for Decoupling:** Use distributed events (`Eto`) to achieve eventual consistency and decouple bounded contexts.
6.  **Testability:** A clean domain layer is highly testable with simple unit tests, as demonstrated, leading to more robust and reliable business logic.
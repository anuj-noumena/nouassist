# ABP Framework v9+ Application Layer Development Guide

This guide provides production-ready code and patterns for developing the Application Layer within an ABP Framework v9+ module, with a strong emphasis on SOLID principles for maintainable, testable, and extensible code.

## Key Architectural Principles + SOLID Integration

* **Single Responsibility Principle (SRP):** Each class has one reason to change. Application services orchestrate, but delegate specific concerns to dedicated services.
* **Open/Closed Principle (OCP):** Code is open for extension but closed for modification through abstractions and strategy patterns.
* **Liskov Substitution Principle (LSP):** Interfaces can be substituted seamlessly without breaking functionality.
* **Interface Segregation Principle (ISP):** Clients depend only on interfaces they actually use.
* **Dependency Inversion Principle (DIP):** High-level modules don't depend on low-level modules; both depend on abstractions.

## Technical Context

* **Framework & Platform:** ABP Framework v9+ (e.g., v9.1.1), .NET 9.0, Multi-tenancy enabled.
* **Core NuGet Packages:** [Same as original]

---

## I. Application Contracts Layer (`.Application.Contracts`) - ISP Enhanced

### 1. Interface Segregation - Focused Service Interfaces

Instead of one large interface, we split responsibilities into focused, cohesive interfaces:

```csharp
// src/NouAssist.Orders.Application.Contracts/Orders/IOrderQueryService.cs
using System;
using System.Threading.Tasks;
using NouAssist.Orders.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Orders.Orders
{
    public interface IOrderQueryService : IApplicationService
    {
        Task<OrderDto> GetAsync(Guid id);
        Task<PagedResultDto<OrderDto>> GetListAsync(GetOrderListDto input);
    }
}
```

```csharp
// src/NouAssist.Orders.Application.Contracts/Orders/IOrderCommandService.cs
using System;
using System.Threading.Tasks;
using NouAssist.Orders.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Orders.Orders
{
    public interface IOrderCommandService : IApplicationService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto input);
        Task<OrderDto> UpdateStatusAsync(Guid id, UpdateOrderStatusDto input);
        Task DeleteAsync(Guid id);
    }
}
```

```csharp
// src/NouAssist.Orders.Application.Contracts/Orders/IOrderProcessingService.cs
using System;
using System.Threading.Tasks;
using NouAssist.Orders.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Orders.Orders
{
    public interface IOrderProcessingService : IApplicationService
    {
        Task<OrderDto> ProcessOrderAsync(Guid id);
        Task<OrderDto> FulfillOrderAsync(Guid id);
        Task<OrderDto> CancelOrderAsync(Guid id, string reason);
    }
}
```

### 2. DTOs with Validation Abstractions

```csharp
// src/NouAssist.Orders.Application.Contracts/Orders/Dtos/CreateOrderDto.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Orders.Dtos
{
    public class CreateOrderDto : IValidatableObject
    {
        [Required]
        public Guid CustomerId { get; set; }
        
        [Required]
        public List<CreateOrderLineDto> OrderLines { get; set; } = new();
        
        public string? Notes { get; set; }
        public Guid? WorkspaceId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OrderLines == null || OrderLines.Count == 0)
            {
                yield return new ValidationResult(
                    "Order must have at least one line item.",
                    new[] { nameof(OrderLines) }
                );
            }
        }
    }
}
```

---

## II. Application Layer (`.Application`) - SOLID Implementation

### 1. SRP - Dedicated Service Abstractions

```csharp
// src/NouAssist.Orders.Application/Orders/Abstractions/IOrderDataLoader.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Customers.Aggregates;
using NouAssist.Orders.Aggregates;
using NouAssist.Products.Aggregates;

namespace NouAssist.Orders.Orders.Abstractions
{
    public interface IOrderDataLoader
    {
        Task<Order> LoadOrderWithDetailsAsync(Guid orderId);
        Task<Customer> LoadCustomerAsync(Guid customerId);
        Task<List<Product>> LoadProductsAsync(List<Guid> productIds);
        Task<OrderDataContext> LoadOrderProcessingDataAsync(Guid orderId);
    }

    public class OrderDataContext
    {
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; }
        public List<DiscountRule> AvailableDiscounts { get; set; }
    }
}
```

```csharp
// src/NouAssist.Orders.Application/Orders/Abstractions/IOrderValidator.cs
using System.Threading.Tasks;
using NouAssist.Orders.Dtos;
using Volo.Abp.Validation;

namespace NouAssist.Orders.Orders.Abstractions
{
    public interface IOrderValidator
    {
        Task ValidateCreateOrderAsync(CreateOrderDto input);
        Task ValidateOrderStatusUpdateAsync(Guid orderId, UpdateOrderStatusDto input);
    }
}
```

```csharp
// src/NouAssist.Orders.Application/Orders/Abstractions/IOrderSpecificationBuilder.cs
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Dtos;
using Volo.Abp.Specifications;

namespace NouAssist.Orders.Orders.Abstractions
{
    public interface IOrderSpecificationBuilder
    {
        ISpecification<Order> BuildListSpecification(GetOrderListDto input);
        ISpecification<Order> BuildSearchSpecification(string searchTerm);
    }
}
```

### 2. SRP - Implementation of Service Abstractions

```csharp
// src/NouAssist.Orders.Application/Orders/Services/OrderDataLoader.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Customers.Aggregates;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Orders.Abstractions;
using NouAssist.Orders.Specifications;
using NouAssist.Products.Aggregates;
using NouAssist.Products.Specifications;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Orders.Orders.Services
{
    public class OrderDataLoader : IOrderDataLoader, ITransientDependency
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IReadOnlyRepository<Customer, Guid> _customerRepository;
        private readonly IReadOnlyRepository<Product, Guid> _productRepository;

        public OrderDataLoader(
            IRepository<Order, Guid> orderRepository,
            IReadOnlyRepository<Customer, Guid> customerRepository,
            IReadOnlyRepository<Product, Guid> productRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<Order> LoadOrderWithDetailsAsync(Guid orderId)
        {
            return await _orderRepository.GetAsync(orderId, includeDetails: true);
        }

        public async Task<Customer> LoadCustomerAsync(Guid customerId)
        {
            return await _customerRepository.GetAsync(customerId);
        }

        public async Task<List<Product>> LoadProductsAsync(List<Guid> productIds)
        {
            return await _productRepository.GetListAsync(new ProductsByIdsSpecification(productIds));
        }

        public async Task<OrderDataContext> LoadOrderProcessingDataAsync(Guid orderId)
        {
            var order = await LoadOrderWithDetailsAsync(orderId);
            var customer = await LoadCustomerAsync(order.CustomerId);
            var productIds = order.OrderLines.Select(l => l.ProductId).ToList();
            var products = await LoadProductsAsync(productIds);
            
            return new OrderDataContext
            {
                Order = order,
                Customer = customer,
                Products = products,
                AvailableDiscounts = new List<DiscountRule>() // Load from business rules
            };
        }
    }
}
```

```csharp
// src/NouAssist.Orders.Application/Orders/Services/OrderValidator.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Orders.Dtos;
using NouAssist.Orders.Orders.Abstractions;
using NouAssist.Products.Specifications;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace NouAssist.Orders.Orders.Services
{
    public class OrderValidator : IOrderValidator, ITransientDependency
    {
        private readonly IOrderDataLoader _dataLoader;
        private readonly IReadOnlyRepository<Product, Guid> _productRepository;

        public OrderValidator(
            IOrderDataLoader dataLoader,
            IReadOnlyRepository<Product, Guid> productRepository)
        {
            _dataLoader = dataLoader;
            _productRepository = productRepository;
        }

        public async Task ValidateCreateOrderAsync(CreateOrderDto input)
        {
            await ValidateCustomerExistsAsync(input.CustomerId);
            await ValidateProductsExistAsync(input.OrderLines.Select(l => l.ProductId).ToList());
            await ValidateProductVariantsAsync(input.OrderLines);
        }

        public async Task ValidateOrderStatusUpdateAsync(Guid orderId, UpdateOrderStatusDto input)
        {
            var order = await _dataLoader.LoadOrderWithDetailsAsync(orderId);
            
            if (!order.CanUpdateStatus(input.NewStatus))
            {
                throw new BusinessException($"Cannot update order status from {order.Status} to {input.NewStatus}");
            }
        }

        private async Task ValidateCustomerExistsAsync(Guid customerId)
        {
            var customer = await _dataLoader.LoadCustomerAsync(customerId);
            if (customer == null)
            {
                throw new EntityNotFoundException(typeof(Customer), customerId);
            }
        }

        private async Task ValidateProductsExistAsync(List<Guid> productIds)
        {
            var products = await _productRepository.GetListAsync(new ProductsByIdsSpecification(productIds));
            var missingIds = productIds.Except(products.Select(p => p.Id)).ToList();
            
            if (missingIds.Any())
            {
                throw new BusinessException($"Products not found: {string.Join(", ", missingIds)}");
            }
        }

        private async Task ValidateProductVariantsAsync(List<CreateOrderLineDto> orderLines)
        {
            var productIds = orderLines.Select(l => l.ProductId).ToList();
            var products = await _productRepository.GetListAsync(new ProductsByIdsSpecification(productIds));
            
            foreach (var line in orderLines)
            {
                var product = products.FirstOrDefault(p => p.Id == line.ProductId);
                if (product?.GetVariant(line.VariantSku) == null)
                {
                    throw new BusinessException($"Product variant '{line.VariantSku}' not found for product {line.ProductId}");
                }
            }
        }
    }
}
```

```csharp
// src/NouAssist.Orders.Application/Orders/Services/OrderSpecificationBuilder.cs
using System;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Dtos;
using NouAssist.Orders.Orders.Abstractions;
using NouAssist.Orders.Specifications;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Specifications;

namespace NouAssist.Orders.Orders.Services
{
    public class OrderSpecificationBuilder : IOrderSpecificationBuilder, ITransientDependency
    {
        public ISpecification<Order> BuildListSpecification(GetOrderListDto input)
        {
            var spec = Specification<Order>.Empty;

            if (input.CustomerId.HasValue)
            {
                spec = spec.And(new OrdersByCustomerSpecification(input.CustomerId.Value));
            }

            if (input.Status.HasValue)
            {
                spec = spec.And(new Specification<Order>(o => o.Status == input.Status.Value));
            }

            if (input.FromDate.HasValue)
            {
                spec = spec.And(new Specification<Order>(o => o.OrderDate >= input.FromDate.Value));
            }

            if (input.ToDate.HasValue)
            {
                spec = spec.And(new Specification<Order>(o => o.OrderDate <= input.ToDate.Value));
            }

            return spec;
        }

        public ISpecification<Order> BuildSearchSpecification(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Specification<Order>.Empty;
            }

            return new Specification<Order>(o => 
                o.OrderNumber.Contains(searchTerm) || 
                o.Notes.Contains(searchTerm));
        }
    }
}
```

### 3. OCP - Strategy Pattern for Order Processing

```csharp
// src/NouAssist.Orders.Application/Orders/Abstractions/IOrderProcessingStrategy.cs
using System.Threading.Tasks;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Orders.Abstractions;

namespace NouAssist.Orders.Orders.Abstractions
{
    public interface IOrderProcessingStrategy
    {
        Task<OrderProcessingResult> ProcessAsync(OrderDataContext context, Guid userId);
        bool CanProcess(Order order);
    }

    public class OrderProcessingResult
    {
        public bool IsSuccessful { get; set; }
        public Order ProcessedOrder { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }
}
```

```csharp
// src/NouAssist.Orders.Application/Orders/Strategies/StandardOrderProcessingStrategy.cs
using System;
using System.Threading.Tasks;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Orders.Abstractions;
using NouAssist.Orders.Services;
using Volo.Abp.DependencyInjection;

namespace NouAssist.Orders.Orders.Strategies
{
    public class StandardOrderProcessingStrategy : IOrderProcessingStrategy, ITransientDependency
    {
        private readonly OrderProcessingService _domainService;

        public StandardOrderProcessingStrategy(OrderProcessingService domainService)
        {
            _domainService = domainService;
        }

        public async Task<OrderProcessingResult> ProcessAsync(OrderDataContext context, Guid userId)
        {
            var domainResult = _domainService.ProcessOrder(
                context.Order, 
                context.Products, 
                context.Customer, 
                context.AvailableDiscounts, 
                userId);

            return new OrderProcessingResult
            {
                IsSuccessful = domainResult.IsSuccessful,
                ProcessedOrder = domainResult.ProcessedOrder,
                Errors = domainResult.Errors,
                Warnings = domainResult.Warnings
            };
        }

        public bool CanProcess(Order order)
        {
            return order.Status == OrderStatus.Confirmed;
        }
    }
}
```

### 4. DIP - Main Application Services

```csharp
// src/NouAssist.Orders.Application/Orders/OrderQueryService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Orders.Dtos;
using NouAssist.Orders.Orders.Abstractions;
using NouAssist.Orders.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Orders.Orders
{
    [Authorize(OrdersPermissions.Orders.Default)]
    public class OrderQueryService : ApplicationService, IOrderQueryService
    {
        private readonly IOrderDataLoader _dataLoader;
        private readonly IOrderSpecificationBuilder _specificationBuilder;
        private readonly IRepository<Order, Guid> _orderRepository;

        public OrderQueryService(
            IOrderDataLoader dataLoader,
            IOrderSpecificationBuilder specificationBuilder,
            IRepository<Order, Guid> orderRepository)
        {
            _dataLoader = dataLoader;
            _specificationBuilder = specificationBuilder;
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> GetAsync(Guid id)
        {
            var order = await _dataLoader.LoadOrderWithDetailsAsync(id);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<PagedResultDto<OrderDto>> GetListAsync(GetOrderListDto input)
        {
            var specification = _specificationBuilder.BuildListSpecification(input);
            
            var orders = await _orderRepository.GetListAsync(specification, 
                includeDetails: true,
                maxResultCount: input.MaxResultCount,
                skipCount: input.SkipCount);
                
            var totalCount = await _orderRepository.CountAsync(specification);

            return new PagedResultDto<OrderDto>(
                totalCount,
                ObjectMapper.Map<List<Order>, List<OrderDto>>(orders)
            );
        }
    }
}
```

```csharp
// src/NouAssist.Orders.Application/Orders/OrderCommandService.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Dtos;
using NouAssist.Orders.Orders.Abstractions;
using NouAssist.Orders.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Orders.Orders
{
    [Authorize(OrdersPermissions.Orders.Default)]
    public class OrderCommandService : ApplicationService, IOrderCommandService
    {
        private readonly IOrderDataLoader _dataLoader;
        private readonly IOrderValidator _validator;
        private readonly IRepository<Order, Guid> _orderRepository;

        public OrderCommandService(
            IOrderDataLoader dataLoader,
            IOrderValidator validator,
            IRepository<Order, Guid> orderRepository)
        {
            _dataLoader = dataLoader;
            _validator = validator;
            _orderRepository = orderRepository;
        }

        [Authorize(OrdersPermissions.Orders.Create)]
        public async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            // 1. Validate input
            await _validator.ValidateCreateOrderAsync(input);

            // 2. Load required data
            var products = await _dataLoader.LoadProductsAsync(
                input.OrderLines.Select(l => l.ProductId).ToList());

            // 3. Create aggregate through domain logic
            var order = new Order(GuidGenerator.Create(), input.CustomerId, input.WorkspaceId);

            foreach (var lineDto in input.OrderLines)
            {
                var product = products.First(p => p.Id == lineDto.ProductId);
                var variant = product.GetVariant(lineDto.VariantSku);
                order.AddOrderLine(product.Id, variant.Sku, variant.Price, lineDto.Quantity);
            }

            // 4. Persist
            await _orderRepository.InsertAsync(order);

            // 5. Return mapped result
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        [Authorize(OrdersPermissions.Orders.UpdateStatus)]
        public async Task<OrderDto> UpdateStatusAsync(Guid id, UpdateOrderStatusDto input)
        {
            // 1. Validate
            await _validator.ValidateOrderStatusUpdateAsync(id, input);

            // 2. Load and update through domain logic
            var order = await _dataLoader.LoadOrderWithDetailsAsync(id);
            order.UpdateStatus(input.NewStatus, input.CancellationReason);

            // 3. Persist
            await _orderRepository.UpdateAsync(order);

            // 4. Return result
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        [Authorize(OrdersPermissions.Orders.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }
    }
}
```

```csharp
// src/NouAssist.Orders.Application/Orders/OrderProcessingService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Orders.Dtos;
using NouAssist.Orders.Orders.Abstractions;
using NouAssist.Orders.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Orders.Orders
{
    [Authorize(OrdersPermissions.Orders.Process)]
    public class OrderProcessingService : ApplicationService, IOrderProcessingService
    {
        private readonly IOrderDataLoader _dataLoader;
        private readonly IEnumerable<IOrderProcessingStrategy> _processingStrategies;
        private readonly IRepository<Order, Guid> _orderRepository;

        public OrderProcessingService(
            IOrderDataLoader dataLoader,
            IEnumerable<IOrderProcessingStrategy> processingStrategies,
            IRepository<Order, Guid> orderRepository)
        {
            _dataLoader = dataLoader;
            _processingStrategies = processingStrategies;
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> ProcessOrderAsync(Guid id)
        {
            // 1. Load all required data
            var context = await _dataLoader.LoadOrderProcessingDataAsync(id);

            // 2. Find appropriate strategy (OCP in action)
            var strategy = _processingStrategies.FirstOrDefault(s => s.CanProcess(context.Order));
            if (strategy == null)
            {
                throw new BusinessException($"No processing strategy found for order {id} in status {context.Order.Status}");
            }

            // 3. Execute strategy
            var result = await strategy.ProcessAsync(context, CurrentUser.Id.Value);

            if (!result.IsSuccessful)
            {
                throw new UserFriendlyException(string.Join(", ", result.Errors));
            }

            // 4. Persist and return
            await _orderRepository.UpdateAsync(result.ProcessedOrder);
            return ObjectMapper.Map<Order, OrderDto>(result.ProcessedOrder);
        }

        public async Task<OrderDto> FulfillOrderAsync(Guid id)
        {
            var order = await _dataLoader.LoadOrderWithDetailsAsync(id);
            order.FulfillOrder(CurrentUser.Id.Value);
            await _orderRepository.UpdateAsync(order);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<OrderDto> CancelOrderAsync(Guid id, string reason)
        {
            var order = await _dataLoader.LoadOrderWithDetailsAsync(id);
            order.CancelOrder(reason, CurrentUser.Id.Value);
            await _orderRepository.UpdateAsync(order);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }
    }
}
```

---

## III. SOLID Principles Summary

### Single Responsibility Principle (SRP)
- **OrderQueryService:** Only handles read operations
- **OrderCommandService:** Only handles write operations  
- **OrderProcessingService:** Only handles complex business workflows
- **OrderDataLoader:** Only loads data from repositories
- **OrderValidator:** Only validates business rules
- **OrderSpecificationBuilder:** Only builds query specifications

### Open/Closed Principle (OCP)
- **Strategy Pattern:** New processing strategies can be added without modifying existing code
- **Specification Pattern:** New query conditions can be added through new specifications
- **Validation:** New validation rules can be added through new validators

### Liskov Substitution Principle (LSP)
- All implementations can be substituted through their interfaces
- Mock implementations in tests demonstrate this principle

### Interface Segregation Principle (ISP)
- Focused interfaces (IOrderQueryService, IOrderCommandService, etc.)
- Clients only depend on methods they actually use
- No "fat" interfaces forcing unnecessary dependencies

### Dependency Inversion Principle (DIP)
- High-level services depend on abstractions, not concrete implementations
- All dependencies are injected through constructors
- Easy to test and maintain through dependency injection

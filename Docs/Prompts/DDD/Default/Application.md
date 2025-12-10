# ABP Framework v10 Application Layer Development Guide (Fully Integrated)

This guide provides production-ready code and patterns for developing the Application Layer within an ABP Framework v10 module. It focuses on the application service's role as an orchestrator for data access, domain logic, and other application concerns like validation and authorization.

## Key Architectural Principles

  * **Data Access Orchestration:** Application Services are the primary orchestrators of data access. They use repositories with specifications for querying, load all necessary data upfront, pass it to domain services for business logic execution, and finally persist the results.
  * **Domain Service Integration:** Application Services coordinate the interaction between the outside world and the domain. They do not contain business logic themselves but delegate it to domain services, passing already-loaded data as parameters.
  * **Separation of Concerns:** The Application Layer is responsible for Application Use Cases, Authorization, Validation, DTO Mapping, and Unit of Work management. It is kept thin by moving all core business rules into the Domain Layer.

## Technical Context

  * **Framework & Platform:** ABP Framework v10 (e.g., v10.0.0), .NET 10.0, Multi-tenancy enabled.
  * **Core NuGet Packages:**
      * **.Application.Contracts Project:**
        ```xml
        <ItemGroup>
          <PackageReference Include="Volo.Abp.Ddd.Application.Contracts" Version="10.0.0" />
          <PackageReference Include="Volo.Abp.Authorization" Version="10.0.0" />
          <PackageReference Include="Volo.Abp.Validation" Version="10.0.0" />
        </ItemGroup>
        ```
      * **.Application Project:**
        ```xml
        <ItemGroup>
          <PackageReference Include="Volo.Abp.Ddd.Application" Version="10.0.0" />
          <PackageReference Include="Volo.Abp.Mapperly" Version="10.0.0" /> <PackageReference Include="Volo.Abp.FluentValidation" Version="10.0.0" />
        </ItemGroup>
        ```

-----

## I. Application Contracts Layer (`.Application.Contracts`)

This layer defines the contract between the client and the server, containing DTOs, service interfaces, and permissions.

### 1\. Permissions Definition

Permissions are defined centrally and used by the `[Authorize]` attribute to secure application service methods.

```csharp
// src/NouAssist.Orders.Application.Contracts/Permissions/OrdersPermissions.cs
namespace NouAssist.Orders.Permissions
{
    public static class OrdersPermissions
    {
        public const string GroupName = "Orders";

        public static class Orders
        {
            public const string Default = GroupName + ".Orders";
            public const string Create = Default + ".Create";
            public const string UpdateStatus = Default + ".UpdateStatus";
            public const string Delete = Default + ".Delete";
            public const string Process = Default + ".Process";
        }
    }
}
```

### 2\. DTOs (Data Transfer Objects)

DTOs are used to transfer data. They are tailored to the needs of the UI or client applications.

```csharp
// src/NouAssist.Orders.Application.Contracts/Orders/Dtos/OrderDto.cs
using System;
using System.Collections.Generic;
using NouAssist.Orders.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Orders.Dtos
{
    public class OrderDto : FullAuditedEntityDto<Guid> { /* ... */ }
    public class OrderLineDto : EntityDto { /* ... */ }
    public class CreateOrderDto { /* ... */ }
    public class CreateOrderLineDto { /* ... */ }
    public class GetOrderListDto : PagedAndSortedResultRequestDto { /* ... */ }

    // DTO for the new IValidatableObject example
    public class UpdateOrderStatusDto
    {
        public OrderStatus NewStatus { get; set; }
        public string? CancellationReason { get; set; }
    }
}
```

### 3\. Application Service Interface

The interface defines the public methods of the application service.

```csharp
// src/NouAssist.Orders.Application.Contracts/Orders/IOrderAppService.cs
using System;
using System.Threading.Tasks;
using NouAssist.Orders.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Orders.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task<OrderDto> GetAsync(Guid id);
        Task<PagedResultDto<OrderDto>> GetListAsync(GetOrderListDto input);
        Task<OrderDto> CreateAsync(CreateOrderDto input);
        Task<OrderDto> ProcessOrderAsync(Guid id);
        Task<OrderDto> UpdateStatusAsync(Guid id, UpdateOrderStatusDto input);
    }
}
```

-----

## II. Application Layer (`.Application`)

This layer contains the implementation of the application services.

### 1\. Mapperly Configuration

In ABP v10, **Mapperly** is the default object mapper. It uses source generation to create mapping code at compile time, improving performance and type safety. Instead of a `Profile` class, you define a partial class with the `[Mapper]` attribute.

Note that ABP's `Volo.Abp.Mapperly` package integrates these mappers into the standard `IObjectMapper` system, so you can still use `ObjectMapper.Map` in your services if preferred, or inject this specific mapper class directly for maximum performance.

```csharp
// src/NouAssist.Orders.Application/Orders/OrdersApplicationMapper.cs
using Riok.Mapperly.Abstractions;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Dtos;
using NouAssist.Orders.Entities;
using Volo.Abp.Mapperly;

namespace NouAssist.Orders.Orders
{
    // 1. Inherit from MapperBase to hook into ABP's IObjectMapper system automatically
    [Mapper]
    public partial class OrdersApplicationMapper : MapperBase<Order, OrderDto>
    {
        // 2. Define specific mapping rules using partial methods
        // Mapperly automatically implements this based on property names
        public override partial OrderDto Map(Order source);

        // 3. Custom mapping logic (replacing AutoMapper's ForMember/MapFrom)
        // We use a custom mapping method for the Order -> OrderDto conversion to handle the TotalAmount
        [UserMapping(Default = true)]
        public OrderDto MapToDto(Order source)
        {
            var dto = Map(source); // Call the generated Map method first
            dto.TotalAmount = source.GetTotalAmount(); // Manually map the custom method
            return dto;
        }

        // 4. Define other mappings in the same file or separate partials
        public partial OrderLineDto Map(OrderLine source);
        
        public partial List<OrderDto> MapList(List<Order> source);
    }
}
```

### 2\. Input Validation

ABP integrates with standard validation mechanisms. Here are two common approaches.

#### a. Using FluentValidation

For complex validation scenarios or to keep validation logic separate from DTOs, FluentValidation is an excellent choice.

```csharp
// src/NouAssist.Orders.Application/Orders/CreateOrderDtoValidator.cs
using FluentValidation;
using NouAssist.Orders.Dtos;

namespace NouAssist.Orders
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.OrderLines).NotEmpty().WithMessage("Order must have at least one line item.");
            RuleForEach(x => x.OrderLines).SetValidator(new CreateOrderLineDtoValidator());
        }
    }
}
```

#### b. Using IValidatableObject

For simpler, self-contained cross-property validation, you can implement the `IValidatableObject` interface directly on the DTO.

```csharp
// src/NouAssist.Orders.Application.Contracts/Orders/Dtos/UpdateOrderStatusDto.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.Orders.Enums;

namespace NouAssist.Orders.Dtos
{
    public class UpdateOrderStatusDto : IValidatableObject
    {
        [Required]
        public OrderStatus NewStatus { get; set; }
        public string? CancellationReason { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewStatus == OrderStatus.Cancelled && string.IsNullOrWhiteSpace(CancellationReason))
            {
                yield return new ValidationResult(
                    "A cancellation reason is required when cancelling an order.",
                    new[] { nameof(CancellationReason) }
                );
            }
        }
    }
}
```

### 3\. Application Service Implementation

This is the core of the application layer. By inheriting `OrdersApplicationMapper` from `MapperBase`, the standard `ObjectMapper` is automatically wired up to use the generated Mapperly code.

```csharp
// src/NouAssist.Orders.Application/Orders/OrderAppService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Customers.Aggregates;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Dtos;
using NouAssist.Orders.Permissions;
using NouAssist.Orders.Services;
using NouAssist.Orders.Specifications;
using NouAssist.Products.Aggregates;
using NouAssist.Products.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Orders.Orders
{
    [Authorize(OrdersPermissions.Orders.Default)]
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IReadOnlyRepository<Product, Guid> _productRepository;
        private readonly IReadOnlyRepository<Customer, Guid> _customerRepository;
        
        private readonly OrderProcessingService _orderProcessingService;

        public OrderAppService(
            IRepository<Order, Guid> orderRepository,
            IReadOnlyRepository<Product, Guid> productRepository,
            IReadOnlyRepository<Customer, Guid> customerRepository,
            OrderProcessingService orderProcessingService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderProcessingService = orderProcessingService;
        }

        public async Task<OrderDto> GetAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            // Uses the Mapperly generated code via the abstraction
            return ObjectMapper.Map<Order, OrderDto>(order); 
        }

        public async Task<PagedResultDto<OrderDto>> GetListAsync(GetOrderListDto input)
        {
            var spec = CreateListSpecification(input);
            
            var orders = await _orderRepository.GetListAsync(spec);
            var totalCount = await _orderRepository.GetCountAsync(spec);

            return new PagedResultDto<OrderDto>(
                totalCount,
                ObjectMapper.Map<List<Order>, List<OrderDto>>(orders)
            );
        }

        [Authorize(OrdersPermissions.Orders.Create)]
        public async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            var productIds = input.OrderLines.Select(l => l.ProductId).Distinct().ToList();
            var products = await _productRepository.GetListAsync(new ProductsByIdsSpecification(productIds));
            
            // Create the aggregate root
            var order = new Order(GuidGenerator.Create(), input.CustomerId, CurrentTenant.Id);

            // Add order lines using domain logic
            foreach (var lineDto in input.OrderLines)
            {
                var product = products.FirstOrDefault(p => p.Id == lineDto.ProductId);
                if (product == null) throw new UserFriendlyException($"Product with ID {lineDto.ProductId} not found.");

                var variant = product.GetVariant(lineDto.VariantSku);
                order.AddOrderLine(product.Id, variant.Sku, variant.Price, lineDto.Quantity);
            }

            await _orderRepository.InsertAsync(order);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        [Authorize(OrdersPermissions.Orders.Process)]
        public async Task<OrderDto> ProcessOrderAsync(Guid id)
        {
            // 1. Load all required data
            var order = await _orderRepository.GetAsync(id, includeDetails: true);
            var customer = await _customerRepository.GetAsync(order.CustomerId);
            var productIds = order.OrderLines.Select(l => l.ProductId).ToList();
            var products = await _productRepository.GetListAsync(new ProductsByIdsSpecification(productIds));
            
            // Assume discount rules are loaded as needed; for simplicity, we pass an empty list.
            var availableDiscounts = new List<DiscountRule>();

            // 2. Execute business logic via the domain service
            var result = _orderProcessingService.ProcessOrder(
                order, products, customer, availableDiscounts, CurrentUser.Id.Value);
            
            if (!result.IsSuccessful)
            {
                throw new UserFriendlyException(string.Join(", ", result.Errors));
            }

            // 3. Persist the updated aggregate
            await _orderRepository.UpdateAsync(result.ProcessedOrder);

            // 4. Return the mapped DTO
            return ObjectMapper.Map<Order, OrderDto>(result.ProcessedOrder);
        }

        private ISpecification<Order> CreateListSpecification(GetOrderListDto input)
        {
            return Specification.And(
                input.CustomerId.HasValue,
                new OrdersByCustomerSpecification(input.CustomerId.Value)
            ).And(
                input.Status.HasValue,
                new Specification<Order>(o => o.Status == input.Status.Value)
            ).And(
                input.FromDate.HasValue,
                new Specification<Order>(o => o.OrderDate >= input.FromDate.Value)
            ).And(
                input.ToDate.HasValue,
                new Specification<Order>(o => o.OrderDate <= input.ToDate.Value)
            );
        }
    }
}
```

-----

## III. Testing Strategy

Robust testing is crucial. Integration tests validate the service from end-to-end with a real database, while unit tests isolate the service logic with mocks.

### Integration Tests (`.Application.Tests`)

Ensure your test module also depends on `AbpMapperlyModule` so the mappings are registered during tests.

```csharp
// src/NouAssist.Orders.Application.Tests/Orders/OrderAppService_Tests.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Customers.Aggregates;
using NouAssist.Orders.Aggregates;
using NouAssist.Orders.Dtos;
using NouAssist.Orders.Enums;
using NouAssist.Products.Aggregates;
using NouAssist.Products.Enums;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace NouAssist.Orders.Orders
{
    public class OrderAppService_Tests : OrdersApplicationTestBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public OrderAppService_Tests()
        {
            _orderAppService = GetRequiredService<IOrderAppService>();
            _orderRepository = GetRequiredService<IRepository<Order, Guid>>();
            _productRepository = GetRequiredService<IRepository<Product, Guid>>();
            _customerRepository = GetRequiredService<IRepository<Customer, Guid>>();
        }

        [Fact]
        public async Task CreateAsync_Should_Create_A_Valid_Order()
        {
            // Arrange
            var customer = await CreateTestCustomerAsync();
            var product = await CreateTestProductAsync();
            var input = new CreateOrderDto
            {
                CustomerId = customer.Id,
                OrderLines = new List<CreateOrderLineDto>
                {
                    new() { ProductId = product.Id, VariantSku = "LP-16-512", Quantity = 1 }
                }
            };

            // Act
            var orderDto = await _orderAppService.CreateAsync(input);

            // Assert
            orderDto.Id.ShouldNotBe(Guid.Empty);
            orderDto.OrderLines.Count.ShouldBe(1);
            
            var orderInDb = await _orderRepository.FindAsync(orderDto.Id);
            orderInDb.ShouldNotBeNull();
            orderInDb.GetTotalAmount().ShouldBe(1500m);
        }

        [Fact]
        public async Task ProcessOrderAsync_Should_Update_Order_Status()
        {
            // Arrange
            var customer = await CreateTestCustomerAsync();
            var product = await CreateTestProductAsync();
            var order = await CreateTestOrderAsync(customer.Id, product.Id);
            order.ConfirmOrder(); // Prerequisite state
            await _orderRepository.UpdateAsync(order);

            // Act
            var result = await _orderAppService.ProcessOrderAsync(order.Id);

            // Assert
            result.Status.ShouldBe(OrderStatus.Processing);
            
            var updatedOrder = await _orderRepository.GetAsync(order.Id);
            updatedOrder.Status.ShouldBe(OrderStatus.Processing);
        }

        #region Test Data Helpers
        private async Task<Customer> CreateTestCustomerAsync()
        {
            return await WithUnitOfWorkAsync(async () =>
                await _customerRepository.InsertAsync(new Customer(GuidGenerator.Create(), "Test", "Customer", "test@cust.com")));
        }
        
        private async Task<Product> CreateTestProductAsync()
        {
            return await WithUnitOfWorkAsync(async () =>
            {
                var product = new Product(GuidGenerator.Create(), "Laptop Pro", ProductStatus.Published, null);
                product.AddVariant(new(product.Id, "LP-16-512", 1500m, 10));
                return await _productRepository.InsertAsync(product);
            });
        }

        private async Task<Order> CreateTestOrderAsync(Guid customerId, Guid productId)
        {
            return await WithUnitOfWorkAsync(async () =>
            {
                var order = new Order(GuidGenerator.Create(), customerId, null);
                order.AddOrderLine(productId, "LP-16-512", 1500m, 1);
                return await _orderRepository.InsertAsync(order);
            });
        }
        #endregion
    }
}
```
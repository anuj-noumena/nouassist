using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.Workflows;
using NouAssist.WorkManagement.Workflow.Workflows.Dtos;
using Shouldly;
using Volo.Abp.Validation;
using Xunit;

namespace NouAssist.WorkManagement.Workflow
{
    public class WorkflowAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IWorkflowAppService _workflowAppService;

        public WorkflowAppServiceTests()
        {
            _workflowAppService = GetRequiredService<IWorkflowAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateWorkflow()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateWorkflowDto
                {
                    Name = "Test Workflow",
                    Description = "Test Description",
                    InitialStatusId = Guid.NewGuid()
                };

                // Act
                var result = await _workflowAppService.CreateAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Id.ShouldNotBe(Guid.Empty);
                result.Name.ShouldBe(input.Name);
                result.Description.ShouldBe(input.Description);
            });
        }

        [Fact]
        public async Task CreateAsync_WithEmptyName_Should_ThrowValidationException()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateWorkflowDto
                {
                    Name = "", // Empty name
                    InitialStatusId = Guid.NewGuid()
                };

                // Act & Assert
                await Should.ThrowAsync<AbpValidationException>(async () =>
                    await _workflowAppService.CreateAsync(input));
            });
        }

        [Fact]
        public async Task GetAsync_WithValidId_Should_ReturnWorkflow()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateWorkflowDto
                {
                    Name = "Test Workflow",
                    InitialStatusId = Guid.NewGuid()
                };

                var createdWorkflow = await _workflowAppService.CreateAsync(createInput);
                var retrievedWorkflow = await _workflowAppService.GetAsync(createdWorkflow.Id);

                // Assert
                retrievedWorkflow.ShouldNotBeNull();
                retrievedWorkflow.Id.ShouldBe(createdWorkflow.Id);
                retrievedWorkflow.Name.ShouldBe(createdWorkflow.Name);
            });
        }

        [Fact]
        public async Task UpdateAsync_WithValidInput_Should_UpdateWorkflow()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateWorkflowDto
                {
                    Name = "Original Name",
                    InitialStatusId = Guid.NewGuid()
                };

                var createdWorkflow = await _workflowAppService.CreateAsync(createInput);

                var updateInput = new UpdateWorkflowDto
                {
                    Name = "Updated Name",
                    Description = "Updated Description"
                };

                var updatedWorkflow = await _workflowAppService.UpdateAsync(createdWorkflow.Id, updateInput);

                // Assert
                updatedWorkflow.ShouldNotBeNull();
                updatedWorkflow.Name.ShouldBe("Updated Name");
                updatedWorkflow.Description.ShouldBe("Updated Description");
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteWorkflow()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateWorkflowDto
                {
                    Name = "Workflow to Delete",
                    InitialStatusId = Guid.NewGuid()
                };

                var createdWorkflow = await _workflowAppService.CreateAsync(createInput);
                await _workflowAppService.DeleteAsync(createdWorkflow.Id);

                // Assert
                await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(async () =>
                    await _workflowAppService.GetAsync(createdWorkflow.Id));
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnPagedResults()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                // Create test workflows
                var initialStatusId = Guid.NewGuid();
                await _workflowAppService.CreateAsync(new CreateWorkflowDto
                {
                    Name = "Workflow 1",
                    InitialStatusId = initialStatusId
                });
                await _workflowAppService.CreateAsync(new CreateWorkflowDto
                {
                    Name = "Workflow 2",
                    InitialStatusId = initialStatusId
                });

                var input = new GetWorkflowListDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0
                };

                var result = await _workflowAppService.GetListAsync(input);

                // Assert
                result.ShouldNotBeNull();
                result.Items.ShouldNotBeEmpty();
                result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            });
        }
    }
}

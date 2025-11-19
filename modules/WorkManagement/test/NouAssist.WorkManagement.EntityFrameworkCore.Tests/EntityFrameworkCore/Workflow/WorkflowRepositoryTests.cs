using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Data;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Workflow
{
    public class WorkflowRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly IWorkflowRepository _workflowRepository;

        public WorkflowRepositoryTests()
        {
            _workflowRepository = GetRequiredService<IWorkflowRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertWorkflow()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                var workflow = new Aggregates.Workflow(
                    Guid.NewGuid(),
                    "Test Workflow",
                    Guid.NewGuid());

                // Act
                var insertedWorkflow = await _workflowRepository.InsertAsync(workflow);

                // Assert
                insertedWorkflow.ShouldNotBeNull();
                insertedWorkflow.Id.ShouldBe(workflow.Id);
                insertedWorkflow.Name.ShouldBe("Test Workflow");
            });
        }

        [Fact]
        public async Task GetAsync_Should_ReturnWorkflow()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var workflow = new Aggregates.Workflow(
                    Guid.NewGuid(),
                    "Test Workflow",
                    Guid.NewGuid());

                await _workflowRepository.InsertAsync(workflow);

                var retrievedWorkflow = await _workflowRepository.GetAsync(workflow.Id);

                // Assert
                retrievedWorkflow.ShouldNotBeNull();
                retrievedWorkflow.Id.ShouldBe(workflow.Id);
                retrievedWorkflow.Name.ShouldBe("Test Workflow");
            });
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateWorkflow()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var workflow = new Aggregates.Workflow(
                    Guid.NewGuid(),
                    "Original Name",
                    Guid.NewGuid());

                await _workflowRepository.InsertAsync(workflow);

                workflow.UpdateName("Updated Name");
                await _workflowRepository.UpdateAsync(workflow);

                var updatedWorkflow = await _workflowRepository.GetAsync(workflow.Id);

                // Assert
                updatedWorkflow.Name.ShouldBe("Updated Name");
            });
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteWorkflow()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var workflow = new Aggregates.Workflow(
                    Guid.NewGuid(),
                    "Workflow to Delete",
                    Guid.NewGuid());

                await _workflowRepository.InsertAsync(workflow);
                await _workflowRepository.DeleteAsync(workflow);

                // Assert
                var deletedWorkflow = await _workflowRepository.FindAsync(workflow.Id);
                deletedWorkflow.ShouldBeNull();
            });
        }

        [Fact]
        public async Task GetListAsync_Should_ReturnWorkflows()
        {
            // Arrange & Act
            await WithUnitOfWorkAsync(async () =>
            {
                var workflow1 = new Aggregates.Workflow(
                    Guid.NewGuid(),
                    "Workflow 1",
                    Guid.NewGuid());

                var workflow2 = new Aggregates.Workflow(
                    Guid.NewGuid(),
                    "Workflow 2",
                    Guid.NewGuid());

                await _workflowRepository.InsertAsync(workflow1);
                await _workflowRepository.InsertAsync(workflow2);

                var workflows = await _workflowRepository.GetListAsync();

                // Assert
                workflows.Count.ShouldBeGreaterThanOrEqualTo(2);
            });
        }
    }
}

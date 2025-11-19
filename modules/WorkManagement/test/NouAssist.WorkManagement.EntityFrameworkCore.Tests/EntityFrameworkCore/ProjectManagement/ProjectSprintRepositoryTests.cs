using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Data;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement
{
    public class ProjectSprintRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly IProjectSprintRepository _sprintRepository;

        public ProjectSprintRepositoryTests()
        {
            _sprintRepository = GetRequiredService<IProjectSprintRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertProjectSprint()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var sprint = new ProjectSprint(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Sprint 1",
                    "Goal",
                    DateTime.Now,
                    DateTime.Now.AddDays(14),
                    null);

                var inserted = await _sprintRepository.InsertAsync(sprint);
                inserted.ShouldNotBeNull();
                inserted.Name.ShouldBe("Sprint 1");
            });
        }

        [Fact]
        public async Task GetAsync_Should_ReturnProjectSprint()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var sprint = new ProjectSprint(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Sprint 1",
                    "Goal",
                    DateTime.Now,
                    DateTime.Now.AddDays(14),
                    null);

                await _sprintRepository.InsertAsync(sprint);
                var retrieved = await _sprintRepository.GetAsync(sprint.Id);
                
                retrieved.ShouldNotBeNull();
                retrieved.Name.ShouldBe("Sprint 1");
            });
        }
    }
}

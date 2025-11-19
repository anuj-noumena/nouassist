using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Requirements;
using NouAssist.WorkManagement.ProjectManagement.Requirements.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.ProjectManagement
{
    public class ProjectRequirementAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IProjectRequirementAppService _projectRequirementAppService;

        public ProjectRequirementAppServiceTests()
        {
            _projectRequirementAppService = GetRequiredService<IProjectRequirementAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateProjectRequirement()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateProjectRequirementDto
                {
                    ProjectId = Guid.NewGuid(),
                    Title = "User Authentication"
                };

                var result = await _projectRequirementAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Title.ShouldBe("User Authentication");
            });
        }
    }
}

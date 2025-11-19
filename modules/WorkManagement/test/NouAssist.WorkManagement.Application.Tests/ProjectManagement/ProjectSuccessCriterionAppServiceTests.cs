using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.SuccessCriteria;
using NouAssist.WorkManagement.ProjectManagement.SuccessCriteria.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.ProjectManagement
{
    public class ProjectSuccessCriterionAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IProjectSuccessCriterionAppService _projectSuccessCriterionAppService;

        public ProjectSuccessCriterionAppServiceTests()
        {
            _projectSuccessCriterionAppService = GetRequiredService<IProjectSuccessCriterionAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateSuccessCriterion()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateProjectSuccessCriterionDto
                {
                    ProjectId = Guid.NewGuid(),
                    Description = "90% user satisfaction"
                };

                var result = await _projectSuccessCriterionAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Description.ShouldBe("90% user satisfaction");
            });
        }
    }
}

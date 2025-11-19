using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Reviews.Templates;
using NouAssist.WorkManagement.Reviews.Templates.Dtos;
using NouAssist.WorkManagement.Reviews.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Reviews
{
    public class ReviewTemplateAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly IReviewTemplateAppService _reviewTemplateAppService;

        public ReviewTemplateAppServiceTests()
        {
            _reviewTemplateAppService = GetRequiredService<IReviewTemplateAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateReviewTemplate()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateReviewTemplateDto
                {
                    Name = "Weekly Review Template",
                    ReviewType = ReviewType.Weekly
                };

                var result = await _reviewTemplateAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Name.ShouldBe("Weekly Review Template");
            });
        }
    }
}

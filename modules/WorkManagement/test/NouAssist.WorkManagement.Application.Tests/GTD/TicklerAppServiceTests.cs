using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.GTD;
using NouAssist.WorkManagement.GTD.GTD.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.GTD
{
    public class TicklerAppServiceTests : WorkManagementApplicationTestBase<WorkManagementApplicationTestModule>
    {
        private readonly ITicklerAppService _ticklerAppService;

        public TicklerAppServiceTests()
        {
            _ticklerAppService = GetRequiredService<ITicklerAppService>();
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_Should_CreateTicklerItem()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateTicklerItemDto
                {
                    Title = "Review insurance policy",
                    TickleDate = DateTime.Now.AddMonths(6)
                };

                var result = await _ticklerAppService.CreateAsync(input);

                result.ShouldNotBeNull();
                result.Title.ShouldBe("Review insurance policy");
            });
        }

        [Fact]
        public async Task SurfaceAsync_Should_SurfaceTicklerItem()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var createInput = new CreateTicklerItemDto
                {
                    Title = "Review insurance policy",
                    TickleDate = DateTime.Now
                };

                var created = await _ticklerAppService.CreateAsync(createInput);
                var result = await _ticklerAppService.SurfaceAsync(created.Id);

                result.Status.ShouldBe(Enums.TicklerStatus.Surfaced);
            });
        }
    }
}

using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD
{
    public class TicklerItemRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly ITicklerRepository _ticklerRepository;

        public TicklerItemRepositoryTests()
        {
            _ticklerRepository = GetRequiredService<ITicklerRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertTicklerItem()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var item = new TicklerItem(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Review insurance",
                    DateTime.Now.AddMonths(6));

                var inserted = await _ticklerRepository.InsertAsync(item);
                inserted.ShouldNotBeNull();
                inserted.Title.ShouldBe("Review insurance");
            });
        }
    }
}

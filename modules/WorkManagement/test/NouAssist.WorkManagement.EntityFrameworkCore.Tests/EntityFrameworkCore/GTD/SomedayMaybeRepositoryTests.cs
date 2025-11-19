using System;
using System.Threading.Tasks;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD
{
    public class SomedayMaybeRepositoryTests : WorkManagementEntityFrameworkCoreTestBase
    {
        private readonly ISomedayMaybeRepository _somedayMaybeRepository;

        public SomedayMaybeRepositoryTests()
        {
            _somedayMaybeRepository = GetRequiredService<ISomedayMaybeRepository>();
        }

        [Fact]
        public async Task InsertAsync_Should_InsertSomedayMaybeItem()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var item = new SomedayMaybeItem(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Learn Piano");

                var inserted = await _somedayMaybeRepository.InsertAsync(item);
                inserted.ShouldNotBeNull();
                inserted.Title.ShouldBe("Learn Piano");
            });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore
{
    public static class ContactsDbContextModelCreatingExtensions
    {
        public static void ConfigureContacts(
            this ModelBuilder builder,
            ContactsDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(ContactsDbContext).Assembly);
        }
    }
}

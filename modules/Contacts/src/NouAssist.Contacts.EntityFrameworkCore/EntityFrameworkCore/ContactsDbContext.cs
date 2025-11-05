using Microsoft.EntityFrameworkCore;
using NouAssist.Contacts.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Contacts.EntityFrameworkCore
{
    [ConnectionStringName(ContactsDbProperties.ConnectionStringName)]
    public class ContactsDbContext : AbpDbContext<ContactsDbContext>, IContactsDbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Client> Clients { get; set; }

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureContacts(this);
        }
    }
}

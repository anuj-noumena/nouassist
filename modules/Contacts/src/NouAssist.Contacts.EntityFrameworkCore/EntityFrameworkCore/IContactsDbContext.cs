using Microsoft.EntityFrameworkCore;
using NouAssist.Contacts.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Contacts.EntityFrameworkCore
{
    [ConnectionStringName(ContactsDbProperties.ConnectionStringName)]
    public interface IContactsDbContext : IEfCoreDbContext
    {
        DbSet<Contact> Contacts { get; }
        DbSet<Client> Clients { get; }
    }
}

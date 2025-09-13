using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace updateddetyraep.Models
{
    public class ContactsBookDBContext : IdentityDbContext<IdentityUser>
    {
        public ContactsBookDBContext(DbContextOptions<ContactsBookDBContext> options)
            : base(options)
        {
        }


        // DbSet për Contacts
        public DbSet<Contact> Contacts { get; set; }
    }
}

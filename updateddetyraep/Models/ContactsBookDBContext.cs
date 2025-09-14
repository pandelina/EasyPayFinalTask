using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using updateddetyraep.Models;

public class ContactsBookDBContext : IdentityDbContext<IdentityUser>
{
    public ContactsBookDBContext(DbContextOptions<ContactsBookDBContext> options)
        : base(options)
    {
    }

    // Tabelat e databazës
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // e domosdoshme për Identity

        // Marrëdhënia Contact -> PhoneNumbers (1 kontakt ka shumë numra)
        modelBuilder.Entity<PhoneNumber>()
            .HasOne(p => p.Contact)       // PhoneNumber ka një Contact
            .WithMany(c => c.PhoneNumbers) // Contact ka shumë PhoneNumbers
            .HasForeignKey(p => p.ContactId) // Foreign key
            .OnDelete(DeleteBehavior.Cascade); // fshij numrat kur fshihet kontakti
    }
}

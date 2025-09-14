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

    // Tabelat e databazes
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.Entity<PhoneNumber>()
            .HasOne(p => p.Contact)       
            .WithMany(c => c.PhoneNumbers) 
            .HasForeignKey(p => p.ContactId) 
            .OnDelete(DeleteBehavior.Cascade); 
    }
}

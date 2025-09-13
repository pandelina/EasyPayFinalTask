using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using updateddetyraep.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace updateddetyraep
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // DbContext
            builder.Services.AddDbContext<ContactsBookDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsBookDBContextConnection")));

            // Identity (login + register, pa role)
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ContactsBookDBContext>()
            .AddDefaultTokenProviders();

            // Dummy email sender (nëse duhet për konfirmim emaili, edhe pse e ke off)
            builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }

    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // No real email sent
            return Task.CompletedTask;
        }
    }
}

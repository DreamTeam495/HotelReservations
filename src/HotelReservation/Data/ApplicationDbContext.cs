using HotelReservation.Areas.Identity.Models;
using HotelReservation.Areas.Rooms.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable(name: "User");
        });
        
        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Role");
        });
        
        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaim");
        });
        
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaim");
        });
        
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogin");
        });
        
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRole");
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserToken");
        });
        
        builder.Entity<Room>(entity =>
        {
            entity.ToTable("Room");
        });
        
        builder.Entity<Reservation>(entity =>
        {
            entity.ToTable("Reservation");
        });
        
        builder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");
        });
    }
}
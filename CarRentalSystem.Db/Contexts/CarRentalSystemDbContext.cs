using CarRentalSystem.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Db.Contexts;

public class CarRentalSystemDbContext (DbContextOptions<CarRentalSystemDbContext> options) : DbContext(options)
{
    DbSet<Car> Cars { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId);
        
        modelBuilder.Entity<Reservation>()
            .HasOne<Car>()
            .WithMany()
            .HasForeignKey(r => r.CarId);
    }
}

using CarRentalSystem.Core.Identity;
using CarRentalSystem.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Core.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Seed a Car
            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Id = 1,
                    Brand = "Audi",
                    Model = "A3",
                    Year = 2020,
                    DailyRate = 10,
                    RegistrationNo = "ABC123",
                    Color = "Red",
                    Image = "car1.jpg"
                }
            );

            // ✅ Seed a Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    CustomerName = "John Doe",
                    CustomerCity = "New York",
                    MobileNumber = "1234567890",
                    CustomerEmail = "john@example.com"
                }
            );

            // ✅ Seed a Booking (linked via CarId & CustomerId)
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    CarId = 1,
                    CustomerId = 1,
                    StartDate = new DateTime(2025, 1, 1),
                    EndDate = new DateTime(2025, 1, 5),
                    TotalAmount = 50
                }
            );
        }
    }
}

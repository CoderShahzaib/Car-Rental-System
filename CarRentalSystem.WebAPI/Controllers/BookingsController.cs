
using CarRentalSystem.Core.DatabaseContext;
using CarRentalSystem.Core.DTOs;
using CarRentalSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.WebAPI.Controllers
{
    public class BookingsController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _context.Bookings.AsNoTracking().Include(b => b.Car).Include(b => b.Customer).Select(b => new BookingDTO{
                    Id = b.Id,
                    CarModel = b.Car != null ? b.Car.Model : string.Empty,
                    CustomerName = b.Customer != null ? b.Customer.CustomerName : string.Empty,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    TotalAmount = b.TotalAmount,
            }).ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _context.Bookings.AsNoTracking().Include(b => b.Car).Include(b => b.Customer).Select(b => new BookingDTO
            {
                Id = b.Id,
                CarModel = b.Car != null ? b.Car.Model : string.Empty,
                CustomerName = b.Customer != null ? b.Customer.CustomerName : string.Empty,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                TotalAmount = b.TotalAmount,
            }).FirstOrDefaultAsync();

            if(booking == null)
            {
                return NotFound(new { message = $"Booking with ID {id} not found." });
            }
            return Ok(booking);
        }
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] CreateBookingDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = new Booking()
            {
                CarId = dto.CarId,
                CustomerId = dto.CustomerId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalAmount = dto.TotalAmount,
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id}, booking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if(booking == null)
            {
                return NotFound(new { message = $"Booking with ID {id} not found." });
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

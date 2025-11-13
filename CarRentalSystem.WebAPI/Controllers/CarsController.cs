
using CarRentalSystem.Core.DatabaseContext;
using CarRentalSystem.Core.DTOs;
using CarRentalSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _context.Cars
                .AsNoTracking()
                .Select(c => new CarDTO
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    DailyRate = c.DailyRate,
                    Color = c.Color,
                    Image = c.Image
                })
                .ToListAsync();

            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _context.Cars
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CarDTO
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    DailyRate = c.DailyRate,
                    Color = c.Color,
                    Image = c.Image
                })
                .FirstOrDefaultAsync();

            if (car == null)
                return NotFound(new { message = $"Car with ID {id} not found." });

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarDTO carDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var car = new Car
            {
                Brand = carDTO.Brand,
                Model = carDTO.Model,
                Year = carDTO.Year,
                Color = carDTO.Color,
                DailyRate = carDTO.DailyRate,
                RegistrationNo = carDTO.RegistrationNo,
                Image = carDTO.Image
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            var result = new CarDTO
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                DailyRate = car.DailyRate,
                Color = car.Color,
                Image = car.Image
            };

            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CreateCarDTO carDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCar = await _context.Cars.FindAsync(id);
            if (existingCar == null)
                return NotFound(new { message = $"Car with ID {id} not found." });

            // Update properties
            existingCar.Brand = carDTO.Brand;
            existingCar.Model = carDTO.Model;
            existingCar.Year = carDTO.Year;
            existingCar.Color = carDTO.Color;
            existingCar.DailyRate = carDTO.DailyRate;
            existingCar.RegistrationNo = carDTO.RegistrationNo;
            existingCar.Image = carDTO.Image;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound(new { message = $"Car with ID {id} not found." });

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

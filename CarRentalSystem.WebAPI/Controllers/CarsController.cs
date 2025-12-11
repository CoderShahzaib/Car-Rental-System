using CarRentalSystem.Core.DatabaseContext;
using CarRentalSystem.Core.DTOs;
using CarRentalSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.WebAPI.Controllers
{
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
                .Select(c => new
                {
                    carId = c.Id,
                    brand = c.Brand,
                    model = c.Model,
                    year = c.Year,
                    color = c.Color,
                    dailyRate = c.DailyRate,
                    carImage = c.Image,
                    registrationNo = c.RegistrationNo
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Message = "",
                Result = true,
                Data = cars
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _context.Cars
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    carId = c.Id,
                    brand = c.Brand,
                    model = c.Model,
                    year = c.Year,
                    color = c.Color,
                    dailyRate = c.DailyRate,
                    carImage = c.Image,
                    registrationNo = c.RegistrationNo
                })
                .FirstOrDefaultAsync();

            if (car == null)
                return NotFound(new ApiResponse<object>
                {
                    Message = $"Car with ID {id} not found.",
                    Result = false,
                    Data = null
                });

            return Ok(new ApiResponse<object>
            {
                Message = "",
                Result = true,
                Data = car
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarDTO carDTO)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .FirstOrDefault();

                return BadRequest(new ApiResponse<object>
                {
                    Message = firstError ?? "Validation failed",
                    Result = false,
                    Data = null
                });
            }


            var car = new Car
            {
                Brand = carDTO.Brand,
                Model = carDTO.Model,
                Year = carDTO.Year,
                Color = carDTO.Color,
                DailyRate = carDTO.DailyRate,
                RegistrationNo = carDTO.RegistrationNo,
                Image = carDTO.carImage
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            var data = new
            {
                carId = car.Id,
                brand = car.Brand,
                model = car.Model,
                year = car.Year,
                color = car.Color,
                dailyRate = car.DailyRate,
                carImage = car.Image,
                registrationNo = car.RegistrationNo
            };

            return Ok(new ApiResponse<object>
            {
                Message = "Car created successfully",
                Result = true,
                Data = data
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CreateCarDTO carDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    Message = "Invalid data",
                    Result = false,
                    Data = ModelState
                });

            var existingCar = await _context.Cars.FindAsync(id);
            if (existingCar == null)
                return NotFound(new ApiResponse<object>
                {
                    Message = $"Car with ID {id} not found.",
                    Result = false,
                    Data = null
                });

            existingCar.Brand = carDTO.Brand;
            existingCar.Model = carDTO.Model;
            existingCar.Year = carDTO.Year;
            existingCar.Color = carDTO.Color;
            existingCar.DailyRate = carDTO.DailyRate;
            existingCar.RegistrationNo = carDTO.RegistrationNo;
            existingCar.Image = carDTO.carImage;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Message = "Car updated successfully",
                Result = true,
                Data = existingCar
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound(new ApiResponse<object>
                {
                    Message = $"Car with ID {id} not found.",
                    Result = false,
                    Data = null
                });

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Message = "Car deleted successfully",
                Result = true,
                Data = new { deletedCarId = id }
            });
        }
    }
}

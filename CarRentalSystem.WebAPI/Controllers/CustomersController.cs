
using CarRentalSystem.Core.DatabaseContext;
using CarRentalSystem.Core.DTOs;
using CarRentalSystem.Core.Models;
using CarRentalSystem.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.WebAPI.Controllers
{
    public class CustomersController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _context.Customers
                .AsNoTracking()
                .ToListAsync();
            return Ok(new ApiResponse<object>
            {
                Message = "",
                Result = true,
                Data = customers
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDTO customerDTO)
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
                    Result = true,
                    Data = null
                });
            }
            var customer = new Customer()
            {
                CustomerName = customerDTO.CustomerName,
                CustomerCity = customerDTO.CustomerCity,
                MobileNumber = customerDTO.MobileNumber,
                CustomerEmail = customerDTO.CustomerEmail,
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<object>
            {
                Message = "Customer created successfully",
                Result = true,
                Data = customer
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CreateCustomerDTO customerDTO) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Message = $"Customer with ID {id} not found.",
                    Result = false,
                    Data = null
                });
            }
            customer.CustomerName = customerDTO.CustomerName;
            customer.CustomerCity = customerDTO.CustomerCity;
            customer.CustomerEmail = customerDTO.CustomerEmail;
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<object>
            {
                Message = "Customer updated successfully",
                Result = true,
                Data = customer
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Message = $"Customer with ID {id} not found.",
                    Result = false,
                    Data = null
                });
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<object>
            {
                Message = "Customer deleted successfully",
                Result = true,
                Data = null
            });
        }
    }
}

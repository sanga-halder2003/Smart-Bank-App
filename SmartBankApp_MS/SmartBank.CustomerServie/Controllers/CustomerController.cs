using Microsoft.AspNetCore.Mvc;
using SmartBank.CustomerServie.Models;
using SmartBank.CustomerServie.Services;

namespace SmartBank.CustomerServie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _service.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _service.GetCustomerByIdAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            var createdCustomer = await _service.CreateCustomerAsync(customer);

            return CreatedAtAction(
                nameof(GetCustomerById),
                new { id = createdCustomer.Id },
                createdCustomer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
                return BadRequest();

            await _service.UpdateCustomerAsync(customer);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _service.DeleteCustomerAsync(id);

            return NoContent();
        }
    }
}
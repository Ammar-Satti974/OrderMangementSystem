using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProject.Models;
using WebApiProject.ServiceLayer;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // Get All Customers

        [HttpGet]
        [Authorize(Policy = "UserAndManagerPolicy")]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // Get Customer By Id
        [HttpGet("{id}")]
        [Authorize(Policy = "UserAndManagerPolicy")]
        public async Task<IActionResult> GetById(int id)
        {
            var customers = await _customerService.GetCustomerByIdAsync(id);
            if (customers == null) { return NotFound(); }
            return Ok(customers);
        }

        // add new customer

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("customer name Cant be Null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _customerService.AddCustomerAsync(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        // Update Customer
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }
            try
            {
                await _customerService.UpdateCustomerAsync(existingCustomer);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists(id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        private async Task<bool> CustomerExists(int id)
        {
            return await _customerService.GetCustomerByIdAsync(id) != null;
        }

        // Delete Customer
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _customerService.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}

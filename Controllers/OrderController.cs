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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Get All Orders

        [HttpGet]
        [Authorize(Policy = "UserAndManagerPolicy")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // Get Order By Id
        [HttpGet("{id}")]
        [Authorize(Policy = "UserAndManagerPolicy")]
        public async Task<IActionResult> GetById(int id)
        {
            var orders = await _orderService.GetOrderByIdAsync(id);
            if (orders == null) { return NotFound(); }
            return Ok(orders);
        }

        // add new Order

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("order name Cant be Null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _orderService.AddOrderAsync(order);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        // Update Order
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("order ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingOrder = await _orderService.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }
            try
            {
                await _orderService.UpdateOrderAsync(existingOrder);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderExists(id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        private async Task<bool> OrderExists(int id)
        {
            return await _orderService.GetOrderByIdAsync(id) != null;
        }

        // Delete Order
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _orderService.GetOrderByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProject.DAL;
using WebApiProject.Models;
using WebApiProject.ServiceLayer;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        // Get All ordersDetails

        [HttpGet]
        [Authorize(Policy = "UserAndManagerPolicy")]
        public async Task<IActionResult> GetAll()
        {
            var ordersDetails = await _orderDetailService.GetAllOrderDetailsAsync();
            return Ok(ordersDetails);
        }

        // Get ordersDetails By Id
        [HttpGet("{id}")]
        [Authorize(Policy = "UserAndManagerPolicy")]
        public async Task<IActionResult> GetById(int id)
        {
            var ordersDetails = await _orderDetailService.GetOrderDetailsByIdAsync(id);
            if (ordersDetails == null) { return NotFound(); }
            return Ok(ordersDetails);
        }

        // add new ordersDetails

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] OrderDetail orderDetail)
        {
            if (orderDetail == null)
            {
                return BadRequest("order name Cant be Null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _orderDetailService.AddOrderDetailsAsync(orderDetail);

            return CreatedAtAction(nameof(GetById), new { id = orderDetail.Id }, orderDetail);
        }

        // Update ordersDetails
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDetail orderDetail)
        {
            if (orderDetail == null)
            {
                return BadRequest("order ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingOrderDetails = await _orderDetailService.GetOrderDetailsByIdAsync(id);
            if (existingOrderDetails == null)
            {
                return NotFound();
            }
            try
            {
                await _orderDetailService.UpdateOrderDetailsAsync(existingOrderDetails);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderDetailExists(id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        private async Task<bool> OrderDetailExists(int id)
        {
            return await _orderDetailService.GetOrderDetailsByIdAsync(id) != null;
        }

        // Delete ordersDetails
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailsByIdAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            await _orderDetailService.DeleteOrderDetailsAsync(id);
            return NoContent();
        }
    }
}

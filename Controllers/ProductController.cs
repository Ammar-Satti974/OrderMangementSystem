using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.Models;
using WebApiProject.DAL;
using WebApiProject.ServiceLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Get All Products
        [HttpGet]
        [Authorize(Policy = "UserAndManagerPolicy")]
        [ResponseCache(Duration =60)]
        public async Task<IActionResult> GetAll()
        {
            return await ExecuteAsync(async () =>
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            });
        }

        // Get Product By Id
        [HttpGet("{id}")]
        [Authorize(Policy = "UserAndManagerPolicy")]
        public async Task<IActionResult> GetById(int id)
        {
            return await ExecuteAsync(async () =>
            {
                var products = await _productService.GetProductByIdAsync(id);
                if (products == null) { return NotFound(); }
                return Ok(products);
            });
        }

        // Create new Product
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            return await ExecuteAsync(async () =>
            {
                if (product == null)
                {
                    return BadRequest("Product Cant be Null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _productService.AddProductAsync(product);

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            });
        }

        // Update Product
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if(product == null)
            {
                return BadRequest("Product ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            try
            {
                await _productService.UpdateProductAsync(product);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // Product not found
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        private async Task<bool> ProductExists(int id)
        {
            return await _productService.GetProductByIdAsync(id) != null;
        }

        // Delete Product
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            return await ExecuteAsync(async () =>
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                await _productService.DeleteProductAsync(id);
                return NoContent();
            });
        }
    }
}

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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

      //  Get All Categories

        [HttpGet]
        [Authorize(Policy = "UserAndManagerPolicy")]
        public async Task<IActionResult> GetAll()
        {
            var category = await _categoryService.GetAllCategoriesAsync();
            return Ok(category);
        }

       // Get Categories By Id
        [HttpGet("{id}")]
        [Authorize(Policy = "UserAndManagerPolicy")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) { return NotFound(); }
            return Ok(category);
        }

      //  add new Categories

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("category name Cant be Null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _categoryService.AddCategoryAsync(category);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

       // Update Categories
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("category ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            try
            {
                await _categoryService.UpdateCategoryAsync(existingCategory);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryExists(id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        private async Task<bool> CategoryExists(int id)
        {
            return await _categoryService.GetCategoryByIdAsync(id) != null;
        }

      //  Delete Categories
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}

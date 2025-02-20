using ElectroDepotClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.ExtensionMethods;
using Server.Models;

namespace Server.Controllers
{
    [Route("ElectroDepot/[controller]")]
    [ApiController]
    public class CategoriesController : CustomControllerBase
    {
        public CategoriesController(DatabaseContext context) : base(context)
        {
        }

        #region Create
        /// <summary>
        /// POST: ElectroDepot/Categories/Create
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<CreateCategoryDTO>> CreateCategory(CreateCategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = categoryDTO.ToCategory();

            Category? existingCategory = await _context.Categories.FirstOrDefaultAsync(u => u.Name == category.Name);

            if (existingCategory != null)
            {
                return Conflict(new { title = "Conflict", status = 409, message = $"Category with name:'{category.Name}' already exists" });
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryID }, category.ToDTO());
        }
        #endregion
        #region Read
        /// <summary>
        /// GET: ElectroDepot/Categories/GetAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            IEnumerable<CategoryDTO> categories = await _context.Categories.Select(x=>x.ToDTO()).ToListAsync();
            return Ok(categories);
        }

        /// <summary>
        /// GET: ElectroDepot/Categories/GetCategoryByID/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCategoryByID/{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new { title = "Not Found", code = "404", message = $"Category with ID:{id} doesn't exsit" });
            }

            return Ok(category.ToDTO());
        }

        /// <summary>
        /// GET: ElectroDepot/Categories/GetCategoryByName/{name}
        /// Retrieves a user by their name.
        /// </summary>
        /// <param name="name">The name of the category to retrieve</param>
        /// <returns>A category if found, otherwise a NotFound result</returns>
        [HttpGet("GetCategoryByName/{name}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryByName(string name)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(u => u.Name == name);

            if (category == null)
            {
                return NotFound(new
                {
                    title = "Not Found",
                    code = "404",
                    message = $"User with Name: {name} doesn't exist"
                });
            }

            return Ok(category.ToDTO());
        }
        #endregion
        #region Update
        /// <summary>
        /// PUT: ElectroDepot/Categories/Update/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDTO categoryDTO)
        {
            Category category = categoryDTO.ToCategory();
            category.CategoryID = id;

            if (!CategoryExists(id))
            {
                return NotFound();
            }

            Category? existingCat = await _context.Categories.FirstOrDefaultAsync(x=>x.Name == categoryDTO.Name);
            
            if(existingCat != null)
            {
                // Teraz sprawdź czy to ten sam
                if(existingCat.CategoryID != id)
                {
                    // ID są inne czyli ktoś chce ustawić taką nazwe jaka już istnieje w systemie
                    return Conflict(new { title = "Conflict", status = 409, message = $"Category with name:'{category.Name}' already exists" });
                }
                else
                {
                    category.Description = categoryDTO.Description;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception exception)
                    {
                        return BadRequest();
                    }
                    return Ok(category.ToDTO());
                }
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound(new { title = "Not Found", status = 404, message = "Category with this ID doesn't exist" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(category.ToDTO());
        }
        #endregion
        #region Delete
        /// <summary>
        /// DELETE: ElectroDepot/Categories/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { title = "Not Found", status = 404, message = "Category with this name already exists" });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }
    }
}

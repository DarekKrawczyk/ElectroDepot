using ElectroDepotClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Models;

namespace Server.Controllers
{
    [Route("ElectroDepot/[controller]")]
    [ApiController]
    public class PredefinedImagesController : CustomControllerBase
    {
        public PredefinedImagesController(DatabaseContext context) : base(context)
        {
        }

        /// <summary>
        /// GET: api/PredefinedImages
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPredefinedImages")]
        public async Task<ActionResult<IEnumerable<PredefinedImageDTO>>> GetAllPredefinedImages()
        {
            return await _context.PredefinedImage.Select(x=>x.ToDTO()).ToListAsync();
        }

        /// <summary>
        /// GET: api/PredefinedImages/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetPredefinedImageByID/{id}")]
        public async Task<ActionResult<PredefinedImageDTO>> GetPredefinedImageByID(int id)
        {
            PredefinedImage? predefinedImage = await _context.PredefinedImage.FindAsync(id);

            if (predefinedImage == null)
            {
                return NotFound();
            }

            return predefinedImage.ToDTO();
        }

        /// <summary>
        /// PUT: api/PredefinedImages/5
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="predefinedImage"></param>
        /// <returns></returns>
        [HttpPut("UpdatePredefinedImage/{ID}")]
        public async Task<IActionResult> UpdatePredefinedImage(int ID, UpdatePredefinedImageDTO predefinedImageDTO)
        {
            PredefinedImage? image = await _context.PredefinedImage.FindAsync(ID);

            if(image == null)
            {
                return NotFound();
            }

            image.Name = predefinedImageDTO.Name;
            image.Category = predefinedImageDTO.Category;
            image.Image = predefinedImageDTO.Image;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PredefinedImageExists(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        /// <summary>
        /// POST: api/PredefinedImages
        /// </summary>
        /// <param name="predefinedImage"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<PredefinedImageDTO>> PostPredefinedImage(CreatePredefinedImageDTO predefinedImageDTO)
        {
            _context.PredefinedImage.Add(predefinedImageDTO.ToModel());
            await _context.SaveChangesAsync();

            return Ok(predefinedImageDTO.ToModel().ToDTO());
        }

        /// <summary>
        /// DELETE: api/PredefinedImages/ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{ID}")]
        public async Task<IActionResult> Delete(int ID)
        {
            PredefinedImage? predefinedImage = await _context.PredefinedImage.FindAsync(ID);
            if (predefinedImage == null)
            {
                return NotFound();
            }

            _context.PredefinedImage.Remove(predefinedImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PredefinedImageExists(int id)
        {
            return _context.PredefinedImage.Any(e => e.PredefinedImageID == id);
        }
    }
}

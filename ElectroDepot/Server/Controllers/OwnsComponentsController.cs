using Server.Models;
using Server.Context;
using Server.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectroDepotClassLibrary.DTOs;

namespace Server.Controllers
{
    [Route("ElectroDepot/[controller]")]
    [ApiController]
    public class OwnsComponentsController : CustomControllerBase
    {
        public OwnsComponentsController(DatabaseContext context) : base(context)
        {
        }
        #region Create
        /// <summary>
        /// POST: ElectroDepot/OwnsComponents/Create
        /// </summary>
        /// <param name="ownsComponent"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<OwnsComponentDTO>> Create(CreateOwnsComponentDTO ownsComponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User? existingUser = await _context.Users.FindAsync(ownsComponent.UserID);

            if (existingUser == null)
            {
                return BadRequest(new { title = "User not found", status = 400, message = "User with this ID doesn't exist." });
            }

            Component? existingComponent = await _context.Components.FindAsync(ownsComponent.ComponentID);

            if (existingComponent == null)
            {
                return BadRequest(new { title = "Component not found", status = 400, message = "Component with this ID doesn't exist." });
            }

            OwnsComponent createdOwnsComponent = ownsComponent.ToOwnComponent();

            _context.OwnsComponent.Add(createdOwnsComponent);
            await _context.SaveChangesAsync();

            return Ok(createdOwnsComponent.ToOwnsComponentDTO());
        }
        #endregion
        #region Read
        /// <summary>
        /// GET: ElectroDepot/OwnsComponents/GetAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<OwnsComponentDTO>>> GetAllOwnsComponent()
        {
            return await _context.OwnsComponent.Select(x => x.ToOwnsComponentDTO()).ToListAsync();
        }

        /// <summary>
        /// GET: ElectroDepot/OwnsComponents/GetByID/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetByID/{id}")]
        public async Task<ActionResult<OwnsComponentDTO>> GetOwnComponentByID(int id)
        {
            var ownsComponent = await _context.OwnsComponent.FindAsync(id);

            if (ownsComponent == null)
            {
                return NotFound();
            }

            return ownsComponent.ToOwnsComponentDTO();
        }

        [HttpGet("GetAllUnusedComponents/{userID}")]
        public async Task<ActionResult<IEnumerable<OwnsComponentDTO>>> GetAllUnusedComponents(int userID)
        {
            User user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                return NotFound();
            }

            //IEnumerable<OwnsComponent> result = await (from ownedComponent in _context.OwnsComponent
            //                                           join usedInProjComp in (from component in _context.Components
            //                                                                   join projectComponent in _context.ProjectComponents
            //                                                                   on component.ComponentID equals projectComponent.ComponentID
            //                                                                   into groupedByComponentID
            //                                                                   select new { ComponentID = component.ComponentID, Quantity = groupedByComponentID.Sum(x => x.Quantity) })
            //                                           on ownedComponent.ComponentID equals usedInProjComp.ComponentID
            //                                           where ownedComponent.UserID == userID
            //                                           select new OwnsComponent()
            //                                           {
            //                                               OwnsComponentID = ownedComponent.OwnsComponentID,
            //                                               UserID = ownedComponent.UserID,
            //                                               ComponentID = ownedComponent.ComponentID,
            //                                               Quantity = ownedComponent.Quantity - usedInProjComp.Quantity,
            //                                           }).ToListAsync();

            IEnumerable<OwnsComponent> result = await (from ownedComponent in _context.OwnsComponent
                                                       join usedInProjComp in (from component in _context.Components
                                                                               join projectComponent in _context.ProjectComponents
                                                                               on component.ComponentID equals projectComponent.ComponentID
                                                                               into groupedByComponentID
                                                                               select new { ComponentID = component.ComponentID, Quantity = groupedByComponentID.Sum(x => x.Quantity) })
                                                       on ownedComponent.ComponentID equals usedInProjComp.ComponentID
                                                       where ownedComponent.UserID == userID
                                                       select new OwnsComponent()
                                                       {
                                                           OwnsComponentID = ownedComponent.OwnsComponentID,
                                                           UserID = ownedComponent.UserID,
                                                           ComponentID = ownedComponent.ComponentID,
                                                           Quantity = ownedComponent.Quantity - usedInProjComp.Quantity,
                                                       }).ToListAsync();

            var grouped = (from ownedComponent in result
                           group ownedComponent by ownedComponent.ComponentID
                           into groupedByComponentID
                           select new
                           {
                               ComponentID = groupedByComponentID.Key,
                               Quantity = groupedByComponentID.Sum(x=>x.Quantity)
                           }).ToList();

            //var result = await (from ownedComponent in (from component in (from comp in _context.Components
            //                                                              join ownsComp in _context.OwnsComponent
            //                                                              on comp.ComponentID equals ownsComp.ComponentID
            //                                                              where ownsComp.UserID == userID
            //                                                              select new OwnsComponent()
            //                                                              {
            //                                                                  OwnsComponentID = ownsComp.OwnsComponentID,
            //                                                                  UserID = userID,
            //                                                                  ComponentID = ownsComp.ComponentID,
            //                                                                  Quantity = ownsComp.Quantity
            //                                                              })
            //                                           join projectComponent in _context.ProjectComponents
            //                                           on component.ComponentID equals projectComponent.ComponentID
            //                                           into groupedByComponentID
            //                                           select new OwnsComponent 
            //                                           { 
            //                                               OwnsComponentID = component.OwnsComponentID,
            //                                               UserID = userID,
            //                                               ComponentID = component.ComponentID, 
            //                                               Quantity = component.Quantity - groupedByComponentID.Sum(x => x.Quantity) 
            //                                           }) group new
            //                                           {
            //                                               OwnsComponentID = ownedComponent.OwnsComponentID,
            //                                               UserID = ownedComponent.UserID,
            //                                               //ComponentID = ownedComponent.ComponentID,
            //                                               //Quantity = ownedComponent.Quantity
            //                                           } by ownedComponent.ComponentID into groupedByCompID
            //                                           orderby groupedByCompID.Key
            //                                           select groupedByCompID).ToListAsync();

            //return Ok(result);
            return Ok(result.Select(x => x.ToOwnsComponentDTO()).ToList());
        }

        /// <summary>
        /// Get amount of 'Component' with such ID for given 'User'
        /// GET: ElectroDepot/OwnsComponents/GetOwnComponentFromUser/{UserID}/Component/{ComponentID}
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ComponentID"></param>
        /// <returns></returns>
        [HttpGet("GetOwnComponentFromUser/{UserID}/Component/{ComponentID}")]
        public async Task<ActionResult<OwnsComponentDTO>> GetOwnComponentFromUser(int UserID, int ComponentID)
        {
            User? user = await _context.Users.FindAsync(UserID);

            if (user == null)
            {
                return NotFound();
            }

            Component? component = await _context.Components.FindAsync(ComponentID);

            if (component == null)
            {
                return NotFound();
            }

            IEnumerable<OwnsComponentDTO> foundComponents = await _context.OwnsComponent.Where(x => x.UserID == UserID && x.ComponentID == ComponentID).Select(y => y.ToOwnsComponentDTO()).ToListAsync();
            OwnsComponentDTO foundComponent = foundComponents.FirstOrDefault();
            if (foundComponent == null)
            {
                return NotFound();
            }

            return Ok(foundComponent);
        }

        /// <summary>
        /// GET: ElectroDepot/OwnsComponents/GetAllOwnComponentFromUser/{UserID}
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet("GetAllOwnComponentFromUser/{UserID}")]
        public async Task<ActionResult<IEnumerable<OwnsComponentDTO>>> GetAllOwnComponentFromUser(int UserID)
        {
            User? user = await _context.Users.FindAsync(UserID);

            if (user == null)
            {
                return NotFound();
            }

            IEnumerable<OwnsComponentDTO> foundComponents = await _context.OwnsComponent.Where(x => x.UserID == UserID).Select(y => y.ToOwnsComponentDTO()).ToListAsync();

            return Ok(foundComponents);
        }

        /// <summary>
        /// GET: ElectroDepot/OwnsComponents/GetAllFreeToUseFromUser/{UserID}
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet("GetAllFreeToUseFromUser/{UserID}")]
        public async Task<ActionResult<IEnumerable<OwnsComponentDTO>>> GetAllFreeToUseFromUser(int UserID)
        {
            User? user = await _context.Users.FindAsync(UserID);

            if (user == null)
            {
                return NotFound();
            }

            IEnumerable<OwnsComponent> unusedComponents = await _context.OwnsComponent
                .Where(oc => !_context.ProjectComponents.Any(pc => pc.ComponentID == oc.ComponentID) && oc.UserID == UserID)
                .ToListAsync();

            return Ok(unusedComponents);
        }

        /// <summary>
        /// GET: ElectroDepot/OwnsComponents/GetAllUsedFromUser/{UserID}
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet("GetAllUsedFromUser/{UserID}")]
        public async Task<ActionResult<IEnumerable<OwnsComponentDTO>>> GetAllUsedFromUser(int UserID)
        {
            User? user = await _context.Users.FindAsync(UserID);

            if (user == null)
            {
                return NotFound();
            }

            IEnumerable<OwnsComponent> unusedComponents = await _context.OwnsComponent
                .Where(oc => _context.ProjectComponents.Any(pc => pc.ComponentID == oc.ComponentID) && oc.UserID == UserID)
                .ToListAsync();

            return Ok(unusedComponents);
        }
        #endregion
        #region Update
        /// <summary>
        /// PUT: ElectroDepot/OwnsComponents/Update/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateOwnsComponentDTO"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateOwnsComponent(int id, UpdateOwnsComponentDTO updateOwnsComponentDTO)
        {
            OwnsComponent? existingOwnsComponent = await _context.OwnsComponent.FindAsync(id);
            if (existingOwnsComponent == null)
            {
                return BadRequest(new { title = "OwnsComponent not found", status = 400, message = "OwnsComponent with this ID doesn't exist." });
            }

            existingOwnsComponent.Quantity = updateOwnsComponentDTO.Quantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnsComponentExists(id))
                {
                    return NotFound(new { title = "Update failed", status = 404, message = "OwnsComponent no longer exists." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion
        #region Delete
        /// <summary>
        /// DELETE: ElectroDepot/OwnsComponents/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOwnsComponent(int id)
        {
            var ownsComponent = await _context.OwnsComponent.FindAsync(id);
            if (ownsComponent == null)
            {
                return NotFound();
            }

            _context.OwnsComponent.Remove(ownsComponent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion


        private bool OwnsComponentExists(int id)
        {
            return _context.OwnsComponent.Any(e => e.OwnsComponentID == id);
        }
    }
}

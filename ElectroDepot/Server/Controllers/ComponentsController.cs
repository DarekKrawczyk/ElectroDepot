using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.ExtensionMethods;
using Server.Models;

namespace Server.Controllers
{
    [Route("ElectroDepot/[controller]")]
    [ApiController]
    public class ComponentsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ImageStorageService ISS;

        public ComponentsController(DatabaseContext context)
        {
            _context = context;
            ISS = new ImageStorageService(AppDomain.CurrentDomain.BaseDirectory);
            ISS.Initialize();
        }
        #region Create
        /// <summary>
        /// POST: ElectroDepot/Components/Create
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ComponentDTO>> CreateComponent(CreateComponentDTO component)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Component? existingComponent = await _context.Components.FirstOrDefaultAsync(u => u.Name == component.Name);

            if (existingComponent != null)
            {
                return Conflict(new { title = "Conflict", status = 409, message = "Component with this name already exists." });
            }

            Component newComponent = component.ToModel();
            newComponent.ImageURI = ISS.InsertComponentImage(component.Image);

            _context.Components.Add(newComponent);
            await _context.SaveChangesAsync();


            return Ok(newComponent.ToDTO());
        }
        #endregion
        #region Read
        /// <summary>
        /// GET: ElectroDepot/Components/GetAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ComponentDTO>>> GetComponents()
        {
            return Ok(await _context.Components.Select(x=>x.ToDTOWithImage(ISS)).ToListAsync());
        }

        [HttpGet("GetImageOfComponent/{ComponentID}")]
        public async Task<ActionResult<byte[]>> GetImageOfComponent(int ComponentID)
        {
            byte[] image = new byte[0] { };
            try
            {
                Component? component = await _context.Components.FindAsync(ComponentID);

                if(component == null)
                {
                    return NotFound();
                }

                image = ISS.RetrieveComponentImage(component.ImageURI);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to retrieve image");
            }
            return Ok(image);   
        }

        /// <summary>
        /// GET: ElectroDepot/Components/GetComponentByID/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetComponentByID/{id}")]
        public async Task<ActionResult<ComponentDTO>> GetComponent(int id)
        {
            var component = await _context.Components.FindAsync(id);

            if (component == null)
            {
                return NotFound();
            }

            return component.ToDTO();
        }

        /// <summary>
        /// GET: ElectroDepot/Components/GetComponentByName?name=someName
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetComponentByName/{name}")]
        public async Task<ActionResult<ComponentDTO>> GetComponentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { title = "Invalid request", message = "Name cannot be null or empty." });
            }

            Component? component = await _context.Components.FirstOrDefaultAsync(x=>x.Name == name);

            if (component == null)
            {
                return NotFound();
            }

            return Ok(component.ToDTO());
        }

        /// <summary>
        /// GET: ElectroDepot/Components/GetAvailableComponentsFromUser/{ID}
        /// </summary>
        /// <param name="ID">UserID</param>
        /// <returns></returns>
        [HttpGet("GetAvailableComponentsFromUser/{ID}")]
        public async Task<ActionResult<IEnumerable<ComponentDTO>>> GetAvailableComponentsFromUser(int ID)
        {
            User? user = await _context.Users.FindAsync(ID);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            try
            {
                //IEnumerable<Component> usersComponents = await (from component in (from ownsComponent in _context.OwnsComponent
                //                                                                   join component in _context.Components
                //                                                                   on ownsComponent.ComponentID equals component.ComponentID
                //                                                                   where ownsComponent.UserID == user.UserID
                //                                                                   select new Component
                //                                                                   {
                //                                                                       ComponentID = component.ComponentID,
                //                                                                       CategoryID = component.CategoryID,
                //                                                                       Name = component.Name,
                //                                                                       Manufacturer = component.Manufacturer,
                //                                                                       Description = component.Description
                //                                                                   })
                //                                                group component by component.ComponentID
                //                                   into groupedByComponentID
                //                                                select new Component()
                //                                                {
                //                                                    ComponentID = groupedByComponentID.Key,
                //                                                    CategoryID = groupedByComponentID.FirstOrDefault().CategoryID,
                //                                                    Name = groupedByComponentID.FirstOrDefault().Name,
                //                                                    Manufacturer = groupedByComponentID.FirstOrDefault().Manufacturer,
                //                                                    Description = groupedByComponentID.FirstOrDefault().Description
                //                                                }).ToListAsync();
                //return Ok(usersComponents.Select(x => x.ToDTO()).ToList());
                IEnumerable<Component> usersComponents = await (from ownsComponent in _context.OwnsComponent
                                                                   join component in _context.Components
                                                                   on ownsComponent.ComponentID equals component.ComponentID
                                                                   where ownsComponent.UserID == user.UserID
                                                                   select new Component()
                                                                   {
                                                                       ComponentID = component.ComponentID,
                                                                       CategoryID = component.CategoryID,
                                                                       Name = component.Name,
                                                                       Manufacturer = component.Manufacturer,
                                                                       ShortDescription = component.ShortDescription,
                                                                       LongDescription = component.LongDescription,
                                                                       DatasheetLink = component.DatasheetLink,
                                                                       ImageURI = component.ImageURI,
                                                                   }).ToListAsync();
                return Ok(usersComponents.Select(x=>x.ToDTO()));
            }
            catch (Exception ex)
            {
                return BadRequest("Error ocurred");
            }
        }

        [HttpGet("GetAvailableComponentsFromUserWithImage/{ID}")]
        public async Task<ActionResult<IEnumerable<ComponentDTO>>> GetAvailableComponentsFromUserWithImage(int ID)
        {
            User? user = await _context.Users.FindAsync(ID);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            try
            {
                IEnumerable<Component> usersComponents = await (from ownsComponent in _context.OwnsComponent
                                                                join component in _context.Components
                                                                on ownsComponent.ComponentID equals component.ComponentID
                                                                where ownsComponent.UserID == user.UserID
                                                                select new Component()
                                                                {
                                                                    ComponentID = component.ComponentID,
                                                                    CategoryID = component.CategoryID,
                                                                    Name = component.Name,
                                                                    Manufacturer = component.Manufacturer,
                                                                    ShortDescription = component.ShortDescription,
                                                                    DatasheetLink = component.DatasheetLink,
                                                                    LongDescription = component.LongDescription,
                                                                    ImageURI = component.ImageURI,
                                                                }).ToListAsync();
                return Ok(usersComponents.Select(x => x.ToDTOWithImage(ISS)));
            }
            catch (Exception ex)
            {
                return BadRequest("Error ocurred");
            }
        }

        /// <summary>
        /// GET: ElectroDepot/Components/GetUserComponents/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetUserComponents/{ID}")]
        public async Task<ActionResult<IEnumerable<ComponentDTO>>> GetUserComponents(int ID)
        {
            User? user = await _context.Users.FindAsync(ID);
            
            if(user == null)
            {
                return BadRequest("User not found");
            }

            try
            {
                IEnumerable<Component> usersComponents = await (from ownsComponent in _context.OwnsComponent
                                                                   join component in _context.Components
                                                                   on ownsComponent.ComponentID equals component.ComponentID
                                                                   where ownsComponent.UserID == user.UserID
                                                                   select new Component()
                                                                   {
                                                                       ComponentID = component.ComponentID,
                                                                       CategoryID = component.CategoryID,
                                                                       Name = component.Name,
                                                                       Manufacturer = component.Manufacturer,
                                                                       ShortDescription = component.ShortDescription,
                                                                       LongDescription = component.LongDescription,
                                                                       DatasheetLink = component.DatasheetLink,
                                                                       ImageURI = component.ImageURI,
                                                                   }).ToListAsync();
                return Ok(usersComponents);
            }
            catch(Exception ex)
            {
                return BadRequest("Error ocurred");
            }

        }

        [HttpGet("GetPurchaseItemsFromComponent/{ComponentID}")]
        public async Task<ActionResult<IEnumerable<PurchaseItemDTO>>> GetPurchaseItemsFromComponent(int ComponentID)
        {
            Component? component = await _context.Components.FindAsync(ComponentID);

            if (component == null)
            {
                return BadRequest();
            }

            try
            {
                IEnumerable<PurchaseItem> purchaseItemFromComponent = await _context.PurchaseItems.Where(x=>x.ComponentID == ComponentID).ToListAsync();
                return Ok(purchaseItemFromComponent.Select(x=>x.ToPurchaseItemDTO()));
            }
            catch (Exception ex)
            {
                return BadRequest("Error ocurred");
            }
        }
        #endregion
        #region Update
        /// <summary>
        /// PUT: ElectroDepot/Components/Update/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<ComponentDTO>> Update(int id, UpdateComponentDTO component)
        {
            Component? existingComponent = await _context.Components.FindAsync(id);

            if (existingComponent == null)
            {
                return NotFound(new { title = "Not Found", code = "404", message = $"Component with ID:{id} doesn't exsit" });
            }

            existingComponent.Name = component.Name;
            existingComponent.Manufacturer = component.Manufacturer;
            existingComponent.ShortDescription = component.ShortDescription;
            existingComponent.LongDescription = component.LongDescription;
            existingComponent.DatasheetLink = component.DatasheetLink;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingComponent.ToDTO());
        }
        #endregion
        #region Delete
        /// <summary>
        /// DELETE: ElectroDepot/Components/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteComponent(int id)
        {
            var component = await _context.Components.FindAsync(id);
            if (component == null)
            {
                return NotFound();
            }

            _context.Components.Remove(component);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
        private bool ComponentExists(int id)
        {
            return _context.Components.Any(e => e.ComponentID == id);
        }
    }
}

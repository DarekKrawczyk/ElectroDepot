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
    public class ProjectsController : CustomControllerBase
    {
        public ProjectsController(DatabaseContext context) : base(context)
        {
        }
        #region Create
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ProjectDTO>> CreateProject(CreateProjectDTO project)
        {
            Project newProject = project.ToProject(_imageStorageService);

            newProject.CreatedAt = DateTime.Now;

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();

            return Ok(newProject.ToProjectDTO(_imageStorageService));
        }
        #endregion
        #region Read
        /// <summary>
        /// GET: ElectroDepot/Projects/GetAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjects()
        {
            return await _context.Projects.Select(x => x.ToProjectDTO(_imageStorageService)).ToListAsync();
        }

        /// <summary>
        /// GET: ElectroDepot/Projects/GetAllOfUser/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetAllOfUser/{ID}")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjectsOfUser(int ID)
        {
            User? user = await _context.Users.FindAsync(ID);

            if (user == null)
            {
                return NotFound();
            }

            return await _context.Projects.Where(x => x.UserID == ID).Select(x => x.ToProjectDTO(_imageStorageService)).ToListAsync();
        }

        [HttpGet("GetProjectOfProjectComponent/{ProjectComponentID}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectOfProjectComponent(int ProjectComponentID)
        {
            ProjectComponent? pc = await _context.ProjectComponents.FindAsync(ProjectComponentID);

            if (pc == null)
            {
                return NotFound();
            }

            Project? project = await _context.Projects.Where(x => x.ProjectID == pc.ProjectID).FirstOrDefaultAsync();

            return Ok(project.ToProjectDTO(_imageStorageService));
        }

        /// <summary>
        /// GET: ElectroDepot/Projects/GetByID/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetByID/{ID}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectByID(int ID)
        {
            var project = await _context.Projects.FindAsync(ID);

            if (project == null)
            {
                return NotFound();
            }

            return project.ToProjectDTO(_imageStorageService);
        }

        /// <summary>
        /// GET: ElectroDepot/Projects/GetImageOfProjectByID/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetImageOfProjectByID/{ID}")]
        public async Task<ActionResult<byte[]>> GetImageOfProjectByID(int ID)
        {
            Project project = await _context.Projects.FindAsync(ID);

            if (project == null)
            {
                return NotFound();
            }

            byte[] image = _imageStorageService.RetrieveProjectImage(project.ImageURI);

            return Ok(image);
        }

        /// <summary>
        /// GET: ElectroDepot/Projects/GetPriceByID/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetPriceByID/{ID}")]
        public async Task<ActionResult<double>> GetProjectPriceByID(int ID)
        {
            Project? project = await _context.Projects.FindAsync(ID);

            if (project == null)
            {
                return NotFound();
            }

            try
            {
                IEnumerable<Component> componentsOfProject = await (from projects in _context.Projects
                                                                    join projectComponents in _context.ProjectComponents
                                                                    on projects.ProjectID equals projectComponents.ProjectID
                                                                    join components in _context.Components
                                                                    on projectComponents.ComponentID equals components.ComponentID
                                                                    where projects.ProjectID == ID
                                                                    select new Component()
                                                                    {
                                                                        ComponentID = components.ComponentID,
                                                                        CategoryID = components.CategoryID,
                                                                        Name = components.Name,
                                                                        Manufacturer = components.Manufacturer,
                                                                        ShortDescription = components.ShortDescription,
                                                                        LongDescription = components.LongDescription
                                                                    }).ToListAsync();

                List<int> componentsIDs = componentsOfProject.Select(x => x.ComponentID).ToList();

                IEnumerable<PurchaseItem> projectPurchaseItemss = await (from purchaseItem in _context.PurchaseItems
                                                                         where componentsIDs.Contains(purchaseItem.ComponentID)
                                                                         select new PurchaseItem()
                                                                         {
                                                                             PurchaseItemID = purchaseItem.PurchaseItemID,
                                                                             ComponentID = purchaseItem.ComponentID,
                                                                             PurchaseID = purchaseItem.PurchaseID,
                                                                             Quantity = purchaseItem.Quantity,
                                                                             PricePerUnit = purchaseItem.PricePerUnit
                                                                         }).ToListAsync();

                double result = projectPurchaseItemss.Sum(x => x.Quantity * x.PricePerUnit);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// GET: ElectroDepot/Projects/GetAllComponentsFromProject/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetAllComponentsFromProject/{ID}")]
        public async Task<ActionResult<IEnumerable<ComponentDTO>>> GetAllComponentsFromProject(int ID)
        {
            try
            {
                Project? project = await _context.Projects.FindAsync(ID);

                if (project == null)
                {
                    return NotFound();
                }
                IEnumerable<Component> componentsFromProject = await (from projectComponents in _context.ProjectComponents
                                                                      join components in _context.Components
                                                                      on projectComponents.ComponentID equals components.ComponentID
                                                                      where projectComponents.ProjectID == ID
                                                                      select new Component
                                                                      {

                                                                          ComponentID = components.ComponentID,
                                                                          CategoryID = components.CategoryID,
                                                                          Name = components.Name,
                                                                          Manufacturer = components.Manufacturer,
                                                                          ShortDescription = components.ShortDescription,
                                                                          LongDescription = components.LongDescription
                                                                      }).ToListAsync();
                return Ok(componentsFromProject.Select(x=>x.ToDTOWithImage(_imageStorageService)));
            }
            catch (Exception exception)
            {
                return BadRequest();
            }
        }
        #endregion
        #region Update
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectDTO"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<ProjectDTO>> UpdateProject(int id, UpdateProjectDTO projectDTO)
        {
            Project? project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Name = projectDTO.Name;
            project.Description = projectDTO.Description;

            try
            {
                await _context.SaveChangesAsync();
                try
                {
                    _imageStorageService.UpdateProjectImage(project.ImageURI, projectDTO.Image);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Eception while updating image for '{project.Name}' project");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(project.ToProjectDTO(_imageStorageService));
        }
        #endregion
        #region Delete
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            Project project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            string imageURI = project.ImageURI;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            try
            {
                _imageStorageService.RemoveProjectImage(imageURI);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception while removing image for deleted project!");
            }

            return Ok();
        }
        #endregion
        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }
    }
}

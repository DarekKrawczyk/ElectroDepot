using ElectroDepotClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.ExtensionMethods;
using Server.Models;

namespace Server.Controllers
{
    [Route("ElectroDepot/[controller]")]
    [ApiController]
    public class ProjectComponentsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProjectComponentsController(DatabaseContext context)
        {
            _context = context;
        }
        #region Create
        /// <summary>
        /// GET: ElectroDepot/ProjectComponents/Create
        /// </summary>
        /// <param name="projectComponentDTO"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ProjectComponentDTO>> CreateProjectComponent(CreateProjectComponentDTO projectComponentDTO)
        {
            ProjectComponent newProjectComponent = projectComponentDTO.ToProjectComponent();

            _context.ProjectComponents.Add(newProjectComponent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByID), new { id = newProjectComponent.ProjectComponentID }, newProjectComponent);
        }
        #endregion
        #region Read
        /// <summary>
        /// GET: ElectroDepot/ProjectComponents/GetAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProjectComponentDTO>>> GetAllProjectComponents()
        {
            return await _context.ProjectComponents.Select(x => x.ToProjectComponentDTO()).ToListAsync();
        }

        /// <summary>
        /// GET: ElectroDepot/ProjectComponents/GetAllByProject
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetAllByProject/{ID}")]
        public async Task<ActionResult<IEnumerable<ProjectComponentDTO>>> GetAllProjectComponentsOfProject(int ID)
        {
            Project? project = await _context.Projects.FindAsync(ID);

            if (project == null)
            {
                return NotFound();
            }

            return await _context.ProjectComponents.Where(x => x.ProjectID == ID).Select(x => x.ToProjectComponentDTO()).ToListAsync();
        }

        [HttpGet("GetProjectComponentsOfComponents/{ComponentID}")]
        public async Task<ActionResult<IEnumerable<ProjectComponentDTO>>> GetProjectComponentsOfComponents(int ComponentID)
        {
            Component? component = await _context.Components.FindAsync(ComponentID);
            if(component == null)
            {
                return NotFound();
            }
            
            IEnumerable<ProjectComponent> project = await (from projectComponent in _context.ProjectComponents
                                                           join comp in _context.Components
                                                           on projectComponent.ComponentID equals comp.ComponentID
                                                           where projectComponent.ComponentID == ComponentID
                                                           select new ProjectComponent()
                                                           {
                                                               ProjectComponentID = projectComponent.ComponentID,
                                                               ComponentID = projectComponent.ComponentID,
                                                               ProjectID = projectComponent.ProjectID,
                                                               Quantity = projectComponent.Quantity,
                                                           }).ToListAsync();

            return Ok(project.Select(x=>x.ToProjectComponentDTO()).ToList());
        }

        /// <summary>
        /// GET: ElectroDepot/ProjectComponents/GetProjectComponentForProject/{ProjectID}/Component/{ComponentID}
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ComponentID"></param>
        /// <returns></returns>
        [HttpGet("GetProjectComponentForProject/{ProjectID}/Component/{ComponentID}")]
        public async Task<ActionResult<IEnumerable<ProjectComponentDTO>>> GetProjectComponentForProject(int ProjectID, int ComponentID)
        {
            Project? project = await _context.Projects.FindAsync(ProjectID);

            if (project == null)
            {
                return NotFound();
            }

            Component? component = await _context.Components.FindAsync(ComponentID);

            if (component == null)
            {
                return NotFound();
            }

            try
            {
                IEnumerable<ProjectComponentDTO> foundProjectComponent = await _context.ProjectComponents.Where(x => x.ProjectID == ProjectID && x.ComponentID == ComponentID).Select(x => x.ToProjectComponentDTO()).ToListAsync();
                ProjectComponentDTO projectComponentDTO = foundProjectComponent.FirstOrDefault();
                if(projectComponentDTO != null)
                {
                    return Ok(projectComponentDTO);
                }
                return NotFound();
            }
            catch(Exception exception)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// GET: ElectroDepot/ProjectComponents/GetAllByProject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetByID/{id}")]
        public async Task<ActionResult<ProjectComponentDTO>> GetByID(int id)
        {
            var project = await _context.ProjectComponents.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project.ToProjectComponentDTO();
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
        public async Task<IActionResult> UpdateProjectComponent(int id, UpdateProjectComponentDTO projectDTO)
        {
            ProjectComponent? projectComponent = await _context.ProjectComponents.FindAsync(id);
            if (projectComponent == null)
            {
                return NotFound();
            }

            projectComponent.Quantity = projectDTO.Quantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectComponentExists(id))
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
        #endregion
        #region Delete
        /// <summary>
        /// DELETE: ElectroDepot/ProjectComponents/Delete/{ID}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProjectComponent(int id)
        {
            ProjectComponent? projectComponent = await _context.ProjectComponents.FindAsync(id);
            if (projectComponent == null)
            {
                return NotFound();
            }

            _context.ProjectComponents.Remove(projectComponent);
            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
        private bool ProjectComponentExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }
    }   
}

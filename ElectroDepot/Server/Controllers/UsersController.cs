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

    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }

        #region Create
        /// <summary>
        /// POST: ElectroDepot/Users/Create
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User? existingUserByUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUserByUsername != null)
            {
                return Conflict(new { title = "Conflict", status = 409, message = "A user with this username already exists." });
            }

            User? existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUserByEmail != null)
            {
                return Conflict(new { title = "Conflict", status = 409, message = "A user with this email already exists." });
            }

            User userToCreate = user.ToUser();

            _context.Users.Add(userToCreate);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = userToCreate.ToDTO().ID }, userToCreate.ToDTO());
        }

        #endregion
        #region Read
        /// <summary>
        /// GET: ElectroDepot/Users/GetAll
        /// </summary>
        /// <returns>All users in database</returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            return Ok(await _context.Users.Select(x=>x.ToDTO()).ToListAsync());
        }

        [HttpGet("ServerStatus")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetServerStatus()
        {
            return Ok("Running");
        }

        /// <summary>
        /// GET: ElectroDepot/Users/GetUserByID/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User with given ID</returns>
        [HttpGet("GetUserByID/{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { title = "not found", status = 404 });
            }

            return Ok(user.ToDTO());
        }

        /// <summary>
        /// GET: ElectroDepot/Users/GetUserByEMail/{id}
        /// </summary>
        /// <param name="EMail"></param>
        /// <returns></returns>
        [HttpGet("GetUserByEMail/{EMail}")]
        public async Task<ActionResult<UserDTO>> GetUser(string EMail)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x=>x.Email == EMail);

            if (user == null)
            {
                return NotFound(new { title = "not found", status = 404 });
            }

            return Ok(user.ToDTO());
        }

        /// <summary>
        /// GET: ElectroDepot/Users/GetUserByUsername/{id}
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        [HttpGet("GetUserByUsername/{Username}")]
        public async Task<ActionResult<UserDTO>> GetUserByName(string Username)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Username == Username);

            if (user == null)
            {
                return NotFound(new { title = "not found", status = 404 });
            }

            return Ok(user.ToDTO());
        }
        #endregion
        #region Update
        /// <summary>
        /// PUT: ElectroDepot/Users/Create/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateUserDTO"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO updateUserDTO)
        {
            User userToBeEdited = await _context.Users.FindAsync(id);

            if(userToBeEdited == null)
            {
                NotFound();
            }

            userToBeEdited.Username = updateUserDTO.Username;
            userToBeEdited.Email = updateUserDTO.Email;
            userToBeEdited.Password = updateUserDTO.Password;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        /// DELETE: ElectroDepod/Users/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}

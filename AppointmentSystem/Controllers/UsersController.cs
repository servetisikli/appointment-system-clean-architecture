using System.Collections.Generic;
using System.Threading.Tasks;
using AppointmentSystem.Application.DTOs.UserDTOs;
using AppointmentSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO userDto)
        {
            var success = await _userService.UpdateUserAsync(id, userDto);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{userId}/roles/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole(int userId, string roleName)
        {
            var success = await _userService.AddUserToRoleAsync(userId, roleName);

            if (!success)
                return BadRequest("Failed to add user to role.");

            return NoContent();
        }

        [HttpDelete("{userId}/roles/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUserFromRole(int userId, string roleName)
        {
            var success = await _userService.RemoveUserFromRoleAsync(userId, roleName);

            if (!success)
                return BadRequest("Failed to remove user from role.");

            return NoContent();
        }

        [HttpGet("{userId}/roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(int userId)
        {
            var roles = await _userService.GetUserRolesAsync(userId);
            return Ok(roles);
        }
    }
}

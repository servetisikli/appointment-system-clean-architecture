using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentSystem.Application.DTOs.UserDTOs;
using AppointmentSystem.Application.Interfaces;

namespace AppointmentSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                LastLogin = u.LastLogin,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            });
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            var userRoles = await _userRepository.GetUserRolesAsync(id);

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin,
                Roles = userRoles.ToList()
            };
        }

        public async Task<bool> UpdateUserAsync(int id, UserDTO userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.IsActive = userDto.IsActive;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            await _userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> AddUserToRoleAsync(int userId, string roleName)
        {
            // We need a method to get roleId by name - for simplicity we'll use an enum or hardcoded value
            int roleId = roleName.ToLower() switch
            {
                "admin" => 1,
                "manager" => 2,
                "user" => 3,
                _ => 0
            };

            if (roleId == 0)
                return false;

            await _userRepository.AddUserToRoleAsync(userId, roleId);
            return true;
        }

        public async Task<bool> RemoveUserFromRoleAsync(int userId, string roleName)
        {
            // We need a method to get roleId by name - for simplicity we'll use an enum or hardcoded value
            int roleId = roleName.ToLower() switch
            {
                "admin" => 1,
                "manager" => 2,
                "user" => 3,
                _ => 0
            };

            if (roleId == 0)
                return false;

            await _userRepository.RemoveUserFromRoleAsync(userId, roleId);
            return true;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
        {
            return await _userRepository.GetUserRolesAsync(userId);
        }
    }
}

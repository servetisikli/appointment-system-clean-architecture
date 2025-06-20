using System.Collections.Generic;
using System.Threading.Tasks;
using AppointmentSystem.Application.DTOs.UserDTOs;

namespace AppointmentSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(int id, UserDTO userDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> AddUserToRoleAsync(int userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(int userId, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(int userId);
    }
}

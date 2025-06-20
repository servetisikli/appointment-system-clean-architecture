using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppointmentSystem.Application.DTOs.UserDTOs;
using AppointmentSystem.Application.Interfaces;
using AppointmentSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace AppointmentSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(
            IUserRepository userRepository, 
            IJwtService jwtService,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO model)
        {
            // Check if username already exists
            if (await _userRepository.ExistsByUsernameAsync(model.Username))
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Username already exists"
                };
            }

            // Check if email already exists
            if (await _userRepository.ExistsByEmailAsync(model.Email))
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Email already exists"
                };
            }

            // Create new user
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = _passwordHasher.HashPassword(model.Password),
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Save user to database
            var createdUser = await _userRepository.AddAsync(user);

            // Add user to default "User" role (Id=3)
            await _userRepository.AddUserToRoleAsync(createdUser.Id, 3);

            // Get user roles
            var userRoles = await _userRepository.GetUserRolesAsync(createdUser.Id);

            // Generate JWT token
            var token = _jwtService.GenerateToken(createdUser.Id, createdUser.Username, userRoles);

            // Return success response
            return new AuthResponseDTO
            {
                Success = true,
                Message = "User registered successfully",
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
                User = new UserDTO
                {
                    Id = createdUser.Id,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    IsActive = createdUser.IsActive,
                    CreatedAt = createdUser.CreatedAt,
                    LastLogin = createdUser.LastLogin,
                    Roles = new List<string>(userRoles)
                }
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO model)
        {
            // Get user by username
            var user = await _userRepository.GetByUsernameAsync(model.Username);

            // Check if user exists
            if (user == null)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // Verify password
            if (!_passwordHasher.VerifyPassword(user.PasswordHash, model.Password))
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // Check if user is active
            if (!user.IsActive)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Account is disabled"
                };
            }

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            // Get user roles
            var userRoles = await _userRepository.GetUserRolesAsync(user.Id);

            // Generate JWT token
            var token = _jwtService.GenerateToken(user.Id, user.Username, userRoles);

            // Return success response
            return new AuthResponseDTO
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
                User = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    LastLogin = user.LastLogin,
                    Roles = new List<string>(userRoles)
                }
            };
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _userRepository.ExistsByUsernameAsync(username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.ExistsByEmailAsync(email);
        }
    }
}

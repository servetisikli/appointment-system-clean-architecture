using System;
using System.Collections.Generic;

namespace AppointmentSystem.Application.DTOs.UserDTOs
{
    public class AuthResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime? Expiration { get; set; }
        public UserDTO? User { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AppointmentSystem.Domain.Entities
{
    public class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            Appointments = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}

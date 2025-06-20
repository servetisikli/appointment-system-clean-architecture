using System.Collections.Generic;

namespace AppointmentSystem.Domain.Entities
{
    public class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}

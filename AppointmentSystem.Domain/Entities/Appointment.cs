using System;

namespace AppointmentSystem.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Attendee { get; set; }
        
        // Added for user relationship
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
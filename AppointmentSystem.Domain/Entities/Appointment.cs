using System;

namespace AppointmentSystem.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Attendee { get; set; }
    }
}
using System.Collections.Generic;
using AppointmentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
    }
}
using AppointmentSystem.Application.Interfaces;
using AppointmentSystem.Domain.Entities;
using AppointmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task AddAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
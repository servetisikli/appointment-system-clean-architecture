using System.Collections.Generic;
using System.Threading.Tasks;
using AppointmentSystem.Domain.Entities;

namespace AppointmentSystem.Application.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<List<Appointment>> GetByUserIdAsync(int userId);
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int id);
    }
}
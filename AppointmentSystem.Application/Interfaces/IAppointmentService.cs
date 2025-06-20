using AppointmentSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.Application.Interfaces
{
    public interface IAppointmentService
    {        Task<List<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<List<Appointment>> GetByUserIdAsync(int userId);
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int id);
    }
}
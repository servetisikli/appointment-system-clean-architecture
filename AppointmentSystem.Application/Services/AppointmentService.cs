using AppointmentSystem.Application.Interfaces;
using AppointmentSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }        public Task<List<Appointment>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Appointment?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<List<Appointment>> GetByUserIdAsync(int userId) => _repository.GetByUserIdAsync(userId);
        public Task AddAsync(Appointment appointment) => _repository.AddAsync(appointment);
        public Task UpdateAsync(Appointment appointment) => _repository.UpdateAsync(appointment);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
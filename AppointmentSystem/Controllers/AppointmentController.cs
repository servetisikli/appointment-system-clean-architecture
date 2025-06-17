using AppointmentSystem.Application.Interfaces;
using AppointmentSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return NotFound();
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Appointment appointment)
        {
            await _appointmentRepository.AddAsync(appointment);
            return CreatedAtAction(nameof(Get), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Appointment appointment)
        {
            if (id != appointment.Id)
                return BadRequest();

            var existing = await _appointmentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _appointmentRepository.UpdateAsync(appointment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _appointmentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _appointmentRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
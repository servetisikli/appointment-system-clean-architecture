using AppointmentSystem.Application.Interfaces;
using AppointmentSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppointmentSystem.Controllers
{    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Admin can see all appointments
            if (User.IsInRole("Admin"))
            {
                var appointments = await _appointmentService.GetAllAsync();
                return Ok(appointments);
            }
            // Regular users can only see their own appointments
            else if (int.TryParse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier), out int userId))
            {
                var appointments = await _appointmentService.GetByUserIdAsync(userId);
                return Ok(appointments);
            }
            
            return Unauthorized();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound();
            return Ok(appointment);
        }        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Appointment appointment)
        {
            // Get the current user ID from the claims
            if (int.TryParse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier), out int userId))
            {
                appointment.UserId = userId;
            }
            else
            {
                return Unauthorized();
            }

            await _appointmentService.AddAsync(appointment);
            return CreatedAtAction(nameof(Get), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Appointment appointment)
        {
            if (id != appointment.Id)
                return BadRequest();

            var existing = await _appointmentService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _appointmentService.UpdateAsync(appointment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _appointmentService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _appointmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
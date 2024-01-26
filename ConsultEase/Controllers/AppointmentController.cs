using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ConsultEaseAPI.Controllers;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ConsultEaseBLL.Interfaces;
using ConsultEaseBLL.DTOs;
using ConsultEaseBLL.DTOs.Appointment;
using ConsultEaseBLL.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace ConsultEaseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentController> _logger;
        // TODO: handle logger
        
        public AppointmentController(IMapper mapper, IAppointmentService appointmentService, ILogger<AppointmentController> logger)
        {
            _logger = logger;
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetAppointments")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            return Ok(appointmentDtos);
        }
        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}:int", Name = "GetAppointmentById")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            return Ok(appointmentDto);
        }
        
        [HttpGet("counsellor/{counsellorId}:int", Name = "GetCounsellorAppointments")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> GetCounsellorAppointments([Required]int counsellorId)
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            return Ok(appointmentDtos);
        }
        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost(Name = "CreateAppointment")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentDto newAppointment)
        {
            try
            {
                await _appointmentService.CreateAppointmentAsync(newAppointment);
            }
            catch (CounsellingCategoryNotFoundException)
            {
                return NotFound($"Counselling category {newAppointment.CounsellingCategoryId} was not found");
            }
            catch (NullReferenceException)
            {
                return BadRequest("Appointment was not created, please check your input data");
            }
            return NoContent();
        }
        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("{appointmentId}:int",Name = "UpdateAppointment")]
        public async Task<IActionResult> UpdateAppointment(int appointmentId, UpdateApppointmentDto appointment)
        {
            try
            {
                await _appointmentService.UpdateAppointmentAsync(appointmentId, appointment);
            }
            catch (AppointmentNotFoundException)
            {
                return NotFound($"Appointment with id {appointmentId} was not found");
            }
            catch (CounsellingCategoryNotFoundException)
            {
                return NotFound($"Counselling category {appointment.Status} was not found");
            }
            return NoContent();
        }
    }
}
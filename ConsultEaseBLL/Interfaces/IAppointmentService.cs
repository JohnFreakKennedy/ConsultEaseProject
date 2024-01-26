using ConsultEaseBLL.DTOs.Appointment;

namespace ConsultEaseBLL.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<AppointmentDto> GetAppointmentByIdAsync(int id);
    Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto newAppointment);
    Task<int> UpdateAppointmentAsync(int appointmentId, UpdateApppointmentDto appointment);
    Task<int> DeleteAppointmentAsync(int appointmentId);
}
using AutoMapper;
using ConsultEaseBLL.DTOs.Appointment;
using ConsultEaseBLL.Exceptions;
using ConsultEaseBLL.Interfaces;
using ConsultEaseDAL.Entities;
using ConsultEaseDAL.Entities.Auth;
using ConsultEaseDAL.Entities.Enums;
using ConsultEaseDAL.Infrastructure.DependencyInjection.Abstractions;
using ConsultEaseDAL.Infrastructure.DependencyInjection.Implementations;



namespace ConsultEaseBLL.Services;

public class AppointmentService: IAppointmentService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public AppointmentService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _repositoryManager.AppointmentRepository.GetAllAppointmentsAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }
    
    public async Task<IEnumerable<AppointmentDto>> GetCounsellorAppointments(int counsellorId)
    {
        var appointments = await _repositoryManager.AppointmentRepository
            .GetAllAppointmentsAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments
            .Where(a => a.ProfessorId == counsellorId)
            .OrderBy(a => a.Priority)
            .ThenByDescending(a => a.AppointmentDate)
            .ThenBy(a=> a.AppointmentStatus)
            );
    }
    
    public async Task<IEnumerable<AppointmentDto>> GetStudentAppointments(int studentId)
    {
        var appointments = await _repositoryManager.AppointmentRepository
            .GetAllAppointmentsAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments
            .Where(a => a.StudentId == studentId));
    }
    public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _repositoryManager.AppointmentRepository.GetAppointmentByIdAsync(id);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto newAppointment)
    {
            var appointment = _mapper.Map<Appointment>(newAppointment);
            var counsellingCategory = await _repositoryManager.CounsellingCategoryRepository
                .GetCounsellingCategoryByIdAsync(newAppointment.CounsellingCategoryId);
            if (counsellingCategory is null)
                throw new CounsellingCategoryNotFoundException
                    ($"Counselling category with id {appointment.CounsellingCategoryId} was not found");
            appointment.Priority = counsellingCategory.AffectTimeDuration*appointment.RequestedTime;
            appointment.AppointmentStatus = AppointmentStatus.Scheduled;
            return _mapper.Map<AppointmentDto>
                (await _repositoryManager.AppointmentRepository!.CreateAppointmentAsync(appointment));
    }

    public async Task<int> UpdateAppointmentAsync(int appointmentId, UpdateApppointmentDto appointment)
    {
        var appointmentToUpdate = await _repositoryManager.AppointmentRepository.GetAppointmentByIdAsync(appointmentId);
        if (appointmentToUpdate == null) throw new AppointmentNotFoundException("Appointment with stated id was not found");
        appointmentToUpdate.AppointmentStatus = appointment.Status;
        return await _repositoryManager.AppointmentRepository.UpdateAppointmentAsync(appointmentToUpdate);
    }

    public async Task<int> DeleteAppointmentAsync(int appointmentId)
    {
        var appointmentToDelete = await _repositoryManager.AppointmentRepository.GetAppointmentByIdAsync(appointmentId);
        if (appointmentToDelete == null)
            throw new AppointmentNotFoundException("Appointment with stated id was not found");
        await _repositoryManager.AppointmentRepository.DeleteAppointmentAsync(appointmentToDelete);
        return appointmentToDelete.Id;
    }
}
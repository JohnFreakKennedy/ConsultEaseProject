using System.Collections.Generic;
using System.Threading.Tasks;
using ConsultEaseDAL.Entities;
using ConsultEaseDAL.Infrastructure.Abstractions.Base;

namespace ConsultEaseDAL.Infrastructure.Abstractions;

public interface IAppointmentRepository: IRepositoryBase<int, Appointment>
{
    
    Task<Appointment?> GetAppointmentByIdAsync(int id);
    Task<IEnumerable<Appointment>> GetAppointmentsByCounsellorIdAsync(int professorId);
    Task<IEnumerable<Appointment>> GetAppointmentsByStudentIdAsync(int studentId);
    Task<Appointment?> CreateAppointmentAsync(Appointment appointment);
    Task<int> UpdateAppointmentAsync(Appointment appointment);
    Task<int> DeleteAppointmentAsync(Appointment appointment);
}
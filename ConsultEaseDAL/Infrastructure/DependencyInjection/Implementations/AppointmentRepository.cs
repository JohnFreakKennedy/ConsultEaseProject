using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConsultEaseDAL.Context;
using ConsultEaseDAL.Entities;
using ConsultEaseDAL.Infrastructure.Implementations.Base;
using ConsultEaseDAL.Infrastructure.Abstractions;

namespace ConsultEaseDAL.Infrastructure.DependencyInjection.Implementations;

public class AppointmentRepository(ConsultEaseDbContext dbContext)
    : RepositoryBase<int, Appointment>(dbContext), IAppointmentRepository
{
    public async Task<Appointment?> FindByKeyAsync(int key) => 
        await dbContext.Appointments.FindAsync(key);

    public async Task<Appointment?> CreateAsync(Appointment entity)
    {
        var appointmentEntry = dbContext.Appointments.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return appointmentEntry.Result.Entity;
    }

    public async Task<int> UpdateAsync(Appointment entity) => 
        await UpdateAsync(entity);

    public async Task<int> DeleteAsync(Appointment entity) =>
        await DeleteAsync(entity);

    public async Task<Appointment?> GetAppointmentByIdAsync(int id) =>
        await FindByKeyAsync(id);
    
    public async Task<IEnumerable<Appointment>> GetAppointmentsByCounsellorIdAsync(int professorId) =>
        await dbContext.Appointments.Where(a => a.ProfessorId == professorId).ToListAsync();

    public async Task<IEnumerable<Appointment>> GetAppointmentsByStudentIdAsync(int studentId) => 
        await dbContext.Appointments.Where(a => a.StudentId == studentId).ToListAsync();

    public async Task<Appointment?> CreateAppointmentAsync(Appointment appointment) =>
        await CreateAsync(appointment);

    public async Task<int> UpdateAppointmentAsync(Appointment appointment) =>
        await UpdateAsync(appointment);

    public async Task<int> DeleteAppointmentAsync(Appointment appointment) =>
        await DeleteAsync(appointment);
}
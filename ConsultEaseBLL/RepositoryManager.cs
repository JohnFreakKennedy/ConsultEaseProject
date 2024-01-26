using ConsultEaseBLL.Interfaces;
using ConsultEaseDAL.Context;
using ConsultEaseDAL.Infrastructure.DependencyInjection.Abstractions;
using ConsultEaseDAL.Infrastructure.DependencyInjection.Implementations;

namespace ConsultEaseBLL;

public class RepositoryManager: IRepositoryManager
{
    private readonly ConsultEaseDbContext _consultEaseDbContext;
    private readonly IAppointmentRepository? _appointmentRepository;
    private readonly ICounsellingCategoryRepository? _counsellingCategoryRepository;
    public RepositoryManager(ConsultEaseDbContext consultEaseDbContext) =>
        _consultEaseDbContext = consultEaseDbContext;
    public IAppointmentRepository AppointmentRepository =>
        _appointmentRepository ?? new AppointmentRepository(_consultEaseDbContext);

    public ICounsellingCategoryRepository CounsellingCategoryRepository =>
        _counsellingCategoryRepository ?? new CounsellingCategoryRepository(_consultEaseDbContext);
}
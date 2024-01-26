using ConsultEaseDAL.Infrastructure.DependencyInjection.Abstractions;

namespace ConsultEaseBLL.Interfaces;

public interface IRepositoryManager
{
    IAppointmentRepository AppointmentRepository { get; }
    ICounsellingCategoryRepository CounsellingCategoryRepository { get; }
}
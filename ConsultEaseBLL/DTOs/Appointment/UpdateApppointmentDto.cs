using ConsultEaseDAL.Entities.Enums;
namespace ConsultEaseBLL.DTOs.Appointment;

public class UpdateApppointmentDto
{
    public AppointmentStatus Status { get; set; }
}
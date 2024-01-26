namespace ConsultEaseBLL.DTOs.Appointment;

public class CreateAppointmentDto
{
    public DateTime Date { get; set; }
    public int RequestedTime { get; set; }
    public int StudentId { get; set; }
    public int ProfessorId { get; set; }
    public int CounsellingCategoryId { get; set; }
}
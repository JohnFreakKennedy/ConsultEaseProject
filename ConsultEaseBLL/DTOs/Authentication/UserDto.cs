using AutoMapper.Configuration.Annotations;

namespace ConsultEaseBLL.DTOs.Authentication;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    
    public ICollection<ConsultEaseDAL.Entities.Appointment> Appointments { get; set; }
    
    [Ignore] public string? Role { get; set; }
}
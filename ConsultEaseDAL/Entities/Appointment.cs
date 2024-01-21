using System;
using ConsultEaseDAL.Entities.Auth;
using ConsultEaseDAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultEaseDAL.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateOnly AppointmentDate { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public int Priority { get; set; }
        public int AffectedTimeDuration { get; set; }
        public string? AppointmentDescription { get; set; }
        
        public int? CounsellingCategoryId { get; set; }
        public CounsellingCategory? CounsellingCategory { get; set; }
        
        public int? ProfessorId { get; set; }
        public User? Professor { get; set; }
        
        public int? StudentId { get; set; }
        public User? Student { get; set; }
    }
}

using System;
using ConsultEaseDAL.Entities.Auth;
using ConsultEaseDAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int RequestedTime { get; set; }
        [MaxLength(200)]
        public string? AppointmentDescription { get; set; }
        
        [ForeignKey("CounsellingCategoryId")]
        public int? CounsellingCategoryId { get; set; }
        public CounsellingCategory? CounsellingCategory { get; set; }
        
        [ForeignKey("ProfessorId")]
        public int? ProfessorId { get; set; }
        public User? Professor { get; set; }
        
        [ForeignKey("StudentId")]
        public int? StudentId { get; set; }
        public User? Student { get; set; }
    }
}

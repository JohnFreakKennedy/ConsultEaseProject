using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsultEaseDAL.Entities
{
    public class CounsellingCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int AffectTimeDuration { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ConsultEaseDAL.Entities.Auth
{
    public class User:IdentityUser<int>
    {
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(20)]
        public string MiddleName { get; set; }
        [MaxLength(20)]
        public string LastName  { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.Library.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string? Gender { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phonenumber { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public bool IsAdmin { get; set; }
    }
}

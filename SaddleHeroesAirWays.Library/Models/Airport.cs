using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.Library.Models
{
    internal class Airport
    {
        public int Id { get; set; }

        [Required]
        public string? IATACode { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public ICollection<Flight>? DepartingFlights { get; set; }
        public ICollection<Flight>? ArrivingFlights { get; set; }
    }
}

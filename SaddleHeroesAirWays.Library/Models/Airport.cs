using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.Library.Models
{
     public class Airport
    {
        public int Id { get; set; }

        [Required]
        public string? IATACode { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        [InverseProperty("DepartureAirport")]
        public ICollection<Flight>? DepartingFlights { get; set; }

        [InverseProperty("ArrivalAirport")]
        public ICollection<Flight>? ArrivingFlights { get; set; }
    }
}

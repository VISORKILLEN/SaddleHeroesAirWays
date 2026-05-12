using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.Library.Models
{
    internal class Booking
    {
        [Required]
        [Key]
        public string? BookingReference { get; set; }

        public int UserId { get; set; }
        public int FlightId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }

        public enum Status
        { 
            Confirmed,
            Cancelled,
            Rebooked
        }
    }
}

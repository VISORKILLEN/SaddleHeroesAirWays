using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.Library.Models
{
     public class BookingDetails
    {
        public int Id { get; set; }
        public string? BookingReference { get; set; }
        public string? Seatnumber { get; set; }
        public bool Baggage { get; set; }
        public string? Notes { get; set; }

        public Booking? Booking { get; set; }
    }
}

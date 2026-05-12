using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaddleHeroesAirWays.Library.Models
{
    internal class Flight
    {
        public int Id { get; set; }
        public string? FlightNumber { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }
        public int TotalSeats { get; set; }
        public decimal Price { get; set; }
        public FlightStatus FlightStatus { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }

    public enum FlightStatus
    {
        Arrived, 
        Delayed, 
        Cancelled
    }
}

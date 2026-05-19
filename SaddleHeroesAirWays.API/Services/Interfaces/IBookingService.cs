using SaddleHeroesAirWays.Library.Models;
﻿using SaddleHeroesAirWays.API.DTOs;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponse>> GetBookingsForWeekAsync(DateTime date);
        Task<IEnumerable<BookingResponse>> GetBookingsForMonthAsync(DateTime date);
        Task<IEnumerable<BookingResponse>> GetBookingsByUserIdAsync(int userId);
    }
}

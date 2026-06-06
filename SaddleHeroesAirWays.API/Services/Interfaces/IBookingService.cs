using SaddleHeroesAirWays.Library.Models;
﻿using SaddleHeroesAirWays.API.DTOs;

namespace SaddleHeroesAirWays.API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponse>> GetBookingsForWeekAsync(DateTime date);
        Task<IEnumerable<BookingResponse>> GetBookingsForMonthAsync(DateTime date);
        Task<IEnumerable<BookingResponse>> GetBookingsByUserIdAsync(int userId);

        Task<ServiceResult<BookingResponse>> GetBookingByBookingReferenceAsync(string bookingReference);
        Task<IEnumerable<BookingResponse>> GetAllBookingsMadeAsync();
        Task<IEnumerable<BookingResponse>> GetBookingsForDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ServiceResult<BookingResponse>> CreateBookingAsync(CreateBookingRequest request);
        Task<ServiceResult<BookingResponse?>> UpdateBookingAsync(string bookingReference, UpdateBooking updateBooking);

        Task<ServiceResult<bool>> DeleteBookingPermanentlyAsync(string bookingReference);
        Task<ServiceResult<bool>> CancelBookingAsync(string bookingReference);
    }
}

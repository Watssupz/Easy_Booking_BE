using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;

namespace Easy_Booking_BE.Repositories;

public interface IBookingsRepository
{
    public Task<BaseDataResponse<List<BookingModel>>> GetAllBookingsAsync();
    public Task<BaseDataResponse<List<BookingModel>>> GetBookingByRoomIdAsync(int room_id);
    public Task<BaseDataResponse<object>> AddBookingAsync(BookingCreateModel booking);
    // public Task<BaseDataResponse<object>> UpdateBookingAsync(int id, BookingModel booking);
    // public Task<BaseDataResponse<object>> DeleteBookingAsync(int id);
}
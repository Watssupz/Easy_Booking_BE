using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;

namespace Easy_Booking_BE.Repositories;

public interface IBookingsRepository
{
    public Task<BaseDataResponse<List<BookingModel>>> GetAllBookingsAsync();
    public Task<BaseDataResponse<List<BookingModel>>> GetBookingByRoomIdAsync(int room_id);
    public Task<BaseDataResponse<object>> AddBookingAsync(BookingCreateModel booking);
    public Task<BaseDataResponse<List<object>>> MyBookingAsync();
    public Task<BaseDataResponse<object>> ConfirmBookingAsync(int booking_id);
    public Task<BaseDataResponse<object>> CancelBookingAsync(int booking_id);
    public Task<BaseDataResponse<List<object>>> GetListBookingByRooms();
    public Task<BaseDataResponse<object>> CheckInBooking(int booking_id);
    public Task<BaseDataResponse<object>> CheckOutBooking(int booking_id);
}
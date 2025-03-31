using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Easy_Booking_BE.Utilities;
using EasyBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class BookingRepository : IBookingsRepository
{
    private readonly EasyBookingBEContext _context;
    private readonly IMapper _mapper;
    private readonly Util _util;

    public BookingRepository(EasyBookingBEContext context, IMapper mapper, Util util)
    {
        _context = context;
        _mapper = mapper;
        _util = util;
    }

    public async Task<BaseDataResponse<List<BookingModel>>> GetAllBookingsAsync()
    {
        var b = await _context.Booking!.ToListAsync();
        var mappedData = _mapper.Map<List<BookingModel>>(b);
        return new BaseDataResponse<List<BookingModel>>
        (
            statusCode: 200,
            message: Constants.SUCCESSFUL,
            data: mappedData != null && mappedData.Any() ? mappedData : new List<BookingModel>()
        );
    }

    public async Task<BaseDataResponse<List<BookingModel>>> GetBookingByRoomIdAsync(int room_id)
    {
        try
        {
            var exist = await _context.Room.FirstOrDefaultAsync(r => r.room_id == room_id);
            if (exist == null)
            {
                return new BaseDataResponse<List<BookingModel>>(
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                );
            }

            var currentDate = DateTime.Today;
            var list_booking = await _context.Booking_Rooms
                .Include(r => r.Room)
                .Include(b => b.Booking)
                .ThenInclude(p => p.Payment_Status)
                .Where(r => r.Room.room_id == room_id && r.Booking.end_date_booking >= currentDate && r.Booking.booking_status == 1)
                .Select(n => new BookingModel()
                {
                    booking_id = n.booking_id,
                    start_date_booking = n.Booking.start_date_booking,
                    end_date_booking = n.Booking.end_date_booking,
                    check_in = n.Booking.check_in,
                    check_out = n.Booking.check_out,
                    num_adults = n.Booking.num_adults,
                    num_children = n.Booking.num_children,
                    price = n.Booking.price,
                    user_id = n.Booking.user_id,
                    payment_status = n.Booking.Payment_Status.payment_status_name,
                    booking_status = n.Booking.Booking_Status.booking_status_name,
                })
                .ToListAsync();
            return new BaseDataResponse<List<BookingModel>>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: list_booking != null ? list_booking : new List<BookingModel>()
            );
        }
        catch (Exception e)
        {
            return new BaseDataResponse<List<BookingModel>>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<object>> AddBookingAsync(BookingCreateModel booking)
    {
        try
        {
            // Kiểm tra xem có booking nào đã tồn tại trong khoảng thời gian được chọn không
            bool isOverlapping = await _context.Booking_Rooms
                .Include(b => b.Booking)
                .Where(b => b.Room.room_id == booking.room_id)
                .AnyAsync(b => 
                    booking.start_date_booking < b.Booking.end_date_booking && 
                    booking.end_date_booking > b.Booking.start_date_booking
                );

            if (isOverlapping)
            {
                return new BaseDataResponse<object>(
                    statusCode: 400,
                    message: "Phòng này đã được đặt trong khoảng thời gian bạn chọn."
                );
            }
            var userId = await _util.GetUserIdFromTokenAsync();

            var newBooking = new Booking()
            {
                user_id = userId,
                start_date_booking = booking.start_date_booking,
                end_date_booking = booking.end_date_booking,
                num_adults = booking.num_adults,
                num_children = booking.num_children,
                price = booking.price,
                check_in = null,
                check_out = null,
                booking_status = 1,
                payment_status = booking.payment_status
            };
            _context.Booking.Add(newBooking);
            await _context.SaveChangesAsync(); // Lưu để lấy booking_id

            var booking_room = new Booking_Room()
            {
                booking_id = newBooking.booking_id,
                room_id = booking.room_id
            };
            
            _context.Booking_Rooms.Add(booking_room);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: new { booking_id = newBooking.booking_id }
            );
        }
        catch (Exception e)
        {
            return new BaseDataResponse<object>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<List<object>>> MyBookingAsync()
    {
        try
        {
            var userId = await _util.GetUserIdFromTokenAsync();
            
            var list_booking = await _context.Booking_Rooms
                .Include(r => r.Room)
                .Include(b => b.Booking)
                .ThenInclude(p => p.Payment_Status)
                .Where(r => r.Booking.user_id == userId)
                .Select(n => new
                {
                    booking_id = n.booking_id,
                    room_id = n.room_id,
                    room_title = n.Room.room_number,
                    location = n.Room.location,
                    start_date_booking = n.Booking.start_date_booking,
                    end_date_booking = n.Booking.end_date_booking,
                    check_in = n.Booking.check_in,
                    check_out = n.Booking.check_out,
                    num_adults = n.Booking.num_adults,
                    num_children = n.Booking.num_children,
                    price = n.Booking.price,
                    user_id = n.Booking.user_id,
                    payment_status = n.Booking.Payment_Status.payment_status_name,
                    booking_status = n.Booking.Booking_Status.booking_status_name,
                    thumbnail = n.Room.thumbnail,
                })
                .ToListAsync();
            var objectList = list_booking.Select(x => (object)x).ToList();
            return new BaseDataResponse<List<object>>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: objectList
            );
        }
        catch (Exception e)
        {
            return new BaseDataResponse<List<object>>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<object>> CancelBookingAsync(int booking_id)
    {
        try
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(b => b.booking_id == booking_id);
            if (booking == null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                );
            }

            booking.booking_status = 3;
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL
            );
        }
        catch (Exception e)
        {
            return new BaseDataResponse<object>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<List<object>>> GetListBookingByRooms()
    {
        try
        {
            var userId = await _util.GetUserIdFromTokenAsync();
            if (userId == null)
            {
                return new BaseDataResponse<List<object>>(
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                );
            }
            
            var list_booking = await _context.Booking_Rooms
                .Include(r => r.Room)
                .Include(b => b.Booking)
                .ThenInclude(p => p.Payment_Status)
                .Where(r => r.Room.user_id == userId)
                .Select(n => new
                {
                    booking_id = n.booking_id,
                    room_id = n.room_id,
                    room_title = n.Room.room_number,
                    location = n.Room.location,
                    start_date_booking = n.Booking.start_date_booking,
                    end_date_booking = n.Booking.end_date_booking,
                    check_in = n.Booking.check_in,
                    check_out = n.Booking.check_out,
                    num_adults = n.Booking.num_adults,
                    num_children = n.Booking.num_children,
                    price = n.Booking.price,
                    user_id = n.Booking.user_id,
                    username = _context.Users
                        .Where(u => u.Id == n.Booking.user_id)
                        .Select(u => u.first_name + " " + u.last_name)
                        .FirstOrDefault() ?? "Unknown",
                    payment_status = n.Booking.Payment_Status.payment_status_name,
                    booking_status = n.Booking.Booking_Status.booking_status_name,
                    thumbnail = n.Room.thumbnail,
                })
                .ToListAsync();
            var objectList = list_booking.Select(x => (object)x).ToList();
            return new BaseDataResponse<List<object>>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: objectList
            );
        }
        catch (Exception e)
        {
            return new BaseDataResponse<List<object>>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }
}
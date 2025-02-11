using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using EasyBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class Room_StatusRepository : IRoom_StatusRepository
{
    private readonly IMapper _mapper;
    private readonly EasyBookingBEContext _context;

    public Room_StatusRepository(EasyBookingBEContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseDataResponse<List<Room_StatusModel>>> GetAllRoom_Status()
    {
        var rs = await _context.Room_Status!.ToListAsync();
        var mappedData = _mapper.Map<List<Room_StatusModel>>(rs);
        return new BaseDataResponse<List<Room_StatusModel>>
        (
            statusCode: 200,
            message: Constants.SUCCESSFUL,
            data: mappedData != null && mappedData.Any() ? mappedData : new List<Room_StatusModel>()
        );
    }

    public async Task<BaseDataResponse<Room_StatusModel>> GetRoom_StatusById(int id)
    {
        var rs = await _context.Room_Status!.FindAsync(id);
        return new BaseDataResponse<Room_StatusModel>(
            statusCode: 200,
            message: rs != null ? Constants.SUCCESSFUL : Constants.NOT_FOUND,
            data: rs != null ? _mapper.Map<Room_StatusModel>(rs) : null
        );
    }

    public async Task<BaseDataResponse<object>> CreateRoom_Status(Room_StatusModel room_Status)
    {
        try
        {
            var exist = await _context.Room_Status!.FirstOrDefaultAsync(rs =>
                rs.room_status_name == room_Status.room_status_name);
            if (exist != null)
            {
                return new BaseDataResponse<object>
                (
                    statusCode: 200,
                    message: Constants.ALREADY_EXSIST,
                    data: exist
                );
            }

            var newRS = _mapper.Map<Room_Status>(room_Status);
            _context.Room_Status!.Add(newRS);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>
            (
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: newRS
            );
        }
        catch (Exception ex)
        {
            return new BaseDataResponse<object>(
                statusCode: 500,
                message: Constants.ERROR
            );
        }
    }

    public async Task<BaseDataResponse<object>> UpdateRoom_Status(int id, Room_StatusModel room_Status)
    {
        var updateRS = await _context.Room_Status!.FindAsync(id);
        if (updateRS != null)
        {
            var existRS = await _context.Room_Status.FirstOrDefaultAsync(rs =>
                rs.room_status_name == room_Status.room_status_name && rs.room_status_id != id);
            if (existRS != null)
            {
                return new BaseDataResponse<object>(
                    statusCode: 200,
                    message: Constants.ALREADY_EXSIST
                );
            }

            updateRS.room_status_name = room_Status.room_status_name;
            _context.Room_Status!.Update(updateRS);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL,
                data: updateRS
            );
        }

        return new BaseDataResponse<object>
        (
            statusCode: 404,
            message: Constants.NOT_FOUND
        );
    }

    public async Task<BaseDataResponse<object>> DeleteRoom_Status(int id)
    {
        var rs = await _context.Room_Status!.FirstOrDefaultAsync(rs =>
            rs.room_status_id == id);
        if (rs != null)
        {
            _context.Room_Status!.Remove(rs);
            await _context.SaveChangesAsync();
            return new BaseDataResponse<object>(
                statusCode: 200,
                message: Constants.SUCCESSFUL
            );
        }

        return new BaseDataResponse<object>
        (
            statusCode: 200,
            message: Constants.UNSUCCESSFUL
        );
    }
}